using ReadToday.Contracts;
using ReadToday.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ReadToday.Services
{
    public class LookupManager : ILookupService
    {
        public IEnumerable<LookupItem> GetBookLookups()
        {
            using (CReadTodayDbContext context = new CReadTodayDbContext())
            {
                return context.Books.AsNoTracking()
                    .Select(b => new LookupItem
                    {
                        Id = b.Id,
                        DisplayMember = b.Title
                    })
                    .ToList();
            }
        }

        public IEnumerable<LookupItem> GetShelfLookups()
        {
            using (CReadTodayDbContext context = new CReadTodayDbContext())
            {
                return context.Shelfs.AsNoTracking()
                    .Select(b => new LookupItem
                    {
                        Id = b.Id,
                        DisplayMember = b.Name
                    })
                    .ToList();
            }
        }

        public IEnumerable<LookupItem> GetLiteraryGenreLookups()
        {
            using (CReadTodayDbContext context = new CReadTodayDbContext())
            {
                return context.LiteraryGenres.AsNoTracking()
                    .Select(b => new LookupItem
                    {
                        Id = b.Id,
                        DisplayMember = b.Name
                    })
                    .ToList();
            }
        }
    }
}
