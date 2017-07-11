using Newtonsoft.Json;

namespace _5th_registration.Models
{
    public class LineData
    {
        [JsonProperty("startX")]
        public float StartX { get; set; }
        [JsonProperty("startY")]
        public float StartY { get; set; }
        [JsonProperty("endX")]
        public float EndX { get; set; }
        [JsonProperty("endY")]
        public float EndY { get; set; }
    }
}