using ReadToday.Contracts;
using System.Collections.Generic;
using System.ServiceModel;

namespace ReadToday.UI.DataProvider
{
    public class LookupClient :
        ClientBase<ILookupService>, ILookupService
    {
        public IEnumerable<LookupItem> GetBookLookups()
        {
            return Channel.GetBookLookups();
        }

        public IEnumerable<LookupItem> GetShelfLookups()
        {
            return Channel.GetShelfLookups();
        }

        public IEnumerable<LookupItem> GetLiteraryGenreLookups()
        {
            return Channel.GetLiteraryGenreLookups();
        }
    }

}
