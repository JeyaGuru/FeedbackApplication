﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CommentApplication.ViewModel
{
    public class SignUpViewModel
    {
        //Required attribute implements validation on Model item that this fields is mandatory for user
        [Required]
        //We are also implementing Regular expression to check if email is valid like a1@test.com
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail id is not valid")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Secret")]
        public string SecretCode { get; set; }

    }
}