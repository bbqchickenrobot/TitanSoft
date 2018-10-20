using System.Collections.Generic;
using System.Threading.Tasks;

namespace TitanSoft.DataAccess
{
    public interface IPagedSearchQuery<T>
    {
        Task<List<T>> SearchAsync(string term, int page = 1, int recs = 20);
    }
}
