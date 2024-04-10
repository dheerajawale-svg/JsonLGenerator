using System.Diagnostics;
using System.Text.Json;

namespace JsonLGenerator
{
    internal class Program
    {
        static string outputFileName = "output_QnA{0}.jsonl";

        static void Main(string[] args)
        {
            Console.WriteLine("JsonL Generator!");

            Console.WriteLine("Enter the directory path where files are located");
            var inputDirName = Console.ReadLine();

            var allFiles = Directory.GetFiles(inputDirName, "*.json");

            var outputFileCounter = 1;

            //Create output file.
            string jsonlFileName = string.Format(outputFileName, outputFileCounter++);
            File.Create(jsonlFileName).Close();

            //Read sample file 1st.
            Helper.ReadInfoOld(File.ReadAllText("custom_dataset_2.json"), jsonlFileName);

            for (int i = 0; i < allFiles.Length; i++)
            {
                if (IsMultipleOfTen(i + 1))
                {
                    jsonlFileName = string.Format(outputFileName, outputFileCounter++);
                    File.Create(jsonlFileName).Close();
                }

                string inputFileName = allFiles[i];
                var data = File.ReadAllText(inputFileName);

                TranformFileInfo(data, jsonlFileName);
            }

            Console.WriteLine("Done!");

            Console.ReadLine();
        }

        private static void TranformFileInfo(string fileData, string outputFile)
        {
            message system = new()
            {
                content = "You are a medical assistent who can cleverly ask patients about problem they have",
                role = "system"
            };

            message userFirst = new()
            {
                role = "user",
                content = "Hello, I am having difficulty in hearing."
            };

            root fileEntry = new()
            {
                messages = [system, userFirst]
            };

            try
            {
                var items = JsonSerializer.Deserialize<Dictionary<string, string>>(fileData);
                Helper.ReadInfoFormatDictionary(fileEntry, items);
            }
            catch (JsonException ex)
            {
                Debug.WriteLine(ex);
                Helper.ReadInfoFormatJArray(fileData, fileEntry);
            }

            message conclusion = new()
            {
                role = "assistant",
                content = "--conclusion here--"
            };
            fileEntry.messages.Add(conclusion);

            var t1 = JsonSerializer.Serialize(fileEntry);
            File.AppendAllText(outputFile, t1 + Environment.NewLine);
        }

        public static bool IsMultipleOfTen(int number)
        {
            return number % 10 == 0;
        }
    }
}
