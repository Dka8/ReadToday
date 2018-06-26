using System;
using Autofac.Features.Indexed;
using Prism.Events;
using ReadToday.UI.Events;

namespace ReadToday.UI.ViewModel
{
    public class CMainWindowModel : CViewModelBase
    {
        private IIndex<String, IContentViewModel> _contentViewModelCreator;
        private IContentViewModel _contentViewModel;


        public CMainWindowModel(IEventAggregator eventAggregator,
            IIndex<String,IContentViewModel> contentViewModelCreator)
        {
            eventAggregator.GetEvent<CUserLoginEvent>().Subscribe(OnUserLogin);
            _contentViewModelCreator = contentViewModelCreator;
            ContentViewModel = _contentViewModelCreator[nameof(CUserLoginViewModel)];
        }

        private void OnUserLogin(Guid userId)
        {
            ContentViewModel = _contentViewModelCreator[nameof(CContentViewModel)];
            ContentViewModel.Load(userId);
        }

        public void Load()
        {
            ContentViewModel.Load(null);
        }

        public IContentViewModel ContentViewModel {
            get => _contentViewModel;
            set
            {
                _contentViewModel = value;
                OnPropertyChanged();
            }
        }
    }
}
