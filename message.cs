using System.Text.Json.Serialization;

namespace JsonLGenerator
{
#pragma warning disable IDE1006
#pragma warning disable CS8981
    public class message
    {
        [JsonPropertyOrder(1)]
        public string content { get; set; }

        public string role { get; set; }
    }


    public class root
    {
        public List<message> messages { get; set; } = [];
    }

    public class roots
    {
        public List<root> rootdata { get; set; }
    }

#pragma warning restore CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.
#pragma warning restore IDE1006 // Naming Styles
}
