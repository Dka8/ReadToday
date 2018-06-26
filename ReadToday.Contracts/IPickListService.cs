using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace ReadToday.Contracts
{
    [ServiceContract]
    public interface IPickListService
    {
        [OperationContract]
        IEnumerable<LookupItem> GetBookOnShelf(String shelf);

        [OperationContract]
        IEnumerable<LookupItem> GetBookNotOnShelf(String shelf);

        [OperationContract]
        void UpdateShelf(String shelf, IEnumerable<String> books);
    }
}
