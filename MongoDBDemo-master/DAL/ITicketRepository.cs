using System;
using System.Collections.Generic;
using System.Text;
using Models;

namespace DAL
{
    public interface ITicketRepository : IBaseRepository<Ticket>
    {
        void AddToArchive(Ticket ticket, string collection);
        IEnumerable<User> GetEmployeesForTicket();
    }
}
