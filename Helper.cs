using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace JsonLGenerator
{
    internal class Helper
    {
        public static void ReadInfoOld(string data, string outputFileName)
        {
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
                File.AppendAllText(outputFileName, t1 + Environment.NewLine);
            }
        }

        public static void ReadTest()
        {
            string file = @"C:\Users\dheeraj.awale\Desktop\AI Samples\QnA\sample12 tinnitus.json";
            var data = File.ReadAllText(file);

            ReadInfoFormatJArray(data, new root());
        }

        public static void ReadInfoFormatDictionary(root fileEntry, Dictionary<string, string> items)
        {
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

                fileEntry.messages.Add(assistent);
                fileEntry.messages.Add(user);
            }
        }

        public static void ReadInfoFormatJArray(string data, root fileEntry)
        {
            JsonArray array = JsonNode.Parse(data)?.AsArray();

            foreach (var item in array)
            {
                var qVal = item["Question"];
                var aVal = item["Answer"];

                message assistent = new()
                {
                    role = "assistant",
                    content = qVal.ToString()
                };

                message user = new()
                {
                    role = "user",
                    content = aVal.ToString()
                };

                fileEntry.messages.Add(assistent);
                fileEntry.messages.Add(user);
            }
        }
    }

    class FileData
    {
        public Row[] Rows { get; set; }
    }

    class Row
    {
        public string Question { get; set; }

        public string Answer { get; set; }
    }
}
