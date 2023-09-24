using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rest.Models
{
    public class Generation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ID { get; set; }

        [BsonElement("DATE_TIME")]
        public DateTime Date_time { get; set; }
        [BsonElement("PLANT_ID")]
        public long Plant_id { get; set; }

        [BsonElement("SOURCE_KEY")]
        public string Source_key { get; set; }

        [BsonElement("DC_POWER")]
        public double Dc_power { get; set; }

        [BsonElement("AC_POWER")]
        public double Ac_power { get; set; }

        [BsonElement("DAILY_YIELD")]
        public double Daily_yield { get; set; }

        [BsonElement("TOTAL_YIELD")]
        public double Total_yield { get; set; }
    }
}
