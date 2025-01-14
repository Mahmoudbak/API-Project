﻿using System.ComponentModel.DataAnnotations;

namespace ProjectApi.Model
{
    public class CustomerDTO
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Name { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
    }

    public class CustomerUpdateDTO
    {
        [Required, EmailAddress]

        public string Email { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }
    }
}

