using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace ReadToday.UI.Wrapper
{
    public class CModelWrapper<T> : CNotifyDataErrorInfoBase
    {
        public CModelWrapper(T model)
        {
            Model = model;
        }

        public T Model { get; }

        protected virtual TValue GetValue<TValue>([CallerMemberName] String propertyName=null)
        {
            return (TValue)typeof(T).GetProperty(propertyName).GetValue(Model);
        }

        protected virtual void SetValue<TValue>(TValue value,
            [CallerMemberName] String propertyName=null)
        {
            typeof(T).GetProperty(propertyName).SetValue(Model, value);
            OnPropertyChanged(propertyName);
            ValidatePropertyInternal(propertyName, value);
        }

        private void ValidatePropertyInternal(String propertyName, Object currentValue)
        {
            ClearErrors(propertyName);
            ValidateDataAnnotations(propertyName, currentValue);
            ValidateCustomError(propertyName);
        }

        private void ValidateCustomError(String propertyName)
        {
            IEnumerable<String> errors = ValidateProperty(propertyName);
            if (errors != null)
            {
                foreach (String error in errors)
                {
                    AddError(propertyName, error);
                }
            }
        }

        private void ValidateDataAnnotations(String propertyName, Object currentValue)
        {
            List<ValidationResult> results = new List<ValidationResult>();
            ValidationContext context = new ValidationContext(Model) { MemberName = propertyName };
            Validator.TryValidateProperty(currentValue, context, results);

            foreach (ValidationResult result in results)
            {
                AddError(propertyName, result.ErrorMessage);
            }
        }

        protected virtual IEnumerable<String> ValidateProperty(String propertyName)
        {
            return null;
        }
    }
}
