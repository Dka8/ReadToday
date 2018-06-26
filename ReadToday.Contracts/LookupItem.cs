using System;
using System.Runtime.Serialization;

namespace ReadToday.Contracts
{
    [DataContract]
    public class LookupItem
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public String DisplayMember { get; set; }
    }
}
