using System.Text.Json.Serialization;

namespace UnrealPluginBuilder
{
    class PluginInfo
    {
        [JsonPropertyName("FriendlyName")]
        public string PluginName { get; set; }
        [JsonPropertyName("VersionName")]
        public string VersionName { get; set; }
        [JsonPropertyName("CanContainContent")]
        public bool CanContainContent { get; set; }
    }
}
