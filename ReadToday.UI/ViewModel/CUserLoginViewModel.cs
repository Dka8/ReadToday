using System;
using System.ComponentModel;
using Prism.Events;
using ReadToday.UI.Command;
using ReadToday.UI.DataProvider;
using ReadToday.UI.Events;

namespace ReadToday.UI.ViewModel
{
    public class CUserLoginViewModel : CViewModelBase, IContentViewModel
    {
        private CUserDataProvider _dataProvider;
        private IEventAggregator _eventAggregator;
        private String _login;

        public CUserLoginViewModel(CUserDataProvider dataProvider,
            IEventAggregator eventAggregator)
        {
            _dataProvider = dataProvider;
            _eventAggregator = eventAggregator;
            UserLoginCommand = new CDelegateCommand(OnLoginExecute, OnLoginCanExecute);
            PropertyChanged += Login_PropertyChanged;
        }

        public CDelegateCommand UserLoginCommand { get; }

        public void Load(Guid? userId)
        {
            _login = String.Empty;
        }

        public String Login
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged();
            }
        }

        private Boolean OnLoginCanExecute(Object arg)
        {
            return !String.IsNullOrWhiteSpace(Login) && Login.Length > 3;
        }

        private void OnLoginExecute(Object obj)
        {
            Guid userId = _dataProvider.GetUserId(Login);
            _eventAggregator.GetEvent<CUserLoginEvent>().Publish(userId);
        }

        private void Login_PropertyChanged(Object sender, PropertyChangedEventArgs e)
        {
            ((CDelegateCommand) UserLoginCommand).RaiseCanExecuteChanged();
        }
    }
}
