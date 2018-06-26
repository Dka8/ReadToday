using System;
using System.Windows.Input;
using Prism.Events;
using ReadToday.UI.Command;
using ReadToday.UI.Events;

namespace ReadToday.UI.ViewModel
{
    public abstract class CEditViewModelBase : CViewModelBase, IEditViewModel
    {
        private Boolean _hasChanges;
        protected IEventAggregator EventAggregator;

        public CEditViewModelBase(IEventAggregator eventAggregator)
        {
            EventAggregator = eventAggregator;
            SaveCommand = new CDelegateCommand(OnSaveExecute, OnSaveCanExecute);
            DeleteCommand = new CDelegateCommand(OnDeleteExecute);
        }

        public abstract void Load(Guid? id);

        public ICommand SaveCommand { get; private set; }

        public ICommand DeleteCommand { get; private set; }

        public Boolean HasChanges
        {
            get => _hasChanges;
            set
            {
                if (_hasChanges != value)
                {
                    _hasChanges = value;
                    OnPropertyChanged();
                    ((CDelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public abstract Guid Id { get; }

        protected abstract void OnDeleteExecute(Object obj);

        protected abstract Boolean OnSaveCanExecute(Object obj);

        protected abstract void OnSaveExecute(Object obj);

        protected virtual void RaiseDetailDeleteEvent(Guid modelId)
        {
            EventAggregator.GetEvent<CDetailDeletedEvent>().Publish(
                new DelailDeletedEventArgs(modelId, this.GetType().Name));
        }

        protected virtual void RaiseDetailSavedEvent(Guid modelId, String displayMember)
        {
            EventAggregator.GetEvent<CDetailSavedEvent>().Publish(
                new DetailSavedEventArgs(modelId, displayMember, this.GetType().Name));
        }
    }
}
