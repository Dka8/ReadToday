using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace ReadToday.Model
{
    public class CShelf
    {
        public CShelf()
        {
            Books = new Collection<CBook>();
        }

        public CShelf(String name)
        {
            Id = Guid.NewGuid();
            Name = name;
            Books = new Collection<CBook>();
        }

        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public String Name { get; set; }

        public ICollection<CBook> Books { get; set; }
    }
}
