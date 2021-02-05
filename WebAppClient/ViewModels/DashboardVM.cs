using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppClient.ViewModels
{
    public class DashboardVM
    {
        public IEnumerable<Ticket> OverdueTickets { get; set; }
        public IEnumerable<Ticket> OpenTickets { get; set; }
        public IEnumerable<Ticket> AllTickets { get; set; }
    }
}
