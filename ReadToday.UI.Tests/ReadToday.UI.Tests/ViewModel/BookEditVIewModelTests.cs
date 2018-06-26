using System;
using Moq;
using Prism.Events;
using ReadToday.Model;
using ReadToday.UI.DataProvider;
using ReadToday.UI.Dialogs;
using ReadToday.UI.Events;
using ReadToday.UI.Tests.Extensions;
using ReadToday.UI.ViewModel;
using Xunit;

namespace ReadToday.UI.Tests.ViewModel
{
    public class BookEditVIewModelTests
    {
        private Guid _bookId = Guid.NewGuid();
        private Mock<IBookDataProvider> _dataPoviderMock;
        private BookEditViewModel _viewModel;
        private Mock<CBookSavedEvent> _bookSavedEventMock;
        private Mock<CBookDeletedEvent> _bookDeletedEventMock;
        private Mock<IEventAggregator> _eventAggregatorMock;
        private Mock<IMessageDialogService> _messageDialogServiceMock;

        public BookEditVIewModelTests()
        {
            _dataPoviderMock = new Mock<IBookDataProvider>();
            _dataPoviderMock.Setup(dp => dp.GetBookById(_bookId))
                .Returns(new CBook(_bookId, "SomeTitle"));
            _bookSavedEventMock = new Mock<CBookSavedEvent>();
            _bookDeletedEventMock = new Mock<CBookDeletedEvent>();
            _eventAggregatorMock = new Mock<IEventAggregator>();
            _eventAggregatorMock.Setup(ea => ea.GetEvent<CBookSavedEvent>())
                .Returns(_bookSavedEventMock.Object);
            _eventAggregatorMock.Setup(ea => ea.GetEvent<CBookDeletedEvent>())
                .Returns(_bookDeletedEventMock.Object);

            _messageDialogServiceMock = new Mock<IMessageDialogService>();

            _viewModel = new BookEditViewModel(_dataPoviderMock.Object,
                _eventAggregatorMock.Object,
                _messageDialogServiceMock.Object);
        }

        [Fact]
        public void ShouldLoadFriend()
        {
            _viewModel.Load(_bookId);

            Assert.NotNull(_viewModel.Book);
            Assert.Equal(_bookId, _viewModel.Book.Id);

            _dataPoviderMock.Verify(dp=>dp.GetBookById(_bookId), Times.Once);
        }

        [Fact]
        public void ShouldRaisePropertyChangedEventForBook()
        {
            var fired = _viewModel.IsPropertyChangedFired(
                () => _viewModel.Load(_bookId),
                nameof(_viewModel.Book));

            Assert.True(fired);
        }

        [Fact]
        public void ShouldDisableSaveCommandWhenBookIsLoaded()
        {
            _viewModel.Load(_bookId);

            Assert.False(_viewModel.SaveCommand.CanExecute(null));
        }

        [Fact]
        public void ShouldDisableSaveCommandWhenBookIsChanged()
        {
            _viewModel.Load(_bookId);

            _viewModel.Book.Title = "Changed";

            Assert.True(_viewModel.SaveCommand.CanExecute(null));
        }

        [Fact]
        public void ShouldDisableSaveCommandWithoutLoad()
        {
            Assert.False(_viewModel.SaveCommand.CanExecute(null));
        }

        [Fact]
        public void ShouldRaiseCanExecuteChangedForSaveCommandWhenBookIsChanged()
        {
            _viewModel.Load(_bookId);
            var fired = false;
            _viewModel.SaveCommand.CanExecuteChanged += (s, e) => fired = true;
            _viewModel.Book.Title = "Changed";
            Assert.True(fired);
        }

        [Fact]
        public void ShouldRaiseCanExecuteChangedForSaveCommandAfterLoad()
        {
            var fired = false;
            _viewModel.SaveCommand.CanExecuteChanged += (s, e) => fired = true;
            _viewModel.Load(_bookId);
            Assert.True(fired);
        }

        [Fact]
        public void ShouldCallSaveMethodOfDataProviderWhenSaveCommandExecuted()
        {
            _viewModel.Load(_bookId);
            _viewModel.Book.Title = "Changed";

            _viewModel.SaveCommand.Execute(null);
            _dataPoviderMock.Verify(dp=>dp.SaveBook(_viewModel.Book.Model), Times.Once);
        }

        [Fact]
        public void ShouldAcceptChangesWhenSaveCommandIsExecuted()
        {
            _viewModel.Load(_bookId);
            _viewModel.Book.Title = "Changed";

            _viewModel.SaveCommand.Execute(null);
            Assert.False(_viewModel.Book.IsChanged);
        }

