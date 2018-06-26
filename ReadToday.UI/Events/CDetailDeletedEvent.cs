using System;
using Prism.Events;

namespace ReadToday.UI.Events
{
    public class CDetailDeletedEvent : PubSubEvent<DelailDeletedEventArgs>
    {

    }

    public class DelailDeletedEventArgs
    {
        public DelailDeletedEventArgs(Guid id, String viewModelName)
        {
            Id = id;
            ViewModelName = viewModelName;
        }

        public Guid Id { get; set; }
        public String ViewModelName { get; set; }
    }
}
