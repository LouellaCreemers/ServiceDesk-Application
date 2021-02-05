using System;
using System.Collections.Generic;
using Models;
using DAL;
using System.Text;
using System.Linq.Expressions;
using System.Linq;
using MongoDB.Driver;

namespace Services.TicketService
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _repo;
        private string collection = "Tickets";

        public TicketService(ITicketRepository repo)
        {
            _repo = repo;
        }
        public void AddTicket(Ticket tick)
        {
            _repo.Add(tick, collection);
        }

        public long CountThemAll()
        {
            return _repo.Count(collection);
        }

        public IEnumerable<Ticket> CountOpenTickets()
        {
            var list = _repo.GetAll(collection);
            return list.Where(x => x.Status.Equals(TicketStatusEnum.Open));
        }

        public IEnumerable<Ticket> CountOverdueTickets()
        {
            var list = _repo.GetAll(collection);

            var listOneDay = list.Where(x => (x.Deadline == DeadlineEnum.one) && (x.Status == TicketStatusEnum.Open));
            var listSevenDays = list.Where(x => (x.Deadline == DeadlineEnum.seven) && (x.Status == TicketStatusEnum.Open));
            var listThirtyDays = list.Where(x => (x.Deadline == DeadlineEnum.thirty) && (x.Status == TicketStatusEnum.Open));

            var allOverdueTickets = new List<Ticket>();

            foreach (var item in listOneDay)
            {
                if (item.DateTime.AddDays(1) < DateTime.Now)
                {
                    allOverdueTickets.Add(item);
                }
            }

            foreach (var item in listSevenDays)
            {
                if (item.DateTime.AddDays(7) < DateTime.Now)
                {
                    allOverdueTickets.Add(item);
                }
            }

            foreach (var item in listThirtyDays)
            {
                if (item.DateTime.AddDays(30) < DateTime.Now)
                {
                    allOverdueTickets.Add(item);
                }
            }
            return allOverdueTickets;
        }

        public IEnumerable<Ticket> GetAllTickets()
        {
            return _repo.GetAll(collection);
        }

        public IEnumerable<Ticket> GetAllTicketsSearch(string input)
        {
            var list = _repo.GetAll(collection);
            return list.Where(x => (x.Subject.Contains(input.ToLower()) || (x.Subject.Contains(input))));
        }

        public IEnumerable<Ticket> GetAllOldTickets() 
        {
            var list = _repo.GetAll(collection);
            var oldTickets = list.Where(x => x.DateTime.AddYears(2) < DateTime.Now);

            foreach (var item in oldTickets) 
            {
                _repo.AddToArchive(item, collection);
            }
            return oldTickets;
        }

        public IEnumerable<Ticket> GetAllTicketsSorted()
        {
            var list = _repo.GetAll(collection);
            return list.Where(x => x.Status == TicketStatusEnum.Open).OrderByDescending(x => x.Priority).ThenByDescending(x => x.DateTime);
        }

        public Ticket GetSingle(string id)
        {
            return _repo.GetSingle(id, collection);
        }

        public Ticket GetSingleWherePredicate(Expression<Func<Ticket, bool>> predicate)
        {
            return _repo.GetSingleItemPredicate(predicate, collection);
        }

        public void RemoveTicket(Ticket tick)
        {
            _repo.Delete(tick, collection);
        }

        public void UpdateTicket(Ticket tick)
        {
            _repo.Update(tick, collection);
        }
    }
}
