using Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public class TicketRepository : EntityBaseRepository<Ticket>, ITicketRepository
    {
        private IMongoDatabase mongoDataBase;
        private string _databaseName;

        string connectionString = "mongodb+srv://lou:lou@cluster0.wewsm.gcp.mongodb.net/test?authSource=admin&replicaSet=atlas-lznwzk-shard-0&readPreference=primary&appname=MongoDB%20Compass&ssl=true";

        public TicketRepository() : base()
        {   
            var client = new MongoClient(connectionString);
            _databaseName = "TheGardenGroup";
            mongoDataBase = client.GetDatabase(_databaseName);
        }

        public IEnumerable<User> GetEmployeesForTicket()
        {
            var _collectionGetter = mongoDataBase.GetCollection<User>("Users");
            var list = _collectionGetter.Find(_ => true).ToList();
            return list.Where(x => x.Type == UserEnum.Employee);
        }

        public void AddToArchive(Ticket ticket, string collection)
        {
            var client = new MongoClient(connectionString);
            _databaseName = "ArchiveTGG";
            mongoDataBase = client.GetDatabase(_databaseName);

            var _collectionGetter = mongoDataBase.GetCollection<Ticket>(collection);
            _collectionGetter.InsertOne(ticket);
        }

    }
}
