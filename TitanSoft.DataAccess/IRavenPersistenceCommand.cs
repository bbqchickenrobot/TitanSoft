using System.Threading.Tasks;

namespace TitanSoft.DataAccess
{
    public interface IRavenPersistenceCommand<T> : IPersistenceCommand<T> where T : IPersistable
    {
        void Update(T param);
        Task UpdateAsync(T param);
    }
}