using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Marvin.JsonPatch;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Models;
using Newtonsoft.Json;
using WebAppClient.Helpers;
using WebAppClient.Models;
using WebAppClient.ViewModels;

namespace WebAppClient.Controllers
{
    public class TicketController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            HttpClient client = MVCClientHttpClient.GetClient();
            HttpResponseMessage ticketResponse = await client.GetAsync("api/ticket/");

            TicketsVM allTicketsVM = new TicketsVM();

            var tempData = TempData.Peek("Type").ToString();
            allTicketsVM.Login = tempData;

            if (ticketResponse.IsSuccessStatusCode)
            {
                string Content = await ticketResponse.Content.ReadAsStringAsync();
                allTicketsVM.lstTickets = JsonConvert.DeserializeObject<IEnumerable<Ticket>>(Content);
            }
            else
            {
                return Content("An error occurred.");
            }

            return View(allTicketsVM);
        }

        [HttpPost]
        public async Task<IActionResult> Index(TicketsVM model, string submitSearch, string submitFilter, string submitArchive)
        {
            HttpClient client = MVCClientHttpClient.GetClient();
            var tempData = TempData.Peek("Type").ToString();
            model.Login = tempData;

            if (ModelState.IsValid && !string.IsNullOrEmpty(submitSearch))
            {
                HttpResponseMessage ticketResponse = await client.GetAsync("api/ticket/getallsorted/");
                string Content = await ticketResponse.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<IEnumerable<Ticket>>(Content);
                model.lstTickets = result;                
                return View(model);
            }

            else if (ModelState.IsValid && !string.IsNullOrEmpty(submitFilter))
            {
                HttpResponseMessage ticketResponse = await client.GetAsync("api/ticket/getbysearch/" + model.FilterKeyword);
                string Content = await ticketResponse.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<IEnumerable<Ticket>>(Content);
                model.lstTickets = result;
                return View(model);
            }

            else if (ModelState.IsValid && !string.IsNullOrEmpty(model.TextSearch))
            {
                HttpResponseMessage ticketSearchResponse = await client.GetAsync("api/ticket/getbysearch/" + model.TextSearch);
                string ContentSearch = await ticketSearchResponse.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<IEnumerable<Ticket>>(ContentSearch);
                model.lstTickets = result;
                return View(model);
            }

            else if (ModelState.IsValid && !string.IsNullOrEmpty(submitArchive))
            {
                HttpResponseMessage userResponse = await client.GetAsync("/api/ticket/getold/");
                string Content = await userResponse.Content.ReadAsStringAsync();

                if (Content.Count() == 0)
                {
                    return this.RedirectToAction("Index");
                }
                else
                { 
                    var result = JsonConvert.DeserializeObject<IEnumerable<Ticket>>(Content);

                    foreach (var item in result) 
                    {
                       await Delete(item.Id);
                    }

                    return this.RedirectToAction("Index");
                }
            }

            else 
            {
                return View(new TicketVM());
            }
        }


        public async Task<IActionResult> CountThem()
        {
            long noOfTickets;
            HttpClient client = MVCClientHttpClient.GetClient();
            HttpResponseMessage ticketResponse = await client.GetAsync("/api/ticket/count/");

            if (ticketResponse.IsSuccessStatusCode)
            {
                string Content = await ticketResponse.Content.ReadAsStringAsync();
                noOfTickets = JsonConvert.DeserializeObject<long>(Content);

            }
            else
            {
                return Content("An error occurred.");
            }
            return Content(noOfTickets.ToString());
        }

        public async Task<IActionResult> Get(string id)
        {
            HttpClient client = MVCClientHttpClient.GetClient();

            HttpResponseMessage ticketResponse = await client.GetAsync("/api/ticket/getticketbyid/" + id);

            if (ticketResponse.IsSuccessStatusCode)
            {
                string Content = await ticketResponse.Content.ReadAsStringAsync();
                var foundTicket = JsonConvert.DeserializeObject<Ticket>(Content);
            }
            else
            {
                return Content("Ticket not found.");
            }
            return null;
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            TicketVM ticketVM = new TicketVM();

            //Getting Employees to use for ticket form
            HttpClient client = MVCClientHttpClient.GetClient();
            HttpResponseMessage userResponse = await client.GetAsync("/api/user/");

            if (userResponse.IsSuccessStatusCode)
            {
                string Content = await userResponse.Content.ReadAsStringAsync();
                var allUsers = JsonConvert.DeserializeObject<IEnumerable<User>>(Content);

                ticketVM.UserList = new SelectList(allUsers.Select(x => x.FirstName));
            }

            return View(ticketVM);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TicketVM ticketVM)
        {
            HttpClient client = MVCClientHttpClient.GetClient();
            var SerializedItemToCreate = JsonConvert.SerializeObject(ticketVM);

            HttpResponseMessage userResponse = await client.PostAsync("/api/ticket/create/",
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
            HttpResponseMessage ticketResponse = await client.GetAsync("/api/ticket/getticketbyid/" + Id);

            if (ticketResponse.IsSuccessStatusCode)
            {

                TicketVM ticketVM = new TicketVM();

                HttpClient clientEmployees = MVCClientHttpClient.GetClient();
                HttpResponseMessage userResponse = await client.GetAsync("/api/user/");

                if (userResponse.IsSuccessStatusCode)
                {
                    string contentEmployees = await userResponse.Content.ReadAsStringAsync();
                    var allUsers = JsonConvert.DeserializeObject<IEnumerable<User>>(contentEmployees);
                    ticketVM.UserList = new SelectList(allUsers.Select(x => x.FirstName));
                }

                string Content = await ticketResponse.Content.ReadAsStringAsync();
                Ticket foundTicket = JsonConvert.DeserializeObject<Ticket>(Content);
                ticketVM.Id = foundTicket.Id;
                ticketVM.Deadline = foundTicket.Deadline;
                ticketVM.Description = foundTicket.Description;
                ticketVM.Type = foundTicket.Type;
                ticketVM.Priority = foundTicket.Priority;
                ticketVM.Status = foundTicket.Status;
                ticketVM.Subject = foundTicket.Subject;
                ticketVM.Login = TempData.Peek("Type").ToString();

                return View(ticketVM);

            }
            return Content("An error occurred.");

        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, TicketVM ticketVM, string adminSave, string employeeSave)
        {
            JsonPatchDocument<Ticket> patchDoc = new JsonPatchDocument<Ticket>();

            if (!string.IsNullOrEmpty(adminSave))
            {
                patchDoc.Replace(e => e.DateTime, DateTime.Now);
                patchDoc.Replace(e => e.Subject, ticketVM.Subject);
                patchDoc.Replace(e => e.Type, ticketVM.Type);
                patchDoc.Replace(e => e.NameOfUser, ticketVM.NameOfUser);
                patchDoc.Replace(e => e.Priority, ticketVM.Priority);
                patchDoc.Replace(e => e.Deadline, ticketVM.Deadline);
                patchDoc.Replace(e => e.Status, ticketVM.Status);
                patchDoc.Replace(e => e.Description, ticketVM.Description);
            }
            else 
            {
                patchDoc.Replace(e => e.DateTime, DateTime.Now);
                patchDoc.Replace(e => e.NameOfUser, ticketVM.NameOfUser);
                patchDoc.Replace(e => e.Status, ticketVM.Status);
            }


            //serialize patch
            var serializedPatch = JsonConvert.SerializeObject(patchDoc);

            HttpClient client = MVCClientHttpClient.GetClient();

            HttpResponseMessage ticketResponse = await client.PatchAsync("/api/ticket/update/" + id,
                                                new StringContent(serializedPatch, System.Text.Encoding.Unicode,
                                                "application/json"));


            if (ticketResponse.IsSuccessStatusCode)
            {
                return this.RedirectToAction("Index");
            }

            return Content("An error occurred.");
        }

        public async Task<IActionResult> Delete(string Id)
        {
            HttpClient client = MVCClientHttpClient.GetClient();
            HttpResponseMessage ticketResponse = await client.DeleteAsync("/api/ticket/deleteticket/" + Id);

            if (ticketResponse.IsSuccessStatusCode)
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

