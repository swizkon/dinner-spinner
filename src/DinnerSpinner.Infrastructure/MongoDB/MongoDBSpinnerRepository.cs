using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DinnerSpinner.Domain.Contracts;
using DinnerSpinner.Domain.Model;
using DinnerSpinner.Domain.Repositories;
using MongoDB.Driver;

namespace DinnerSpinner.Infrastructure.MongoDB;

public class MongoDbSpinnerRepository : ISpinnerRepository
{
    private readonly IMongoDatabase _database;
    private readonly IMongoCollection<Spinner> _spinners;
    private readonly IMongoCollection<User> _users;

    public MongoDbSpinnerRepository(IDatabaseSettings settings)
    {
        _database = new MongoClient(settings.ConnectionString).GetDatabase(settings.DatabaseName);
        _spinners = _database.GetCollection<Spinner>(nameof(Spinner));
    }

    public async Task<IList<Spinner>> GetAll()
        => await _spinners.Find(spinner => true).ToListAsync();

    public async Task<Spinner> GetById(Guid id)
        => await _spinners.Find<Spinner>(spinner => spinner.Id == id).FirstOrDefaultAsync();

    public async Task Save(Spinner s)
    {
        using var session = await _database.Client.StartSessionAsync();
        var replace = await _spinners.ReplaceOneAsync(session, spinner => spinner.Id == s.Id, s);
        
        if (replace.ModifiedCount == 0)
            await _spinners.InsertOneAsync(session, s);
    }
    public async Task<Spinner> RemoveById(Guid id)
    {
        var spinner = await GetById(id);
        if ((await _spinners.DeleteOneAsync(s => s.Id == id)).IsAcknowledged)
        {
            return spinner;
        }

        return default;
    }
}

/*
public class SpinnerService
{
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

    //public Task UpdateAsync(Guid id, Spinner spinnerIn)
    //{
    //    // spinnerIn.Version
    //    // Do a concurrency check?
    //    return _spinners.ReplaceOneAsync(spinner => spinner.Id == id, spinnerIn);
    //}

    public async Task<Spinner> Remove(Guid id)
    {
        var spinner = await Get(id);
        if ((await _spinners.DeleteOneAsync(s => s.Id == id)).IsAcknowledged)
        {
            return spinner;
        }

        return default;
    }
    
}
*/