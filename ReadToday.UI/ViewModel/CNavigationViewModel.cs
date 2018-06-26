using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Prism.Events;
using ReadToday.Contracts;
using ReadToday.Model;
using ReadToday.UI.DataProvider;
using ReadToday.UI.Events;

namespace ReadToday.UI.ViewModel
{
    public interface INavigationViewModel
    {
        void Load();
    }

    public class CNavigationViewModel : CViewModelBase, INavigationViewModel
    {
        private ILookupService _lookupService;
        private IEventAggregator _eventAggregator;

        public CNavigationViewModel(
            ILookupService lookupService,
            IEventAggregator eventAggregator)
        {
            _lookupService = lookupService;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<CDetailSavedEvent>().Subscribe(OnDetailSaved);
            _eventAggregator.GetEvent<CDetailDeletedEvent>().Subscribe(OnDetailDeleted);

            Books = new ObservableCollection<CNavigationItemViewModel>();
            Shelfs = new ObservableCollection<CNavigationItemViewModel>();
        }

        public ObservableCollection<CNavigationItemViewModel> Books { get; set; }

        public ObservableCollection<CNavigationItemViewModel> Shelfs { get; set; }

        public void Load()
        {
            LoadBookLookup();
            LoadShelfLookup();
        }

        private void LoadBookLookup()
        {
            Books.Clear();
            var lookup = _lookupService.GetBookLookups();
            foreach (var item in lookup)
            {
                Books.Add(new CNavigationItemViewModel(
                    item.Id, item.DisplayMember,
                    nameof(CBookEditViewModel), _eventAggregator));
            }
        }

        private void LoadShelfLookup()
        {
            Shelfs.Clear();
            var lookup = _lookupService.GetShelfLookups();
            foreach (var item in lookup)
            {
                Shelfs.Add(new CNavigationItemViewModel(
                    item.Id, item.DisplayMember,
                    nameof(CShelfEditViewModel), _eventAggregator));
            }
        }

        private void OnDetailDeleted(DelailDeletedEventArgs args)
        {
            switch (args.ViewModelName)
            {
                case nameof(CBookEditViewModel):
                    OnDetailDeleted(Books, args);
                    break;
                case nameof(CShelfEditViewModel):
                    OnDetailDeleted(Shelfs, args);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private void OnDetailDeleted(ObservableCollection<CNavigationItemViewModel> items, DelailDeletedEventArgs args)
        {
            CNavigationItemViewModel navigationItem = items.Single(b => b.Id == args.Id);
            items.Remove(navigationItem);
        }

        private void OnDetailSaved(DetailSavedEventArgs args)
        {
            switch (args.ViewModelName)
            {
                case nameof(CBookEditViewModel):
                    OnDetailSaved(Books, args);
                    break;
                case nameof(CShelfEditViewModel):
                    OnDetailSaved(Shelfs, args);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private void OnDetailSaved(ObservableCollection<CNavigationItemViewModel> items, DetailSavedEventArgs args)
        {
            CNavigationItemViewModel navigationItem = items.SingleOrDefault(i => i.Id == args.Id);
            if (navigationItem != null)
            {
                navigationItem.DisplayMember = args.DisplayMember;
            }
            else
            {
                navigationItem = new CNavigationItemViewModel(args.Id, args.DisplayMember,
                    args.ViewModelName, _eventAggregator);
                items.Add(navigationItem);
            }
        }
    }
}
