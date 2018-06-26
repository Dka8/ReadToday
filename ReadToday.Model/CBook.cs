using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace ReadToday.Model
{
    public class CBook
    {
        public CBook()
        {
            Characters = new Collection<CCharacter>();
            Shelfs = new Collection<CShelf>();
        }

        public CBook(String title, String description, String author)
        {
            Id = Guid.NewGuid();
            Title = title;
            Description = description;
            Author = author;
            Characters = new Collection<CCharacter>();
            Shelfs = new Collection<CShelf>();
        }

        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public String Title { get; set; }

        public String Description { get; set; }

        [Required]
        [StringLength(50)]
        public String Author { get; set; }

        public Guid? LiteraryGenreId { get; set; }
        public CLiteraryGenre LiteraryGenre { get; set; }

        public ICollection<CCharacter> Characters { get; set; }

        public ICollection<CShelf> Shelfs { get; set; }
    }
}
