using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WifiHotspotDotNet
{
    public class PassphraseLengthException : Exception
    {

    }

    public class SSIDLengthException : Exception
    {

    }

    public class HostedNetworkNotStartedException : Exception
    {

    }

    public class HostedNetworkNotStoppedException : Exception
    {

    }

    public class HostedNetworkModeNotSetException : Exception
    {

    }

    public class HostedNetworkSSIDNotSetException : Exception
    {

    }

    class HostedNetworkPassphraseNotSetException : Exception
    {

    }
}
