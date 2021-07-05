using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson.Serialization.Attributes;

namespace MVCMongodbCrudOpration.Models
{
    public class info
    {
        [BsonId]
        public MongoDB.Bson.BsonObjectId Id { get; set; }
        public int info_id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public int age { get; set; }
    }
}