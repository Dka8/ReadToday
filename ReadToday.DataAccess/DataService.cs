using System;
using System.Collections.Generic;
using System.Linq;
using ReadToday.Model;

namespace ReadToday.DataAccess
{
    public class DataService : IDataService
    {
        public void DeleteBook(Guid bookId)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CLookupItem> GetAllBooks()
        {
            using (CReadTodayDbContext context = new CReadTodayDbContext())
            {
                return context.Books.AsNoTracking()
                    .Select(b => CLookupItem.Create(b.Id, b.Title)).ToList();
            }
        }

        public CBook GetBookById(Guid bookId)
        {
            throw new NotImplementedException();
        }

        public void SaveBook(CBook book)
        {
            throw new NotImplementedException();
        }
    }
}
