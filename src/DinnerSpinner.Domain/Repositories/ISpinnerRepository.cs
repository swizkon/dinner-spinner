using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DinnerSpinner.Domain.Model;

namespace DinnerSpinner.Domain.Repositories
{
    public interface ISpinnerRepository
    {
        Task<IList<Spinner>> GetAll();
        Task<Spinner> GetById(Guid id);
        Task Save(Spinner s);
        Task<Spinner> RemoveById(Guid id);
    }
}
