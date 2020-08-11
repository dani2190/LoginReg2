using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
namespace LoginReg2.Models
{


    public class User
    {
        [Key]
            public int UserId { get; set; }

            [Required]
            public string FirstName {get;set;}

            [Required]
            public string LastName {get;set;}

            [EmailAddress]
            [Required]
            public string Email {get;set;}

            [DataType(DataType.Password)]
            [Required]
            [MinLength(8, ErrorMessage="Password must be 8 characters or longer!")]
            public string Password {get;set;}
            public string Confirm {get; set;}
            public DateTime CreatedAt {get;set;} = DateTime.Now;
            public DateTime UpdatedAt {get;set;} = DateTime.Now;
    }
}

    