using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DinnerSpinner.Api.Domain.Contracts;
using DinnerSpinner.Domain.Model;
using DinnerSpinner.Web.Configuration;
using MongoDB.Driver;

namespace DinnerSpinner.Web.Domain.Services;

public class SpinnerService
{
    private readonly IMongoDatabase _database;
    private readonly IMongoCollection<Spinner> _spinners;
    private readonly IMongoCollection<User> _users;

    public SpinnerService(IDatabaseSettings settings)
    {
        _database = new MongoClient(settings.ConnectionString).GetDatabase(settings.DatabaseName);
        _spinners = _database.GetCollection<Spinner>(nameof(Spinner));
    }

    public async Task< List<Spinner>> Get()
        => await _spinners.Find(spinner => true).ToListAsync();

    public async Task<Spinner> Get(Guid id) 
        => await _spinners.Find<Spinner>(spinner => spinner.Id == id).FirstOrDefaultAsync();

    public async Task<Spinner> Create(CreateSpinner createSpinner)
    {
        using var session = await _database.Client.StartSessionAsync();

        try
        {
            var spinner = new Spinner
            {
                Id = Guid.NewGuid(),
                Name = createSpinner.Name,
                Version = 1,
                Members = new List<UserRef>
                {
                    new UserRef
                    {
                        Name = createSpinner.OwnerName,
                        Email = createSpinner.OwnerEmail
                    }
                }
            };

            await _spinners.InsertOneAsync(session, spinner);

            return spinner;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    public Task UpdateAsync(Guid id, Spinner spinnerIn)
    {
        // spinnerIn.Version
        // Do a concurrency check?
        return _spinners.ReplaceOneAsync(spinner => spinner.Id == id, spinnerIn);
    }

    public async Task<Spinner> Remove(Guid id)
    {
        var spinner = await Get(id);
        if ((await _spinners.DeleteOneAsync(s => s.Id == id)).IsAcknowledged)
        {
            return spinner;
        }

        return default;
    }

    public async Task<Spinner> AddDinner(Guid spinnerId, string name, List<string> ingredients)
    {
        var spinner = await Get(spinnerId);

        var dinner = new Dinner
        {
            Name = name,
            Id = Guid.NewGuid(),
            Ingredients = ingredients.Select(i => new Ingredient(i)).ToList(),
            SpinnerRef = new SpinnerRef
            {
                Id = spinner.Id,
                Name = spinner.Name
            }
        };

        spinner.Dinners.Add(dinner);

        await UpdateAsync(spinner.Id, spinner);

        return spinner;
    }

    public async Task<Spinner> RemoveDinner(Guid spinnerId, Guid dinnerId)
    {
        var spinner = await Get(spinnerId);

        spinner.Dinners = spinner.Dinners.Where(d => d.Id != dinnerId).ToList();

        await UpdateAsync(spinner.Id, spinner);

        return spinner;
    }
}
