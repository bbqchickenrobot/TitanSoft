using System.Collections.Generic;
using System.Threading.Tasks;

namespace TitanSoft.DataAccess
{
    public interface IGetAllQuery<T> where T : IPersistable
    {
        List<T> GetAll();
        Task<List<T>> GetAllAsync();
    }
}
