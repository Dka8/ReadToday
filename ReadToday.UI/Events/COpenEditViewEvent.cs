using System;
using Prism.Events;


namespace ReadToday.UI.Events
{
    public class COpenEditViewEvent : PubSubEvent<OpenEditViewEventArgs>
    {
    }

    public class OpenEditViewEventArgs
    {
        public Guid? Id { get; set; }
        public String ViewModelName { get; set; }
    }
}
