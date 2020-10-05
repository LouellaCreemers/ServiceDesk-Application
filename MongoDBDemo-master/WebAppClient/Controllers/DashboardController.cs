using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;
using Newtonsoft.Json;
using WebAppClient.Helpers;
using WebAppClient.ViewModels;

namespace WebAppClient.Controllers
{
    public class DashboardController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index(DashboardVM model)
        {
            HttpClient client = MVCClientHttpClient.GetClient();
            HttpResponseMessage allResponse = await client.GetAsync("api/ticket/");
            HttpResponseMessage openResponse = await client.GetAsync("api/ticket/countticketopen/");
            HttpResponseMessage overdueResponse = await client.GetAsync("api/ticket/countticketoverdue/");

            if (allResponse.IsSuccessStatusCode && openResponse.IsSuccessStatusCode)
            {
                string allContent = await allResponse.Content.ReadAsStringAsync();
                string openContent = await openResponse.Content.ReadAsStringAsync();
                string overdueContent = await overdueResponse.Content.ReadAsStringAsync();

                model.AllTickets = JsonConvert.DeserializeObject<IEnumerable<Ticket>>(allContent);
                model.OpenTickets = JsonConvert.DeserializeObject<IEnumerable<Ticket>>(openContent);
                model.OverdueTickets = JsonConvert.DeserializeObject<IEnumerable<Ticket>>(overdueContent);
            }

            else { return View(new DashboardVM()); }

            return View(model);

        }

    }
}
