using System.Collections.Generic;
using ReadToday.Model;

namespace ReadToday.UI.DataProvider
{
    public interface IShelfRepository : IGenericRepository<CShelf>
    {
        List<CBook> GetAllBooks();
    }
}