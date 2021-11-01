using System.Text.Json.Serialization;

namespace UnrealPluginBuilder
{
    class EngineVersion
    {
        [JsonPropertyName("MajorVersion")]
        public int MajorVersion { get; set; }
        [JsonPropertyName("MinorVersion")]
        public int MinorVersion { get; set; }
    }
}
