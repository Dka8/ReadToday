using System;
using System.Data.Entity;
using System.Linq;
using ReadToday.DataAccess;
using ReadToday.Model;

namespace ReadToday.UI.DataProvider
{
    public class CBookRepository : CGenericRepository<CBook, CReadTodayDbContext>,
                                  IBookRepository
    {
        public CBookRepository(CReadTodayDbContext context) : base(context) { }

        public override CBook GetById(Guid bookId)
        {
            return Context.Books
                .Include(b => b.Characters)
                .Single(b => b.Id == bookId);
        }

        public void RemoveCharacter(CCharacter character)
        {
            Context.Characters.Remove(character);
        }
    }
}
