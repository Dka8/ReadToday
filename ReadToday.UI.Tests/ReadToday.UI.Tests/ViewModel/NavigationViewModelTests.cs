using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Prism.Events;
using ReadToday.Model;
using ReadToday.UI.DataProvider;
using ReadToday.UI.Events;
using ReadToday.UI.ViewModel;
using Xunit;

namespace ReadToday.UI.Tests.ViewModel
{
    public class NavigationViewModelTests
    {
        private NavigationViewModel _viewModel;
        private CBookSavedEvent _bookSavedEvent;
        private CBookDeletedEvent _bookDeletedEvent;
        private readonly Guid _someGuid;
        private readonly Guid _otherGuid;

        public NavigationViewModelTests()
        {
            _someGuid = Guid.NewGuid();
            _otherGuid = Guid.NewGuid();

            _bookSavedEvent = new CBookSavedEvent();
            _bookDeletedEvent = new CBookDeletedEvent();

            var eventAggregatorMock = new Mock<IEventAggregator>();
            eventAggregatorMock.Setup(ea => ea.GetEvent<CBookSavedEvent>())
                .Returns(_bookSavedEvent);
            eventAggregatorMock.Setup(ea => ea.GetEvent<CBookDeletedEvent>())
                .Returns(_bookDeletedEvent);

            var navigationDataProviderMock = new Mock<INavigationDataProvider>();
            navigationDataProviderMock.Setup(dp => dp.GetAllBooks())
                .Returns(new List<CLookupBook>
                {
                    CLookupBook.Create(_someGuid, "SomeTitle"),
                    CLookupBook.Create(_otherGuid, "OtherTitle")
                });

            _viewModel = new NavigationViewModel(
                navigationDataProviderMock.Object, eventAggregatorMock.Object);
        }

        [Fact]
        public void ShouldLoadBooks()
        {
            _viewModel.Load();

            Assert.Equal(2, _viewModel.Books.Count);

            var book = _viewModel.Books.SingleOrDefault(b => b.Id == _someGuid);
            Assert.NotNull(book);
            Assert.Equal("SomeTitle", book.Title);

            book = _viewModel.Books.SingleOrDefault(b => b.Id == _otherGuid);
            Assert.NotNull(book);
            Assert.Equal("OtherTitle", book.Title);
        }

        [Fact]
        public void ShouldLoadBooksOnlyOnce()
        {
            _viewModel.Load();
            _viewModel.Load();

            Assert.Equal(2, _viewModel.Books.Count);
        }

        [Fact]
        public void ShouldUpdateNavigationItemWhenBookIsSaved()
        {
            _viewModel.Load();
            var navigationItem = _viewModel.Books.First();
            var bookId = navigationItem.Id;

            _bookSavedEvent.Publish(
                new CBook(bookId, "ChangedTitle"));

            Assert.Equal("ChangedTitle", navigationItem.Title);
        }

        [Fact]
        public void ShouldAddNavigationItemWhenAddedFriendIsSaved()
        {
            _viewModel.Load();
            var newBookId = Guid.NewGuid();

            _bookSavedEvent.Publish(new CBook(newBookId, "NewBook"));

            Assert.Equal(3, _viewModel.Books.Count);

            var addedBook = _viewModel.Books.SingleOrDefault(b => b.Id == newBookId);
            Assert.NotNull(addedBook);
            Assert.Equal("NewBook", addedBook.Title);
        }

        [Fact]
        public void ShouldRemoveNavigationItemWhenBookIsDeleted()
        {
            _viewModel.Load();

            var deletedBookId = _viewModel.Books.First().Id;

            _bookDeletedEvent.Publish(deletedBookId);

            var deletedBook = _viewModel.Books.SingleOrDefault(b => b.Id == deletedBookId);

            Assert.Null(deletedBook);
        }
    }
}
