using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AlsRitter.GenerateMap.V1.Do {
    // Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
    public class JInitial {
        [JsonProperty("x")] public int X { get; set; }

        [JsonProperty("y")] public int Y { get; set; }

        [JsonProperty("speed")] public double Speed { get; set; }

        [JsonProperty("jumpMin")] public double JumpMin { get; set; }

        [JsonProperty("jumpMax")] public double JumpMax { get; set; }

        [JsonProperty("jumpSpeed")] public double JumpSpeed { get; set; }
    }

    public class JBackground {
        [JsonProperty("bg_id")] public string BgId { get; set; }

        [JsonProperty("color")] public string Color { get; set; }
    }

    public class JPrefab {
        [JsonProperty("prefab_id")] public string PrefabId { get; set; }

        [JsonProperty("x")] public int X { get; set; }

        [JsonProperty("y")] public int Y { get; set; }
    }

    public class JTileData {
        [JsonProperty("key")] public string Key { get; set; }

        [JsonProperty("layer")] public int Layer { get; set; }

        [JsonProperty("tile_sprite_id")] public string TileSpriteId { get; set; }

        [JsonProperty("color")] public string Color { get; set; }

        [JsonProperty("effect_keys")] public List<string> EffectKeys { get; set; }
    }

    public class JTilePoint {
        [JsonProperty("x")] public int X { get; set; }
        
        [JsonProperty("y")] public int Y { get; set; }
        
        [JsonProperty("key")] public string Key { get; set; }
        
        [JsonProperty("layer")] public int Layer { get; set; }
    }

    /**
     * JSON to Bean
     */
    public class ConvertMapData {
        [JsonProperty("create_time")] 
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime CreateTime { get; set; }

        [JsonProperty("version")] public string Version { get; set; }

        [JsonProperty("author")] public string Author { get; set; }

        [JsonProperty("initial")] public JInitial Initial { get; set; }

        [JsonProperty("background")] public JBackground Background { get; set; }

        [JsonProperty("prefabs")] public List<JPrefab> Prefabs { get; set; }

        [JsonProperty("tileData")] public List<JTileData> TileData { get; set; }

        [JsonProperty("tiles")] public List<JTilePoint> Tiles { get; set; }
    }
}