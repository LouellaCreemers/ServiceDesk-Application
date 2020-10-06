using Marvin.JsonPatch;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Security.Principal;
using WebAppClient.Helpers;
using WebAppClient.Models;
using WebAppClient.ViewModels;
using static System.Net.WebRequestMethods;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.CompilerServices;

namespace WebAppClient.Controllers
{
    public class LoginController : Controller
    {
        public string errorMessage = "";
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(UserVM user)
        {
            if (ModelState.IsValid)
            {
                HttpClient client = MVCClientHttpClient.GetClient();
                HttpResponseMessage userResponse = await client.GetAsync("api/user/");

                UsersVM AllUsersVM = new UsersVM();
                TicketsVM ticketsVM = new TicketsVM();


                if (userResponse.IsSuccessStatusCode)
                {
                    string Content = await userResponse.Content.ReadAsStringAsync();
                    AllUsersVM.lstUser = JsonConvert.DeserializeObject<IEnumerable<User>>(Content);
                }
                else
                {
                    return Content("An error occurred.");
                }

                try
                {
                    //var userlog = new GenericPrincipal(new GenericIdentity(user.EmailAdress), new string[] { AllUsersVM.lstUser.Where(u => u.EmailAdress == user.EmailAdress && u.Password == user.Password).ToList().ToString() });
                    var loggedInUser = AllUsersVM.lstUser.Single(x => (x.Password == user.Password) && (x.EmailAdress == user.EmailAdress));
                    if (!loggedInUser.Equals(null))
                    {

                        TempData["Type"] = loggedInUser.Type.ToString();

                        return RedirectToAction("Index", "Dashboard",  ticketsVM.Login);
                    }
                    else
                    {
                        return View("Index");
                    }
                }
                catch
                {
                    return View(new UserVM());
                }

            }

            else 
            {
                return View(new UserVM());
            }
        }
    }
}
