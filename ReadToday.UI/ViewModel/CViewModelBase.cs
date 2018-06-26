using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ReadToday.UI.Annotations;

namespace ReadToday.UI.ViewModel
{
    public class CViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] String propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
