using System;
using Newtonsoft.Json;

namespace TranslateApp
{
    public class Translate2Model
    {
        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; }

        [JsonProperty(PropertyName = "username")]
        public string username { get; set; }

        [JsonProperty(PropertyName = "password")]
        public string password { get; set; }

       

    }
}
