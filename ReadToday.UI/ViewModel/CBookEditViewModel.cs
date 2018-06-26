using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Prism.Events;
using ReadToday.Contracts;
using ReadToday.Model;
using ReadToday.UI.Command;
using ReadToday.UI.DataProvider;
using ReadToday.UI.Dialogs;
using ReadToday.UI.Wrapper;

namespace ReadToday.UI.ViewModel
{
    public interface IBookEditViewModel : IEditViewModel
    {
    }

    public class NullLookupItem : LookupItem
    {
        public NullLookupItem() { }
        public NullLookupItem(String displayMember)
        {
            DisplayMember = displayMember;
        }
        public new Guid? Id { get => null; }
    }
        
    public class CBookEditViewModel : CEditViewModelBase, IBookEditViewModel
    {
        private IBookRepository _bookRepository;
        private IMessageDialogService _messageDialogService;
        private ILookupService _lookupService;
        private CBookWrapper _book;
        private CCharacterWrapper _selectedCharacter;

        public CBookEditViewModel(IBookRepository bookRepository,
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService,
            ILookupService lookupService)
            : base(eventAggregator)
        {
            _bookRepository = bookRepository;
            _messageDialogService = messageDialogService;
            _lookupService = lookupService;

            AddCharacterCommand = new CDelegateCommand(OnAddCharacterExecute);
            RemoveCharacterCommand = new CDelegateCommand(OnRemoveCharacterExecute, OnRemoveCharacterCanExecute);

            LiteraryGenres = new ObservableCollection<LookupItem>();
            Characters = new ObservableCollection<CCharacterWrapper>();
        }

        private Boolean OnRemoveCharacterCanExecute(Object arg)
        {
            return SelectedCharacter != null;
        }

        private void OnRemoveCharacterExecute(Object obj)
        {
            SelectedCharacter.PropertyChanged -= CharacterWrapper_PropertyChanged;
            _bookRepository.RemoveCharacter(SelectedCharacter.Model);

            Characters.Remove(SelectedCharacter);
            SelectedCharacter = null;
            HasChanges = _bookRepository.HasChanges();
            ((CDelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private void OnAddCharacterExecute(Object obj)
        {
            CCharacterWrapper newCharacter = new CCharacterWrapper(new CCharacter());
            newCharacter.PropertyChanged += CharacterWrapper_PropertyChanged;
            Characters.Add(newCharacter);
            Book.Model.Characters.Add(newCharacter.Model);
            newCharacter.Name = String.Empty; // Trigger validation
        }

        public override void Load(Guid? bookId)
        {
            CBook book = bookId.HasValue
                ? _bookRepository.GetById(bookId.Value)
                : CreateNewBook();

            InitializeBookCharacters(book.Characters);
            InitializeBook(book);
            LoadLiteraryGenres();
        }

        private void InitializeBookCharacters(ICollection<CCharacter> characters)
        {
            foreach(CCharacterWrapper wrapper in Characters)
            {
                wrapper.PropertyChanged -= CharacterWrapper_PropertyChanged;
            }
            Characters.Clear();

            foreach(CCharacter character in characters)
            {
                CCharacterWrapper wrapper = new CCharacterWrapper(character);
                Characters.Add(wrapper);
                wrapper.PropertyChanged += CharacterWrapper_PropertyChanged;
            }
        }

        private void CharacterWrapper_PropertyChanged(Object sender, PropertyChangedEventArgs e)
        {
            if (!HasChanges)
            {
                HasChanges = _bookRepository.HasChanges();
            }
            if(e.PropertyName == nameof(CCharacterWrapper.HasErrors))
            {
                ((CDelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        private void InitializeBook(CBook book)
        {
            Book = new CBookWrapper(book);
            Book.PropertyChanged += Book_PropertyChanged;

            InvalidateCommands();

            // Little trick to trigger a validation
            Book.Title = Book.Title + String.Empty;
            Book.Author = Book.Author + String.Empty;
        }

        private void LoadLiteraryGenres()
        {
            LiteraryGenres.Clear();
            LiteraryGenres.Add(new NullLookupItem("-"));
            var lookup = _lookupService.GetLiteraryGenreLookups();
            foreach (var lookupItem in lookup)
            {
                LiteraryGenres.Add(lookupItem);
            }
        }

        public CBookWrapper Book
        {
            get => _book;
            private set
            {
                _book = value;
                OnPropertyChanged();
            }
        }
        public CCharacterWrapper SelectedCharacter
        {
            get => _selectedCharacter;
            set
            {
                _selectedCharacter = value;
                OnPropertyChanged();
                ((CDelegateCommand)RemoveCharacterCommand).RaiseCanExecuteChanged();
            }
        }

        public ICommand AddCharacterCommand { get; }
        public ICommand RemoveCharacterCommand { get; }
        public ObservableCollection<LookupItem> LiteraryGenres { get; }
        public ObservableCollection<CCharacterWrapper> Characters { get; }

        public override Guid Id => Book.Id;

        protected override void OnSaveExecute(Object obj)
        {
            _bookRepository.Save();
            HasChanges = _bookRepository.HasChanges();
            RaiseDetailSavedEvent(Book.Id, Book.Title);
        }

        protected override Boolean OnSaveCanExecute(Object arg)
        {
            return Book != null 
                && !Book.HasErrors 
                && HasChanges
                && Characters.All(ch => !ch.HasErrors);
        }

        protected override void OnDeleteExecute(Object obj)
        {
            MessageDialogResult result = _messageDialogService.ShowYesNoDialog("Delete book", 
                $"Do you really want to delete the book '{Book.Title}'");
            if (result == MessageDialogResult.Yes)
            {
                _bookRepository.Delete(Book.Model);
                _bookRepository.Save();
                RaiseDetailDeleteEvent(Book.Id);
            }
        }

        private Boolean OnDeleteCanExecute(Object arg)
        {
            return Book != null && Book.Id != Guid.Empty;
        }

        private void Book_PropertyChanged(Object sender, PropertyChangedEventArgs e)
        {
            if (!HasChanges)
            {
                HasChanges = _bookRepository.HasChanges();
            }
            if(e.PropertyName == nameof(Book.HasErrors))
            {
                InvalidateCommands();
            }
        }

        private void InvalidateCommands()
        {
            ((CDelegateCommand) SaveCommand).RaiseCanExecuteChanged();
            ((CDelegateCommand) DeleteCommand).RaiseCanExecuteChanged();
        }

        private CBook CreateNewBook()
        {
            CBook book = new CBook();
            _bookRepository.Add(book);
            return book;
        }

    }
}
