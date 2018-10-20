using System;
using System.Threading.Tasks;

namespace TitanSoft.DataAccess
{
    public interface IPersistenceCommand<T>  where T : IPersistable
    {
        void Store(T param);
        Task StoreAsync(T param);
    }
}