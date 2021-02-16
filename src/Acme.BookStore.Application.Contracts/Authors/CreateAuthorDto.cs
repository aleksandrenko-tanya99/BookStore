using System;
using System.ComponentModel.DataAnnotations;

namespace Acme.BookStore.Authors
{
    public class CreateAuthorDto
    {
        [Required]
        [StringLength(AuthorConsts.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        public string ShortBio { get; set; }
        [Required]
        public string Image { get; set; } = "http://via.placeholder.com/300x300";
    }
}