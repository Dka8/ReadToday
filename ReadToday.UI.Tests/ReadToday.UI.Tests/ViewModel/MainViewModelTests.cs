using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Prism.Events;
using ReadToday.Model;
using ReadToday.UI.Events;
using ReadToday.UI.Tests.Extensions;
using ReadToday.UI.ViewModel;
using ReadToday.UI.Wrapper;

namespace ReadToday.UI.Tests.ViewModel
{
    public class MainViewModelTests
    {
        private ContentViewModel _viewModel;
        private Mock<INavigationViewModel> _navigationViewModelMock;
        private Mock<IEventAggregator> _eventAggregatorMock;
        private COpenBookEditViewEvent _openBookEditViewEvent;
        private CBookDeletedEvent _bookDeleteEvent;
        private List<Mock<IBookEditViewModel>> _bookEditViewModelMocks;

        public MainViewModelTests()
        {
            _bookEditViewModelMocks = new List<Mock<IBookEditViewModel>>();
            _navigationViewModelMock = new Mock<INavigationViewModel>();

            _openBookEditViewEvent = new COpenBookEditViewEvent();
            _bookDeleteEvent = new CBookDeletedEvent();
            _eventAggregatorMock = new Mock<IEventAggregator>();
            _eventAggregatorMock.Setup(ea => ea.GetEvent<COpenBookEditViewEvent>())
                .Returns(_openBookEditViewEvent);
            _eventAggregatorMock.Setup(ea => ea.GetEvent<CBookDeletedEvent>())
                .Returns(_bookDeleteEvent);

            _viewModel = new ContentViewModel(_navigationViewModelMock.Object,
                CreateBookEditViewModel, _eventAggregatorMock.Object);
        }

        private IBookEditViewModel CreateBookEditViewModel()
        {
            var bookEditViewModelMock = new Mock<IBookEditViewModel>();
            bookEditViewModelMock.Setup(vm => vm.Load(It.IsAny<Guid>()))
                .Callback<Guid?>(bookId =>
                {
                    bookEditViewModelMock.Setup(vm => vm.Book)
                        .Returns(new BookWrapper(new CBook(bookId.Value, "Title")));
                });

            _bookEditViewModelMocks.Add(bookEditViewModelMock);
            return bookEditViewModelMock.Object;
        }

        [Fact]
        public void ShouldCallTheLoadMethodOfTheNavigationViewModel()
        {
            _viewModel.Load();

            _navigationViewModelMock.Verify(vm => vm.Load(), Times.Once);
        }

        [Fact]
        public void ShouldAddBookEditViewModelAndLoadAndSelectIt()
        {
            var bookId = Guid.NewGuid();
            _openBookEditViewEvent.Publish(bookId);

            Assert.Equal(1, _viewModel.BookEditViewModels.Count);
            var bookEditVm = _viewModel.BookEditViewModels.First();
            Assert.Equal(bookEditVm, _viewModel.SelectedBookEditViewModel);
            _bookEditViewModelMocks.First().Verify(vm => vm.Load(bookId), Times.Once());
        }

        [Fact]
        public void ShouldAddBookEditViewModelAndLoadItWithIdNullAndSelectIt()
        {
            _viewModel.AddBookCommand.Execute(null);

            Assert.Equal(1, _viewModel.BookEditViewModels.Count);
            var bookEditVm = _viewModel.BookEditViewModels.First();
            Assert.Equal(bookEditVm, _viewModel.SelectedBookEditViewModel);
            _bookEditViewModelMocks.First().Verify(vm => vm.Load(null), Times.Once());
        }

        [Fact]
        public void ShouldAddBookEditViewModelsOnlyOnce()
        {
            var someBookId = Guid.NewGuid();
            var otherBookId = Guid.NewGuid();
            _openBookEditViewEvent.Publish(someBookId);
            _openBookEditViewEvent.Publish(otherBookId);
            _openBookEditViewEvent.Publish(someBookId);

            Assert.Equal(2, _viewModel.BookEditViewModels.Count);
        }

        [Fact]
        public void ShouldRaisePropertyChangedEventForSelectedEditViewModel()
        {
            var bookEditVmMock = new Mock<IBookEditViewModel>();
            var fired = _viewModel.IsPropertyChangedFired(
                () => { _viewModel.SelectedBookEditViewModel = bookEditVmMock.Object; },
                nameof(_viewModel.SelectedBookEditViewModel)
            );

            Assert.True(fired);
        }

        [Fact]
        public void ShouldRemoveBookEditViewModelOnCloseBookTabCommand()
        {
            _openBookEditViewEvent.Publish(Guid.NewGuid());
            var bookEditVm = _viewModel.SelectedBookEditViewModel;
            _viewModel.CloseBookTabCommand.Execute(bookEditVm);

            Assert.Equal(0, _viewModel.BookEditViewModels.Count);
        }

        [Fact]
        public void ShouldRemoveBookEditViewModelOnBookDeletedEvent()
        {
            var deletedBookId = Guid.NewGuid();
            _openBookEditViewEvent.Publish(deletedBookId);
            _openBookEditViewEvent.Publish(Guid.NewGuid());
            _openBookEditViewEvent.Publish(Guid.NewGuid());

            _bookDeleteEvent.Publish(deletedBookId);

            Assert.Equal(2, _viewModel.BookEditViewModels.Count);
            Assert.True(_viewModel.BookEditViewModels.All(vm=>vm.Book.Id != deletedBookId));
        }
    }
}
