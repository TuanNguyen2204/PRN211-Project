using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace CinemaProject.Models
{
    public partial class Person
    {
        public Person()
        {
            Rates = new HashSet<Rate>();
        }

        public int PersonId { get; set; }
        public string Fullname { get; set; }
        public string Gender { get; set; }

        [Required(ErrorMessage = "Email không để trống.")]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage ="Email không hợp lệ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mật khẩu không để trống.")]
        public string Password { get; set; }
        public int? Type { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<Rate> Rates { get; set; }
    }
}
