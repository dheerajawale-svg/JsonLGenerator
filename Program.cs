using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JsonLGenerator
{
    internal class Program
    {
        static string jsonOutputFileName = "patientQnAdata.jsonl";

        static void Main(string[] args)
        {
            Console.WriteLine("JsonL Generator!");

            Console.WriteLine("Enter the data file path");
            var inputFileName = Console.ReadLine();

            if (string.IsNullOrEmpty(inputFileName))
                inputFileName = "custom_dataset_2.json";

            var data = File.ReadAllText(inputFileName);

            File.Create(jsonOutputFileName).Close();

            roots collection = new roots() { rootdata = new() };

            var dictionary = JsonSerializer.Deserialize<Dictionary<string, List<Dictionary<string, string>>>>(data);
            foreach (var items in dictionary["PatientInfo"])
            {
                message system = new message
                {
                    content = "You are a medical assistent who can cleverly ask patients about problem they have",
                    role = "system"
                };

                message userFirst = new message
                {
                    role = "user",
                    content = "Hello, I am having difficulty in hearing."
                };

                root obj = new()
                {
                    messages = [system, userFirst]
                };

                for (int index = 0; index < items.Count; index++)
                {
                    var kvp = items.ElementAt(index);

                    message assistent = new()
                    {
                        role = "assistant",
                        content = kvp.Key
                    };

                    message user = new()
                    {
                        role = "user",
                        content = kvp.Value
                    };

                    obj.messages.Add(assistent);
                    obj.messages.Add(user);
                }

                var t1 = JsonSerializer.Serialize(obj);
                File.AppendAllText(jsonOutputFileName, t1 + Environment.NewLine);
            }

            Console.WriteLine("Done!");

            Console.ReadLine();
        }
    }

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
        public List<message> messages { get; set; }
    }

    public class roots
    {
        public List<root> rootdata { get; set; }
    }
#pragma warning restore CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.
#pragma warning restore IDE1006 // Naming Styles
}