        [Fact]
        public void ShouldPublishBookSavedEventWhenSaveCommandIsExecuted()
        {
            _viewModel.Load(_bookId);
            _viewModel.Book.Title = "Changed";

            _viewModel.SaveCommand.Execute(null);
            _bookSavedEventMock.Verify(e => e.Publish(_viewModel.Book.Model), Times.Once());
        }

        [Fact]
        public void ShouldCreateNewBookWhenNullIsPassedToLoadMethod()
        {
            _viewModel.Load(null);

            Assert.NotNull(_viewModel.Book);
            Assert.Equal(Guid.Empty, _viewModel.Book.Id);
            Assert.Null(_viewModel.Book.Title);
            Assert.Null(_viewModel.Book.Description);
            Assert.Null(_viewModel.Book.Author);

            _dataPoviderMock.Verify(dp=>dp.GetBookById(It.IsAny<Guid>()), Times.Never);
        }

        [Fact]
        public void ShouldEnableDeleteCommandForExistingBook()
        {
            _viewModel.Load(_bookId);
            Assert.True(_viewModel.DeleteCommand.CanExecute(null));
        }

        [Fact]
        public void ShouldDisableDeleteCommandForNewBook()
        {
            _viewModel.Load(null);
            Assert.False(_viewModel.DeleteCommand.CanExecute(null));
        }

        [Fact]
        public void ShouldDisableDeleteCommandWithoutLoad()
        {
            Assert.False(_viewModel.DeleteCommand.CanExecute(null));
        }

        [Fact]
        public void ShouldRaiseCanExecuteChangedForDeleteCommandWhenAcceptingChanges()
        {
            _viewModel.Load(_bookId);
            var fired = false;
            _viewModel.Book.Title = "Changed";
            _viewModel.DeleteCommand.CanExecuteChanged += (s, e) => fired = true;
            _viewModel.Book.AcceptChanges();
            Assert.True(fired);
        }

        [Fact]
        public void ShouldRaiseCanExecuteChangedForDeleteCommandAfterLoad()
        {
            var fired = false;
            _viewModel.DeleteCommand.CanExecuteChanged += (s, e) => fired = true;
            _viewModel.Load(_bookId);
            Assert.True(fired);
        }

        [Theory]
        [InlineData(MessageDialogResult.Yes, 1)]
        [InlineData(MessageDialogResult.No, 0)]
        public void ShouldCallDeleteBookWhenDeleteCommandIsExecuted(
            MessageDialogResult result, int expectedDeleteBookCalls)
        {
            _viewModel.Load(_bookId);

            _messageDialogServiceMock.Setup(ds => ds.ShowYesNoDialog(It.IsAny<string>(),
                It.IsAny<string>())).Returns(result);

            _viewModel.DeleteCommand.Execute(null);

            _dataPoviderMock.Verify(dp => dp.DeleteBook(_bookId),
                Times.Exactly(expectedDeleteBookCalls));
            _messageDialogServiceMock.Verify(ds => ds.ShowYesNoDialog(It.IsAny<string>(),
                It.IsAny<string>()), Times.Once);
        }

        [Theory]
        [InlineData(MessageDialogResult.Yes, 1)]
        [InlineData(MessageDialogResult.No, 0)]
        public void ShouldPublishBookDeletedEventWhenDeleteCommandIsExecuted(
            MessageDialogResult result, int expectedPublishCalls)
        {
            _viewModel.Load(_bookId);
            _messageDialogServiceMock.Setup(ds => ds.ShowYesNoDialog(It.IsAny<string>(),
                It.IsAny<string>())).Returns(result);

            _viewModel.DeleteCommand.Execute(null);

            _bookDeletedEventMock.Verify(e => e.Publish(_bookId), Times.Exactly(expectedPublishCalls));
            _messageDialogServiceMock.Verify(ds => ds.ShowYesNoDialog(It.IsAny<string>(),
                It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void ShouldDisplayCorrectMessageInDeleteDialog()
        {
            _viewModel.Load(_bookId);

            var b = _viewModel.Book;
            b.Title = "SomeTitle";

            _viewModel.DeleteCommand.Execute(null);

            _messageDialogServiceMock.Verify(d => d.ShowYesNoDialog("Delete book",
                $"Do you really want to delete the book '{b.Title}'"),
                Times.Once);
        }
    }
}
