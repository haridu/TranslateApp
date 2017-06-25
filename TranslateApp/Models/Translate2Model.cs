using System;
using Newtonsoft.Json;

namespace TranslateApp
{
    public class Translate2Model
    {
        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; }

        [JsonProperty(PropertyName = "username")]
        public float Longitude { get; set; }

        [JsonProperty(PropertyName = "password")]
        public float Latitude { get; set; }

        public string City { get; set; }

    }
}
