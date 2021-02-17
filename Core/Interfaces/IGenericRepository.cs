using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;


namespace Core.Interfaces
{
    public interface IGenericRepository<T> where T : BaseClass
    {
         Task<T> GetByIdAsync(int id);
         Task<IReadOnlyList<T>> ListAllAsync();        
        
    }
}