using System;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;
using Microsoft.UI.Xaml.Data;

namespace timeboxed.Models
{
    [Bindable]
    public class BoxData
    {
        [JsonProperty(PropertyName = "_id"), PrimaryKey]
        public string _id { get; set; }

        [JsonProperty(PropertyName = "createdAt")]
        public string createdAt { get; set; }

        [JsonProperty(PropertyName = "exposure")]
        public string exposure { get; set; }

        [JsonProperty(PropertyName = "grouptag")]
        public string[] grouptag { get; set; }

        [JsonProperty(PropertyName = "Image")]
        public string Image { get; set; }

        [JsonProperty(PropertyName = "currentLocation")]
        public BoxcurrentLocation currentLocation { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string name { get; set; }

        [JsonProperty(PropertyName = "sensors")]
        [OneToMany(CascadeOperations = CascadeOperation.CascadeDelete)]
        public List<BoxSensor> sensors { get; set; }

        [JsonProperty(PropertyName = "updatedAt")]
        public string updatedAt { get; set; }

        [JsonProperty(PropertyName = "state")]
        public string state { get; set; }

        public BoxDataDatabase ToDatabase()
        {
            return new BoxDataDatabase
            {
                _id = _id,
                createdAt = createdAt,
                exposure = exposure,
                grouptag = JsonConvert.SerializeObject(grouptag),
                Image = Image,
                currentLocation = JsonConvert.SerializeObject(currentLocation),
                name = name,
                updatedAt = updatedAt,
                state = state
            };
        }
    }

    public class BoxSensor
    {
        [JsonProperty(PropertyName = "_id")]
        public string _id { get; set; }

        [JsonProperty(PropertyName = "lastMeasurment")]
        public BoxlastMeasurment lastMeasurment { get; set; }

        [JsonProperty(PropertyName = "sensorType")]
        public string sensorType { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string title { get; set; }

        [JsonProperty(PropertyName = "unit")]
        public string unit { get; set; }
    }

    public class BoxlastMeasurment
    {
        [ForeignKey(typeof(BoxSensor))]
        public string sensorid { get; set; }
        [JsonProperty("value")]
        public string value { get; set; }

        [JsonProperty("createdAt")]
        public string createdAt { get; set; }
    }

    public class BoxcurrentLocation
    {
        [JsonProperty("coordinates")]
        public double[] coordinates { get; set; }

        [JsonProperty("timestamp")]
        public string timestamp { get; set; }

        [JsonProperty("type")]
        public string type { get; set; }
    }
}

