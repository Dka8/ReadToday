using System;
using ReadToday.Model;

namespace ReadToday.UI.Wrapper
{
    public class CShelfWrapper : CModelWrapper<CShelf>
    {
        public CShelfWrapper(CShelf model) : base(model)
        {
        }

        public Guid Id => Model.Id;

        public String Name
        {
            get => GetValue<String>();
            set => SetValue(value);
        }
    }
}
