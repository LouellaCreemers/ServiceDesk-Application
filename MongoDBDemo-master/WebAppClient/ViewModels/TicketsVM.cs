using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppClient.ViewModels
{
    public class TicketsVM
    {
        public IEnumerable<Ticket> lstTickets { get; set; }
        public string TextSearch { get; set; }

        public string FilterKeyword { get; set; }
    }
}
