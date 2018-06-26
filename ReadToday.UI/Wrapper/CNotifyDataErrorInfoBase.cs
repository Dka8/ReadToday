using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using ReadToday.UI.ViewModel;

namespace ReadToday.UI.Wrapper
{
    public class CNotifyDataErrorInfoBase : CViewModelBase, INotifyDataErrorInfo
        {
            private Dictionary<String, List<String>> _errorsByProperyName
                = new Dictionary<String, List<String>>();

            public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

            public Boolean HasErrors => _errorsByProperyName.Any();

            public IEnumerable GetErrors(String propertyName)
            {
                return _errorsByProperyName.ContainsKey(propertyName)
                    ? _errorsByProperyName[propertyName]
                    : null;
            }

            protected virtual void OnErrorsChange(String propertyName)
            {
                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
                base.OnPropertyChanged(nameof(HasErrors));
            }

            protected void AddError(String propertyName, String error)
            {
                if (!_errorsByProperyName.ContainsKey(propertyName))
                {
                    _errorsByProperyName[propertyName] = new List<String>();
                }
                if (!_errorsByProperyName[propertyName].Contains(error))
                {
                    _errorsByProperyName[propertyName].Add(error);
                    OnErrorsChange(propertyName);
                }
            }

            protected void ClearErrors(String propertyName)
            {
                if (_errorsByProperyName.ContainsKey(propertyName))
                {
                    _errorsByProperyName.Remove(propertyName);
                    OnErrorsChange(propertyName);
                }
            }
        }
}
