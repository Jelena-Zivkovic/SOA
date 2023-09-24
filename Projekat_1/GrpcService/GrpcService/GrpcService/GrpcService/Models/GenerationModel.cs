using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcService.Models
{
    public class GenerationModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ID { get; set; }

        [BsonElement("DATE_TIME")]
        public string Date_time { get; set; }
        [BsonElement("PLANT_ID")]
        public string Plant_id { get; set; }

        [BsonElement("SOURCE_KEY")]
        public string Source_key { get; set; }

        [BsonElement("DC_POWER")]
        public string Dc_power { get; set; }

        [BsonElement("AC_POWER")]
        public string Ac_power { get; set; }

        [BsonElement("DAILY_YIELD")]
        public string Daily_yield { get; set; }

        [BsonElement("TOTAL_YIELD")]
        public string Total_yield { get; set; }
    }
}
