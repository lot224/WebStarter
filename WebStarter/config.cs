using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace WebStarter {
    public class Config {

        public List<ConfigEntry> Entries { get; set; }

        public Config() {
            Entries = LoadSolutions();

            if (Entries.Count == 0) {
                Entries.Add(new ConfigEntry() { Active = true, MaxAttempts = 1, Name = "Test 1", TimeOut = 5, Url = "http://127.0.0.1:80" });
                Entries.Add(new ConfigEntry() { Active = true, MaxAttempts = 1, Name = "Test 2", TimeOut = 5, Url = "http://127.0.0.1:80" });
                Entries.Add(new ConfigEntry() { Active = true, MaxAttempts = 1, Name = "Test 2", TimeOut = 5, Url = "http://127.0.0.1:80" });
                Save(Entries);

                Console.WriteLine("No config entries found, creating example entries.");
                Console.WriteLine("Entries written to (" + FileName + ")." + Environment.NewLine);

            }
        }

        private static void Save(object obj) {
            string Data = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            File.WriteAllText(FileName, Data);
        }

        private static string FileName {
            get {
                var assembly = Assembly.GetExecutingAssembly();
                var directory = new System.IO.FileInfo(assembly.Location).DirectoryName;
                System.IO.Directory.CreateDirectory(directory);
                return Path.Combine(directory, "config.json");
            }
        }

        private static List<ConfigEntry> LoadSolutions() {
            List<ConfigEntry> nResult = new List<ConfigEntry>();
            if (System.IO.File.Exists(FileName)) {
                string data = System.IO.File.ReadAllText(FileName);
                nResult = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ConfigEntry>>(data);
            }
            return nResult;
        }
    }


    public class ConfigEntry {
        public string Name { get; set; }
        public string Url { get; set; }
        public int TimeOut { get; set; }
        public int MaxAttempts { get; set; }
        public bool Active { get; set; }
    }

}