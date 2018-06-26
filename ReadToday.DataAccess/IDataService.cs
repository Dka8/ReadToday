using System;
using System.Collections.Generic;
using ReadToday.Model;

namespace ReadToday.DataAccess
{
    public interface IDataService : IDisposable
    {
        CBook GetBookById(Guid bookId);

        IEnumerable<CLookupItem> GetAllBooks();

        void SaveBook(CBook book);

        void DeleteBook(Guid bookId);
    }
}
