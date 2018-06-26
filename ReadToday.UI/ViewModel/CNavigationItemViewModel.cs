using System;
using System.Windows.Input;
using Prism.Commands;
using Prism.Events;
using ReadToday.UI.Events;

namespace ReadToday.UI.ViewModel
{
    public class CNavigationItemViewModel : CViewModelBase {

        private String _editViewModelName;
        private String _title;
        private IEventAggregator _eventAggregator;

        public CNavigationItemViewModel(Guid id, String title,
            String editViewModelName,
            IEventAggregator eventAggregator)
        {
            Id = id;
            DisplayMember = title;
            _editViewModelName = editViewModelName;
            _eventAggregator = eventAggregator;
            OpenEditViewCommand = new DelegateCommand(OnOpenEditViewExecute);
        } 

        public ICommand OpenEditViewCommand { get; }
        public Guid Id { get; }
        public String DisplayMember
        {
            get =>_title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        private void OnOpenEditViewExecute()
        {
            _eventAggregator.GetEvent<COpenEditViewEvent>()
                .Publish(
               new OpenEditViewEventArgs
               {
                   Id = Id,
                   ViewModelName = _editViewModelName
               });
        }
    }
}
