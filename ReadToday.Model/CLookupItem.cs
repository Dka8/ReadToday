using System;

namespace ReadToday.Model
{
    public class CLookupItem
    {
        public static CLookupItem Create(Guid id, String title)
        {
            return new CLookupItem(id, title);
        }

        public CLookupItem()
        {

        }

        public CLookupItem(Guid id, String title)
        {
            Id = id;
            DisplayMember = title;
        }

        public Guid Id { get; set; }
        public String DisplayMember { get; set; }
    }

    public class NullLookupItem : CLookupItem
    {
        public NullLookupItem() { }
        public NullLookupItem(String displayMember)
        {
            DisplayMember = displayMember;
        }
        public new Guid? Id { get => null; }
    }
}
