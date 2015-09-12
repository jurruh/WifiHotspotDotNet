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

        public NetworkInfo Info
        {
            get
            {
                List<String> response = WriteAndReadProcessData("netsh wlan show hostednetwork");

                return new NetworkInfo(response);
            }
        }

        public WifiHotspot(String ssid, string passphrase)
        {
            this.SSID = ssid;
            this.Passphrase = passphrase;
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
            List<String> response = WriteAndReadProcessData(String.Format("netsh wlan set hostednetwork mode=allow ssid=\"{0}\" key=\"{1}\" ", this.SSID, this.Passphrase));

            if (response[0] != "The hosted network mode has been set to allow. ")
            {

            }

            if (response[1] != "The SSID of the hosted network has been successfully changed. ")
            {

            }
        }

        public void Start()
        {
            Initialize();

            List<String> response = WriteAndReadProcessData("netsh wlan start hosted network");

            if (response[0] != "The hosted network started. ")
            {
                throw new HostedNetworkNotStartedException();
            }
        }

        public void Stop()
        {
            List<String> response = WriteAndReadProcessData("netsh wlan stop hosted network");

            if (response[0] != "The hosted network stopped. ")
            {
                throw new HostedNetworkNotStoppedException();
            }
        }
    }
}
