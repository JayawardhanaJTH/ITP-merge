using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Staff_Management1.Models
{
    public class User
    {
        public int UserID { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "This field is required")]
        public string Password { get; set; }


        public string LoginErrorMessage { get; set; }
    }
}