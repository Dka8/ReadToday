using System;
using ReadToday.Model;

namespace ReadToday.UI.Wrapper
{
    public class CCharacterWrapper : CModelWrapper<CCharacter>
    {
        public CCharacterWrapper(CCharacter model) : base(model) { }

        public String Name
        {
            get => GetValue<String>();
            set => SetValue(value);
        }
    }
}
