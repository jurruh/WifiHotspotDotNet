using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WifiHotspotDotnet
{
    public class NetworkInfo
    {
        private List<String> Data;

        public String Mode { get; }
        public String SSID { get; }
        public String Authentication { get; }
        public String Cipher { get; }
        public String BSSID { get; }
        public String RadioType { get; }

        public int MaxClients { get; }
        public int Channel { get; }
        public int NumberClients { get; }

        public NetworkInfo(List<String> data)
        {
            this.Data = data;

            this.Mode = GetDataValue("Mode");
            this.SSID = GetDataValue("SSID name", true);
            this.Authentication = GetDataValue("Authentication");
            this.Cipher = GetDataValue("Cipher");
            this.BSSID = GetDataValue("BSSID");
            this.RadioType = GetDataValue("Radio type");

            try
            {
                this.MaxClients = Convert.ToInt32(GetDataValue("Max number of clients"));
            }
            catch (FormatException) { }

            try
            {
                this.Channel = Convert.ToInt32(GetDataValue("Channel"));
            }
            catch (FormatException) { }

            try
            {
                this.NumberClients = Convert.ToInt32(GetDataValue("Number of clients"));
            }
            catch (FormatException) { }
        }

        private string GetDataValue(String name, bool removeQuotes = false)
        {
            foreach (String s in Data)
            {
                if (s.StartsWith("    " + name))
                {
                    int startIndex = s.IndexOf(":") + 1;

                    string value = s.Substring(startIndex, s.Length - startIndex);

                    if (removeQuotes)
                    {
                        value = value.Remove(0, 2);
                        value = value.Remove(value.Length - 1, 1);
                    }

                    return value;
                }
            }

            return "";

            //throw new KeyNotFoundException();
        }
    }
}
