using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models
{
    public class Ticket : IEntityBase
    {
        public Ticket()
        {
            TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
            DateTime timeUTC = DateTime.UtcNow;
            DateTime = TimeZoneInfo.ConvertTimeFromUtc(timeUTC, timeZone);          
        }

        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Subject { get; set; }
        public TypeOfIncidentEnum Type { get; set; }
        public string NameOfUser { get; set; }
        public PriorityOfIncidentEnum Priority { get; set; }
        public DeadlineEnum Deadline { get; set; }
        public TicketStatusEnum Status { get; set; }
        public string Description { get; set; }

    }
}
