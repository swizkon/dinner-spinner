using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DinnerSpinner.Domain.Contracts;
using DinnerSpinner.Domain.Model;
using DinnerSpinner.Domain.Repositories;

namespace DinnerSpinner.Domain.DomainServices;

public class SpinnerService
{
    private readonly ISpinnerRepository _repository;

    public SpinnerService(ISpinnerRepository repository)
    {
        _repository = repository;
    }

    public async Task<IList<Spinner>> Get()
        => await _repository.GetAll();

    public async Task<Spinner> Get(Guid id)
        => await _repository.GetById(id);

    public async Task<Spinner> Create(CreateSpinner createSpinner)
    {
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

            await _repository.Save(spinner);

            return spinner;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    public async Task UpdateAsync(Guid id, Spinner spinnerIn)
    {
        await _repository.Save(spinnerIn);
    }

    public async Task<Spinner> Remove(Guid id)
    {
        return await _repository.RemoveById(id);
    }

    public async Task<Spinner> AddDinner(Guid spinnerId, string name, List<string> ingredients)
    {
        var spinner = await Get(spinnerId);

        var dinner = new Dinner
        {
            Name = name,
            Id = Guid.NewGuid(),
            Ingredients = ingredients.Select(i => new Ingredient(i)).ToList(),
            //SpinnerRef = new SpinnerRef
            //{
            //    Id = spinner.Id,
            //    Name = spinner.Name
            //}
        };

        spinner.Dinners.Add(dinner);

        await UpdateAsync(spinner.Id, spinner);

        return spinner;
    }

    public async Task<Spinner> UpdateDinner(Guid spinnerId, Guid dinnerId, string name)
    {
        var spinner = await Get(spinnerId);

        var dinner = spinner.Dinners.FirstOrDefault(d => d.Id == dinnerId);
        if(dinner != null)
            dinner.Name = name;

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

    public async Task<IList<Spinner>> GetAll()
        => await _repository.GetAll().ConfigureAwait(false);
}
