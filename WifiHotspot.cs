using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WifiHotspotDotnet
{
    public class WifiHotspot
    {
        public String SSID { get; set; }

        private String _passphrase;
        public String Passphrase
        {
            get
            {
                return _passphrase;
            }
            set
            {
                if (value.Length < 8 || value.Length > 63)
                {
                    throw new PassphraseLengthException();
                }

                _passphrase = value;
            }
        }

        public int ClientsConnected
        {
            get
            {
                var s = WriteAndReadProcessData("netsh wlan show hostednetwork");
                Console.Write(s);
                return 0;
            }
        }

        public WifiHotspot(String ssid, string passphrase)
        {
            this.SSID = ssid;
            this.Passphrase = passphrase;
            Initialize();
        }

        private Process GetDefaultCMDProcess()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo("cmd")
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            Process process = new Process();
            process.StartInfo = startInfo;
            process.Start();

            return process;
        }

        private List<String> WriteAndReadProcessData(String command)
        {
            Process process = GetDefaultCMDProcess();

            process.StandardInput.WriteLine(command);
            process.StandardInput.WriteLine("exit");

            //Reads all data 
            String output = process.StandardOutput.ReadToEnd();
            List<String> outputList = MultipleLinesToStringList(output);

            //Index where the response of our command started
            int startIndex = outputList.IndexOf(outputList.First(o => o.EndsWith(command))) + 1;

            List<String> commandData = outputList.GetRange(startIndex, outputList.Count - startIndex - 1);

            return commandData;
        }

        private List<String> MultipleLinesToStringList(String text)
        {
            return new List<string>(text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries));
        }

        public void Initialize()
        {
            WriteAndReadProcessData(String.Format("netsh wlan set hostednetwork mode=allow ssid=\"{0}\" key=\"{1}\" ", this.SSID, this.Passphrase));

        }

        public void Start()
        {
            WriteAndReadProcessData("netsh wlan start hosted network");
        }

        public void Stop()
        {
            WriteAndReadProcessData("netsh wlan stop hosted network");
        }
    }
}
