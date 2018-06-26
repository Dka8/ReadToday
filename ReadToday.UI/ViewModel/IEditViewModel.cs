using System;

namespace ReadToday.UI.ViewModel
{
    public interface IEditViewModel
    {
        Guid Id { get; }
        void Load(Guid? id);
        Boolean HasChanges { get; }
    }
}
