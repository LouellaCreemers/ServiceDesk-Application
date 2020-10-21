using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace WebAppClient.ViewModels
{
    public class UserVM
    {
        public string Id { get; set; }

        [DisplayName("First name: ")]
        public string FirstName { get; set; }

        [DisplayName("Last name: ")]
        public string LastName { get; set; }

        [DisplayName("Type of user: ")]
        public UserEnum Type { get; set; }

        [DisplayName("Email: ")]
        public string EmailAdress { get; set; }

        [DisplayName("Password: ")]
        public string Password { get; set; }

        [DisplayName("Phone number: ")]
        public string PhoneNumber { get; set; }

        [DisplayName("Location: ")]
        public string Location { get; set; }
    }
    
}

