using Microsoft.AspNetCore.Mvc.Rendering;
using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppClient.ViewModels
{
    public class TicketVM
    {
        public string Id { get; set; }

        [DisplayName("Ticket Subject: ")]
        public string Subject { get; set; }

        [DisplayName("Incident Type:  ")]
        public TypeOfIncidentEnum Type { get; set; }

        [DisplayName("Reported By User: ")]
        public string NameOfUser { get; set; }

        [DisplayName("Priority: ")]
        public PriorityOfIncidentEnum Priority { get; set; }

        [DisplayName("Status: ")]
        public TicketStatusEnum Status { get; set; }

        [DisplayName("Days Before Deadline: ")]
        public DeadlineEnum Deadline { get; set; }

        [DisplayName("Description: ")]
        public string Description { get; set; }

        public virtual SelectList UserList { get; set; }
        public string Login { get; set; }
    }
}
