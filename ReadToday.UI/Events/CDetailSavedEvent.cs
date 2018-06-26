using System;
using Prism.Events;

namespace ReadToday.UI.Events
{
    public class CDetailSavedEvent : PubSubEvent<DetailSavedEventArgs>
    {
    }

    public class DetailSavedEventArgs
    {
        public DetailSavedEventArgs(Guid id, String displayMember, String viewModelName)
        {
            Id = id;
            DisplayMember = displayMember;
            ViewModelName = viewModelName;
        }

        public Guid Id { get; set; }
        public String DisplayMember { get; set; }
        public String ViewModelName { get; set; }
    }
}
