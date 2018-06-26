using ReadToday.DataAccess;
using ReadToday.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ReadToday.UI.DataProvider
{
    public class CShelfRepository : CGenericRepository<CShelf, CReadTodayDbContext>,
        IShelfRepository
    {
        public CShelfRepository(CReadTodayDbContext contex) : base(contex)
        {
        }

        public List<CBook> GetAllBooks()
        {
            return Context.Set<CBook>().ToList();
        }

        public override CShelf GetById(Guid id)
        {
            return Context.Shelfs
                .Include(sh => sh.Books)
                .Single(sh => sh.Id == id);
        }
    }
}
