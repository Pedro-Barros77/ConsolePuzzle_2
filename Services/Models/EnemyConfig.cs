using Newtonsoft.Json;

namespace ConsolePuzzle_2.Services.Models
{
    /// <summary>
    /// This is a class to map json enemy config to a object.
    /// </summary>
    public class EnemyConfig
    {
        [JsonProperty("x")]
        public int X { get; set; }

        [JsonProperty("y")]
        public int Y { get; set; }

        [JsonProperty("speed")]
        public float Speed { get; set; }

        [JsonProperty("delay")]
        public float Delay { get; set; }

        [JsonProperty("dir")]
        public string Dir { get; set; } = "down";

        [JsonProperty("startDelay")]
        public float StartDelay { get; set; }

        [JsonProperty("bulletSpeed")]
        public float BulletSpeed { get; set; }

        [JsonProperty("rateOfFire")]
        public float RateOfFire { get; set; }

        [JsonProperty("movementRadius")]
        public int MovementRadius { get; set; }

        [JsonProperty("sightRadius")]
        public int SightRadius { get; set; }
    }
}
