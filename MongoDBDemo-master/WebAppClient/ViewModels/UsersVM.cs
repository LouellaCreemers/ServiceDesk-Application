using Models;
using System.Collections.Generic;

namespace WebAppClient.ViewModels
{
    public class UsersVM
    {
        public IEnumerable<User> lstUser { get; set; }
        public string TextSearch { get; set; }
    }
}
