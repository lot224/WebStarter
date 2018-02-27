using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebStarter {
    class Program {

        static int maxNameLength = 0;
        static int maxUrlLength = 0;

        static void Main(string[] args) {

            List<ConfigEntry> Entries = new Config().Entries;

            maxNameLength = (from x in Entries orderby x.Name.Length descending select x.Name.Length).FirstOrDefault();
            maxUrlLength = (from x in Entries orderby x.Url.Length descending select x.Url.Length).FirstOrDefault();

            List<Task> Tasks = new List<Task>();

            foreach (ConfigEntry entry in Entries) {
                if (entry.Active) {
                    Task task = Task.Factory.StartNew(() => ExecuteEndpoint(entry));
                    Tasks.Add(task);
                }
            }

            Task.WaitAll(Tasks.ToArray());

            Console.WriteLine(Environment.NewLine + "Completed, Everything should be awake now.  If not, run it again.");
            Console.WriteLine(Environment.NewLine + "Press any key to exit.");
            Console.ReadKey();
        }

        static void ExecuteEndpoint(ConfigEntry entry, int attempt = 0) {

            attempt += 1;

            string ordinal = "";

            switch (attempt.ToString().Last()) {
                case '1': ordinal = attempt + "st"; break;
                case '2': ordinal = attempt + "nd"; break;
                case '3': ordinal = attempt + "rd"; break;
                default: ordinal = attempt + "th"; break;
            }

            if (attempt == entry.MaxAttempts)
                ordinal = "Final";

            Console.WriteLine(string.Format("{0} {1} ({2} Attempt)", entry.Name.PadRight(maxNameLength), entry.Url.PadRight(maxUrlLength), ordinal));

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(entry.Url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            try {
                request.GetResponse();
            } catch {
                if (attempt < entry.MaxAttempts) {
                    System.Threading.Thread.Sleep(entry.TimeOut);
                    ExecuteEndpoint(entry, attempt);
                }
            }
        }

    }
}