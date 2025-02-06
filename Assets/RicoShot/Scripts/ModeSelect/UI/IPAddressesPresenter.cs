using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using TMPro;
using UnityEngine;

namespace RicoShot.ModeSelect.UI
{
    public class IPAddressesPresenter : MonoBehaviour
    {
        [SerializeField] private TMP_Text ipAddressesText;

        private void Start()
        {
            var ipAddresses = GetIPAddresses();
            ipAddressesText.text = string.Join("\n", ipAddresses);
        }

        private ICollection<string> GetIPAddresses()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            var ipAddresses = new List<string>();
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetworkV6) continue;
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    ipAddresses.Add(ip.ToString());
            }

            return ipAddresses;
        }
    }
}