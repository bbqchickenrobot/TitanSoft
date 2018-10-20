using System;
using System.Threading.Tasks;

namespace TitanSoft.DataAccess
{
    public interface IFindByIdQuery<T, TKey>
            where TKey : IComparable // attempt to limit to primitive tyeps including string type
            where T : IPersistable
    {
        T FindById(TKey id);
        Task<T> FindByIdAsync(TKey id);
    }
}