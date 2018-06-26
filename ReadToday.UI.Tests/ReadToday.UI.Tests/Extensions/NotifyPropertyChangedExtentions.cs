using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadToday.UI.Tests.Extensions
{
    public static class NotifyPropertyChangedExtentions
    {
        public static bool IsPropertyChangedFired(
            this INotifyPropertyChanged notifyPropertyChanged,
            Action action, string propertyName)
        {
            var fired = false;
            notifyPropertyChanged.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == propertyName)
                {
                    fired = true;
                }
            };

            action();

            return fired;
        }
    }
}
