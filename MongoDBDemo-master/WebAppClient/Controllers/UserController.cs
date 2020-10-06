using Marvin.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebAppClient.Helpers;
using WebAppClient.Models;
using WebAppClient.ViewModels;

namespace WebAppClient.Controllers
{
    public class UserController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            HttpClient client = MVCClientHttpClient.GetClient();
            HttpResponseMessage userResponse = await client.GetAsync("api/user/");

            UsersVM allUsersVM = new UsersVM();

            var type = TempData.Peek("Type");
            allUsersVM.Login = type.ToString();

            if (userResponse.IsSuccessStatusCode)
            {
                string Content = await userResponse.Content.ReadAsStringAsync();
                allUsersVM.lstUser = JsonConvert.DeserializeObject<IEnumerable<User>>(Content);
            }
            else
            {
                return Content("An error occurred.");
            }

            return View(allUsersVM);
        }

        [HttpPost]
        public async Task<IActionResult> Index(UsersVM model)
        {
            if (ModelState.IsValid && !string.IsNullOrEmpty(model.TextSearch))
            {
                HttpClient client = MVCClientHttpClient.GetClient();
                HttpResponseMessage userResponse = await client.GetAsync("api/user/");


                if (ModelState.IsValid && !string.IsNullOrEmpty(model.TextSearch))
                {
                    HttpResponseMessage userSearchResponse = await client.GetAsync("api/user/getbysearch/" + model.TextSearch);
                    string Content = await userSearchResponse.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<IEnumerable<User>>(Content);
                    model.lstUser = result;
                    return View(model);
                }

                else
                {
                    return Content("An error occurred.");
                }
                return View(model);
            }
            else
            {
                return View(new UsersVM());
            }
        }


        public async Task<IActionResult> CountThem()
        {
            long noOfUsers;
            HttpClient client = MVCClientHttpClient.GetClient();
            HttpResponseMessage userResponse = await client.GetAsync("/api/user/count/");

            if (userResponse.IsSuccessStatusCode)
            {
                string Content = await userResponse.Content.ReadAsStringAsync();
                noOfUsers = JsonConvert.DeserializeObject<long>(Content);

            }
            else
            {
                return Content("An error occurred.");
            }
            return Content(noOfUsers.ToString());
        }

        public async Task<IActionResult> Get(string id)
        {
            HttpClient client = MVCClientHttpClient.GetClient();

            HttpResponseMessage userResponse = await client.GetAsync("/api/user/getuserbyid/" + id);

            if (userResponse.IsSuccessStatusCode)
            {
                string Content = await userResponse.Content.ReadAsStringAsync();
                var foundUser = JsonConvert.DeserializeObject<User>(Content);
            }
            else
            {
                return Content("User not found.");
            }
            return null;
        }


        [HttpGet]
        public IActionResult Create()
        {
            UserVM userVM = new UserVM();
            return View(userVM);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserVM userVM)
        {
            HttpClient client = MVCClientHttpClient.GetClient();
            var SerializedItemToCreate = JsonConvert.SerializeObject(userVM);

            HttpResponseMessage userResponse = await client.PostAsync("/api/user/create/",
                                                new StringContent(SerializedItemToCreate,
                                                System.Text.Encoding.Unicode,
                                                "application/json"));

            if (userResponse.IsSuccessStatusCode)
            {
                return this.RedirectToAction("Index");
            }
            return this.RedirectToAction("Error");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string Id)
        {
            HttpClient client = MVCClientHttpClient.GetClient();
            HttpResponseMessage userResponse = await client.GetAsync("/api/user/getuserbyid/" + Id);

            if (userResponse.IsSuccessStatusCode)
            {

                UserVM userVM = new UserVM();

                string Content = await userResponse.Content.ReadAsStringAsync();
                User foundUser = JsonConvert.DeserializeObject<User>(Content);
                userVM.Id = foundUser.Id;
                userVM.FirstName = foundUser.FirstName;
                userVM.LastName = foundUser.LastName;
                userVM.Type = foundUser.Type;
                userVM.Location = foundUser.Location;
                userVM.EmailAdress = foundUser.EmailAdress;
                userVM.PhoneNumber = foundUser.Phonenumber;
                userVM.Password = foundUser.Password;

                return View(userVM);

            }
            return Content("An error occurred.");

        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, UserVM userVM)
        {
            JsonPatchDocument<User> patchDoc = new JsonPatchDocument<User>();
            patchDoc.Replace(e => e.FirstName, userVM.FirstName);
            patchDoc.Replace(e => e.LastName, userVM.LastName);
            patchDoc.Replace(e => e.Type, userVM.Type);
            patchDoc.Replace(e => e.Location, userVM.Location);
            patchDoc.Replace(e => e.EmailAdress, userVM.EmailAdress);
            patchDoc.Replace(e => e.Phonenumber, userVM.PhoneNumber);
            patchDoc.Replace(e => e.Password, userVM.Password);

            //serialize patch
            var serializedPatch = JsonConvert.SerializeObject(patchDoc);

            HttpClient client = MVCClientHttpClient.GetClient();

            HttpResponseMessage userResponse = await client.PatchAsync("/api/user/update/"+ id, 
                                                new StringContent(serializedPatch, System.Text.Encoding.Unicode,
                                                "application/json"));


            if (userResponse.IsSuccessStatusCode)
            {
                return this.RedirectToAction("Index");
            }

            return Content("An error occurred.");
        }

        public async Task<IActionResult> Delete(string Id)
        {
            HttpClient client = MVCClientHttpClient.GetClient();
            HttpResponseMessage userResponse = await client.DeleteAsync("/api/user/deleteuser/" + Id);

            if (userResponse.IsSuccessStatusCode)
            {
                return this.RedirectToAction("Index");
            }
            return Content("An error occurred.");
        }
        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
