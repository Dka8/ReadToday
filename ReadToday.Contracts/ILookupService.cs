using System.Collections.Generic;
using System.ServiceModel;

namespace ReadToday.Contracts
{
    [ServiceContract]
    public interface ILookupService
    {
        [OperationContract]
        IEnumerable<LookupItem> GetBookLookups();

        [OperationContract]
        IEnumerable<LookupItem> GetShelfLookups();

        [OperationContract]
        IEnumerable<LookupItem> GetLiteraryGenreLookups();
    }
}
