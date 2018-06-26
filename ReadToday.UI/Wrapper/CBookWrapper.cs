using System;
using System.Collections.Generic;
using ReadToday.Model;

namespace ReadToday.UI.Wrapper
{
    public class CBookWrapper : CModelWrapper<CBook>
    {
        public CBookWrapper(CBook model) : base(model)
        {
        }

        public Guid Id => GetValue<Guid>();

        public String Title
        {
            get => GetValue<String>();
            set => SetValue(value);
        }

        public String Description
        {
            get => GetValue<String>();
            set => SetValue(value);
        }

        public String Author
        {
            get => GetValue<String>();
            set => SetValue(value);
        }

        public Guid? LiteraryGenreId
        {
            get => GetValue<Guid?>();
            set => SetValue(value);
        }

        protected override IEnumerable<String> ValidateProperty(String propertyName)
        {
            switch(propertyName)
            {
                case nameof(Title):
                    if(String.Equals(Title, "Title", StringComparison.OrdinalIgnoreCase))
                    {
                        yield return "Title is not a valid title here";
                    }
                    break;
                case nameof(Description):
                    if(String.Equals(Description, "Description", StringComparison.OrdinalIgnoreCase))
                    {
                        yield return "Description is not a valid description here";
                    }
                    break;
                case nameof(Author):
                    if(String.Equals(Author, "Author", StringComparison.OrdinalIgnoreCase))
                    {
                        yield return "Author is not a valid author here";
                    }
                    break;
            }
        }
    }
}
