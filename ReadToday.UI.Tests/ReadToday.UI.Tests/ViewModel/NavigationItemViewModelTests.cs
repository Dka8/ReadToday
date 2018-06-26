using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Prism.Events;
using ReadToday.UI.Events;
using ReadToday.UI.Tests.Extensions;
using ReadToday.UI.ViewModel;
using Xunit;

namespace ReadToday.UI.Tests.ViewModel
{
    public class NavigationItemViewModelTests
    {
        private Guid _bookId;
        private Mock<IEventAggregator> _eventAggregatorMock;
        private NavigationItemViewModel _viewModel;

        public NavigationItemViewModelTests()
        {
            _bookId = Guid.NewGuid();
            _eventAggregatorMock = new Mock<IEventAggregator>();
            _viewModel = new NavigationItemViewModel(_bookId,
                "SomeTitle", _eventAggregatorMock.Object);
        }

        [Fact]
        public void ShouldPublishOpenBookEditViewEvent()
        {
            var eventMock = new Mock<COpenBookEditViewEvent>();
            _eventAggregatorMock
                .Setup(ea => ea.GetEvent<COpenBookEditViewEvent>())
                .Returns(eventMock.Object);

            _viewModel.OpenBookEditViewCommand.Execute(null);

            eventMock.Verify(e => e.Publish(_bookId), Times.Once);
        }

        [Fact]
        public void ShouldRaisePropertyChangedEventForTitle()
        {
            var fired = _viewModel.IsPropertyChangedFired(
                () => { _viewModel.Title = "CHanged"; }, nameof(_viewModel.Title));

            Assert.True(fired);
        }
    }
}
