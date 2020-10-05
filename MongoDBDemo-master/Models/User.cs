using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Models
{
    public class User : IEntityBase
    {

        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public UserEnum Type { get; set; }
        public string EmailAdress { get; set; }
        public string Password { get; set; }
        public string Phonenumber { get; set; }
        public string Location { get; set; }


    }
}
