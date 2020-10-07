using Models;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Services.TicketService
{
    public interface ITicketService
    {
        IEnumerable<Ticket> GetAllTickets();
        IEnumerable<Ticket> GetAllTicketsSorted();
        IEnumerable<Ticket> GetAllTicketsSearch(string input);
        IEnumerable<Ticket> GetAllOldTickets();
        IEnumerable<Ticket> CountOverdueTickets();
        IEnumerable<Ticket> CountOpenTickets();
        long CountThemAll();

        Ticket GetSingle(string id);

        Ticket GetSingleWherePredicate(Expression<Func<Ticket, bool>> predicate);

        void AddTicket(Ticket tick);

        void UpdateTicket(Ticket tick);

        void RemoveTicket(Ticket tick);
    }
}
