using System;
using System.Collections.Generic;
using System.Linq;
using ReadToday.DataAccess;
using ReadToday.Model;

namespace ReadToday.UI.DataProvider
{
    class CLookupDataService : 
        IBookLookupDataService,
        ILiteraryGenreLookupDataService,
        IShelfLookupItemDataService
    {
        private readonly Func<CReadTodayDbContext> _contextCreator;

        public CLookupDataService(Func<CReadTodayDbContext> contextCreator)
        {
            _contextCreator = contextCreator;
        }

        public IEnumerable<CLookupItem> GetBookLookup()
        {
            using (CReadTodayDbContext context = _contextCreator())
            {
                return context.Books.AsNoTracking()
                    .Select(b => new CLookupItem
                    {
                        Id = b.Id,
                        DisplayMember = b.Title
                    })
                    .ToList();
            }
        }

        public IEnumerable<CLookupItem> GetLiteraryGenreLookup()
        {
            using (CReadTodayDbContext context = _contextCreator())
            {
                return context.LiteraryGenres.AsNoTracking()
                    .Select(b => new CLookupItem
                    {
                        Id = b.Id,
                        DisplayMember = b.Name
                    })
                    .ToList();
            }
        }

        public IEnumerable<CLookupItem> GetShelfLookup()
        {
            using(CReadTodayDbContext context = _contextCreator())
            {
                return context.Shelfs.AsNoTracking()
                    .Select(sh => new CLookupItem
                    {
                        Id = sh.Id,
                        DisplayMember = sh.Name
                    })
                    .ToList();
            }
        }
    }
}
