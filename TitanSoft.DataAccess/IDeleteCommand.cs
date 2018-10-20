using System.Threading.Tasks;

namespace TitanSoft.DataAccess
{
    public interface IDeleteCommand<T> where T : IPersistable
    {
        void Delete(T param);
        Task DeleteAsync(T param);
    }
}