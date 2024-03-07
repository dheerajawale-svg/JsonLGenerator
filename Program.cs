using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JsonLGenerator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("JsonL Generator!");

            Console.WriteLine("Enter the data file path");
            var inputFileName = Console.ReadLine();

            if (string.IsNullOrEmpty(inputFileName))
                inputFileName = "custom_dataset_2.json";

            var data = File.ReadAllText(inputFileName);

            var jsonFileName = "patientQnAdata.jsonl";
            File.Create(jsonFileName).Close();

            roots collection = new roots() { rootdata = new() };

            var dictionary = JsonSerializer.Deserialize<Dictionary<string, List<Dictionary<string, string>>>>(data);
            foreach (var items in dictionary["PatientInfo"])
            {
                foreach (KeyValuePair<string, string> kvp in items)
                {
                    message system = new message
                    {
                        content = "You are a medical assistent who can cleverly ask patients about problem they have",
                        role = "system"
                    };

                    message user = new message
                    {
                        role = "user",
                        content = kvp.Key
                    };

                    message assistent = new message
                    {
                        role = "assistant",
                        content = kvp.Value
                    };

                    root obj = new root
                    {
                        messages = new List<message> { system, user, assistent }
                    };

                    var t1 = JsonSerializer.Serialize(obj);
                    File.AppendAllText(jsonFileName, t1 + Environment.NewLine);

                    collection.rootdata.Add(obj);
                }
            }

            //var output = JsonSerializer.Serialize(collection.rootdata);

            //File.WriteAllText("output.jsonl", output);

            //var jsonData = (JObject)JsonConvert.DeserializeObject(data);
            //var patientInfo = jsonData["PatientInfo"].Values();

            //foreach (var item in patientInfo)
            //{
            //    Debug.WriteLine(item.ToString());
            //}

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
