using System.Collections.Generic;
using ReadToday.Model;

namespace ReadToday.UI.DataProvider
{
    public interface IShelfLookupItemDataService
    {
        IEnumerable<CLookupItem> GetShelfLookup();
    }
}
