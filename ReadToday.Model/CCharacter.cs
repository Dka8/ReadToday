using System;
using System.ComponentModel.DataAnnotations;

namespace ReadToday.Model
{
    public class CCharacter
    {
        public CCharacter() {
            Id = Guid.NewGuid();
        }
        public CCharacter(String name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }

        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public String Name { get; set; }

        public Guid BookId { get; set; }

        public CBook Book { get; set; }
    }
}
