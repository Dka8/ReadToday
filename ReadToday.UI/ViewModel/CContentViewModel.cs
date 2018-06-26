using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Autofac.Features.Indexed;
using Prism.Events;
using ReadToday.UI.Command;
using ReadToday.UI.Events;

namespace ReadToday.UI.ViewModel
{
    public class CContentViewModel : CViewModelBase, IContentViewModel
    {
        private IEditViewModel _selectedEditViewModel;
        private IIndex<String, IEditViewModel> _editViewModelCreator;

        public CContentViewModel(INavigationViewModel navigationViewModel,
            IIndex<String, IEditViewModel> editViewmModelCreator,
            IEventAggregator eventAggregator)
        {
            NavigationViewModel = navigationViewModel;
            _editViewModelCreator = editViewmModelCreator;
            eventAggregator.GetEvent<COpenEditViewEvent>().Subscribe(OnOpenEditView);
            eventAggregator.GetEvent<CDetailDeletedEvent>().Subscribe(OnDetailDeleted);

            EditViewModels = new ObservableCollection<IEditViewModel>();
            CloseBookTabCommand = new CDelegateCommand(OnCloseBookTabExecute);
            AddBookCommand = new CDelegateCommand(OnAddBookExecute);
            AddShelfCommand = new CDelegateCommand(OnAddShelfExecute);
        }

        public INavigationViewModel NavigationViewModel { get; }

        public ObservableCollection<IEditViewModel> EditViewModels { get; }

        public IEditViewModel SelectedEditViewModel {
            get => _selectedEditViewModel;
            set
            {
                _selectedEditViewModel = value;
                OnPropertyChanged();
            }
        }

        public ICommand CloseBookTabCommand { get; }

        public ICommand AddBookCommand { get; }
        public ICommand AddShelfCommand { get; }

        public void Load(Guid? userId)
        {
            NavigationViewModel.Load();
        }

        private void OnCloseBookTabExecute(Object obj)
        {
            IEditViewModel editViewModel = (IEditViewModel)obj;
            EditViewModels.Remove(editViewModel);
        }

        private void OnAddBookExecute(Object obj)
        {
            SelectedEditViewModel = CreateEditViewModel(nameof(CBookEditViewModel));
            SelectedEditViewModel.Load(null);
            EditViewModels.Add(SelectedEditViewModel);
        }

        private void OnAddShelfExecute(Object obj)
        {
            SelectedEditViewModel = CreateEditViewModel(nameof(CShelfEditViewModel));
            SelectedEditViewModel.Load(null);
            EditViewModels.Add(SelectedEditViewModel);
        }

        private void OnDetailDeleted(DelailDeletedEventArgs args)
        {
            IEditViewModel bookEditVm = EditViewModels.Single(vm => vm.Id == args.Id);
            EditViewModels.Remove(bookEditVm);
        }

        private void OnOpenEditView(OpenEditViewEventArgs args)
        {
            IEditViewModel editViewModel = EditViewModels.SingleOrDefault(vm => vm.Id == args.Id);

            if (editViewModel == null)
            {
                editViewModel = CreateEditViewModel(args.ViewModelName);
                editViewModel.Load(args.Id);
                EditViewModels.Add(editViewModel);
            }

            SelectedEditViewModel = editViewModel;
        }

        private IEditViewModel CreateEditViewModel(String editViewModel)
        {
            return _editViewModelCreator[editViewModel];
        }
    }
}
