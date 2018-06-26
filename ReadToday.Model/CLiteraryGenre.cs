using System;
using System.ComponentModel.DataAnnotations;

namespace ReadToday.Model
{
    public class CLiteraryGenre
    {
        public CLiteraryGenre() { }
        public CLiteraryGenre(String name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }

        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public String Name { get; set; }
    }
}
