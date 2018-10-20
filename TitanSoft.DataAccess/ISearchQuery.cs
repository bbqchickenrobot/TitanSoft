using System.Collections.Generic;
using System.Threading.Tasks;

namespace TitanSoft.DataAccess
{
    public interface ISearchQuery<T>
    {
        Task<List<T>> SearchAsync(string term);
    }
}
