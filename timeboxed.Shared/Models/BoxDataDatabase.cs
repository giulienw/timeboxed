using System.Collections.Generic;
using Newtonsoft.Json;
using SQLite;

namespace timeboxed.Models
{
    public class BoxDataDatabase
    {
        [PrimaryKey]
        public string _id { get; set; }
        public string createdAt { get; set; }
        public string exposure { get; set; }
        public string grouptag { get; set; }
        public string Image { get; set; }
        public string currentLocation { get; set; }
        public string name { get; set; }
        public string updatedAt { get; set; }
        public string state { get; set; }

        public BoxData ToNormal()
        {
            return new BoxData
            {
                _id = _id,
                createdAt = createdAt,
                exposure = exposure,
                grouptag = JsonConvert.DeserializeObject<string[]>(grouptag),
                Image = Image,
                currentLocation = JsonConvert.DeserializeObject<BoxcurrentLocation>(currentLocation),
                name = name,
                updatedAt = updatedAt,
                state = state
            };
        }
    }
}