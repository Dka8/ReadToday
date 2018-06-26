using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Prism.Events;
using ReadToday.Model;
using ReadToday.UI.Command;
using ReadToday.UI.DataProvider;
using ReadToday.UI.Dialogs;
using ReadToday.UI.Wrapper;

namespace ReadToday.UI.ViewModel
{
    public class CShelfEditViewModel : CEditViewModelBase, IShelfEditViewModel
    {
        private IShelfRepository _shelfRepository;
        private CShelfWrapper _shelf;
        private IMessageDialogService _messageDialogService;
        private CBook _selectedAvailableBook;
        private CBook _selectedAddedBook;
        private List<CBook> _allBooks;

        public CShelfEditViewModel(IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService,
            IShelfRepository shelfRepository) : base(eventAggregator)
        {
            _shelfRepository = shelfRepository;
            _messageDialogService = messageDialogService;

            AddedBooks = new ObservableCollection<CBook>();
            AvailableBooks = new ObservableCollection<CBook>();
            AddBookCommand = new CDelegateCommand(OnAddBookExecute, OnAddBookCanExecute);
            RemoveBookCommand = new CDelegateCommand(OnRemoveBookExecute, OnRemoveBookCanExecute);
        }

        private Boolean OnRemoveBookCanExecute(Object arg)
        {
            return SelectedAddedBook != null;
        }

        private void OnRemoveBookExecute(Object obj)
        {
            CBook bookToRemove = SelectedAddedBook;

            Shelf.Model.Books.Remove(bookToRemove);
            AddedBooks.Remove(bookToRemove);
            AvailableBooks.Add(bookToRemove);
            HasChanges = _shelfRepository.HasChanges();
            ((CDelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private Boolean OnAddBookCanExecute(Object arg)
        {
            return SelectedAvailableBook != null;
        }

        private void OnAddBookExecute(Object obj)
        {
            CBook bookToAdd = SelectedAvailableBook;

            Shelf.Model.Books.Add(bookToAdd);
            AddedBooks.Add(bookToAdd);
            AvailableBooks.Remove(bookToAdd);
            HasChanges = _shelfRepository.HasChanges();
            ((CDelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        public CShelfWrapper Shelf
        {
            get => _shelf;
            private set
            {
                _shelf = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<CBook> AddedBooks { get; }

        public ObservableCollection<CBook> AvailableBooks { get; }

        public ICommand AddBookCommand { get; }
        public ICommand RemoveBookCommand { get; }

        public CBook SelectedAvailableBook
        {
            get { return _selectedAvailableBook; }
            set
            {
                _selectedAvailableBook = value;
                OnPropertyChanged();
                ((CDelegateCommand)AddBookCommand).RaiseCanExecuteChanged();
            }
        }

        public CBook SelectedAddedBook
        {
            get { return _selectedAddedBook; }
            set
            {
                _selectedAddedBook = value;
                OnPropertyChanged();
                ((CDelegateCommand)RemoveBookCommand).RaiseCanExecuteChanged();
            }
        }

        public override Guid Id => Shelf.Id;

        public override void Load(Guid? id)
        {
            CShelf shelf = id.HasValue
                ? _shelfRepository.GetById(id.Value)
                : CreateNewShelf();

            InitializeShelf(shelf);

            _allBooks = _shelfRepository.GetAllBooks();

            SetupPickList();
        }

        private void SetupPickList()
        {
            List<Guid> shelfsBookIds = Shelf.Model.Books.Select(f => f.Id).ToList();
            IOrderedEnumerable<CBook> addedBooks = _allBooks.Where(f => shelfsBookIds.Contains(f.Id)).OrderBy(f => f.Title);
            IOrderedEnumerable<CBook> availableBooks = _allBooks.Except(addedBooks).OrderBy(f => f.Title);

            AddedBooks.Clear();
            AvailableBooks.Clear();
            foreach (CBook addedBook in addedBooks)
            {
                AddedBooks.Add(addedBook);
            }
            foreach (CBook availableBook in availableBooks)
            {
                AvailableBooks.Add(availableBook);
            }
        }

        protected override void OnDeleteExecute(Object obj)
        {
            MessageDialogResult result = _messageDialogService.ShowYesNoDialog(
                "Delete shelf",
                $"Do you really want to delete the shelf '{Shelf.Name}'");
            if(result == MessageDialogResult.Yes)
            {
                _shelfRepository.Delete(Shelf.Model);
                _shelfRepository.Save();
                RaiseDetailDeleteEvent(Shelf.Id);
            }
        }

        protected override Boolean OnSaveCanExecute(Object obj)
        {
            return Shelf != null && !Shelf.HasErrors && HasChanges;
        }

        protected override void OnSaveExecute(Object obj)
        {
            _shelfRepository.Save();
            HasChanges = _shelfRepository.HasChanges();
            RaiseDetailSavedEvent(Shelf.Id, Shelf.Name);
        }

        private void InitializeShelf(CShelf shelf)
        {
            Shelf = new CShelfWrapper(shelf);
            Shelf.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                {
                    HasChanges = _shelfRepository.HasChanges();
                }

                if (e.PropertyName == nameof(Shelf.HasErrors))
                {
                    ((CDelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            };
            ((CDelegateCommand)SaveCommand).RaiseCanExecuteChanged();

            // Little trick to trigger a validation
            Shelf.Name = Shelf.Name + String.Empty;
        }

        private CShelf CreateNewShelf()
        {
            CShelf shelf = new CShelf(String.Empty);
            _shelfRepository.Add(shelf);
            return shelf;
        }
    }
}
