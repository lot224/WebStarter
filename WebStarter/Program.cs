using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebStarter {
    class Program {
        static void Main(string[] args) {

            Task ccclx = Task.Factory.StartNew(() => ExecuteEndpoint("http://dev.api.ccclx/swagger/ui/index"));
            Task client = Task.Factory.StartNew(() => ExecuteEndpoint("http://dev.api.client/swagger/ui/index"));
            Task cms = Task.Factory.StartNew(() => ExecuteEndpoint("http://dev.api.cms/swagger/ui/index"));
            Task imaging = Task.Factory.StartNew(() => ExecuteEndpoint("http://dev.api.imaging/swagger/ui/index"));
            Task policy = Task.Factory.StartNew(() => ExecuteEndpoint("http://dev.api.policy/swagger/ui/index"));
            Task safety = Task.Factory.StartNew(() => ExecuteEndpoint("http://dev.api.safety/swagger/ui/index"));
            Task shared = Task.Factory.StartNew(() => ExecuteEndpoint("http://dev.api.shared/swagger/ui/index"));
            Task sso = Task.Factory.StartNew(() => ExecuteEndpoint("http://dev.api.sso/swagger/ui/index"));
            Task vendor = Task.Factory.StartNew(() => ExecuteEndpoint("http://dev.api.vendor/swagger/ui/index"));

            Task.WaitAll(ccclx, client, cms, imaging, policy, safety, shared, sso, vendor);

            Console.WriteLine(Environment.NewLine + "Completed, Everything should be awake now.");
            Console.WriteLine(Environment.NewLine + "Press any key to exit.");
            Console.ReadKey();
        }


        static void ExecuteEndpoint(string url, int retry = 0) {

            switch (retry) {
                case 0: Console.WriteLine("Executing " + url + " ( 1st attempt )"); break;
                case 1: Console.WriteLine("Executing " + url + " ( 2nd attempt )"); break;
                case 2: Console.WriteLine("Executing " + url + " ( 3rd attempt )"); break;
                case 3: Console.WriteLine("Executing " + url + " ( final attempt )"); break;
            }

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            try {
                request.GetResponse();
            } catch {
                if (retry < 3) {
                    System.Threading.Thread.Sleep(5000);
                    ExecuteEndpoint(url, retry += 1);
                }
            }
        }
    }
}
