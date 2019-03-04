using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Leopard.Common
{
    public class Utilities
    {
        public static IPAddress ResolveIPAddress(string addrOrHost, byte[] subnet, AddressFamily family)
        {
            var loopback = family == AddressFamily.InterNetwork ? IPAddress.Loopback : IPAddress.IPv6Loopback;
            IList<IPAddress> nodeIps;

            // if the address is an empty string, just enumerate all ip addresses available
            // on this node
            if (string.IsNullOrEmpty(addrOrHost))
            {
                nodeIps = NetworkInterface.GetAllNetworkInterfaces()
                            .Where(iface => iface.OperationalStatus == OperationalStatus.Up)
                            .SelectMany(iface => iface.GetIPProperties().UnicastAddresses)
                            .Select(addr => addr.Address)
                            .Where(addr => addr.AddressFamily == family && !IPAddress.IsLoopback(addr))
                            .ToList();
            }
            else
            {
                // Fix StreamFilteringTests_SMS tests
                if (addrOrHost.Equals("loopback", StringComparison.OrdinalIgnoreCase))
                {
                    return loopback;
                }

                // check if addrOrHost is a valid IP address including loopback (127.0.0.0/8, ::1) and any (0.0.0.0/0, ::) addresses
                IPAddress address;
                if (IPAddress.TryParse(addrOrHost, out address))
                {
                    return address;
                }

                // Get IP address from DNS. If addrOrHost is localhost will 
                // return loopback IPv4 address (or IPv4 and IPv6 addresses if OS is supported IPv6)
                nodeIps =  Dns.GetHostAddresses(addrOrHost);
            }

            var candidates = new List<IPAddress>();
            foreach (var nodeIp in nodeIps.Where(x => x.AddressFamily == family))
            {
                // If the subnet does not match - we can't resolve this address.
                // If subnet is not specified - pick smallest address deterministically.
                if (subnet == null)
                {
                    candidates.Add(nodeIp);
                }
                else
                {
                    var ip = nodeIp;
                    if (subnet.Select((b, i) => ip.GetAddressBytes()[i] == b).All(x => x))
                    {
                        candidates.Add(nodeIp);
                    }
                }
            }
            if (candidates.Count > 0)
            {
                return PickIPAddress(candidates);
            }


            throw new ArgumentException("Hostname '" + addrOrHost + "' with subnet and family " + family + " is not a valid IP address or DNS name");
        }

        internal static IPAddress PickIPAddress(IReadOnlyList<IPAddress> candidates)
        {
            IPAddress chosen = null;
            foreach (IPAddress addr in candidates)
            {
                if (chosen == null)
                {
                    chosen = addr;
                }
                else
                {
                    if (CompareIPAddresses(addr, chosen)) // pick smallest address deterministically
                        chosen = addr;
                }
            }
            return chosen;
        }

        private static bool CompareIPAddresses(IPAddress lhs, IPAddress rhs)
        {
            byte[] lbytes = lhs.GetAddressBytes();
            byte[] rbytes = rhs.GetAddressBytes();

            if (lbytes.Length != rbytes.Length) return lbytes.Length < rbytes.Length;

            // compare starting from most significant octet.
            // 10.68.20.21 < 10.98.05.04
            for (int i = 0; i < lbytes.Length; i++)
            {
                if (lbytes[i] != rbytes[i])
                {
                    return lbytes[i] < rbytes[i];
                }
            }
            // They're equal
            return false;
        }
    }


}
