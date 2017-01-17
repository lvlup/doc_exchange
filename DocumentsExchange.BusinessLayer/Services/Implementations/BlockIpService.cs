using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Caching;
using System.Web.Hosting;
using DocumentsExchange.BusinessLayer.Services.Interfaces;

namespace DocumentsExchange.BusinessLayer.Services.Implementations
{
    public class FileContents
    {
        public string[] Ipv4Masks = new string[0];
        public string[] Ipv6Masks = new string[0];
    }

    public class BlockIpService : IBlockIpService
    {
        // Define constants
        private const string FilePath = "~/App_Data/bannedIp.txt";
        private const double CACHE_EXPIRATION = 30.0; // Seconds

        // Define member variables
        private FileContents _fileContents;


        public BlockIpService()
        {
            _fileContents = ReadBannedIpListFile();
        }


        public bool IsIpBlocked(string ip)
        {
            IPAddress ipAddress;

            if (IPAddress.TryParse(ip, out ipAddress))
            {

                if (ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    // IPv4 address
                    string[] ipParts = ip.Split('.');

                    foreach (string banned in _fileContents.Ipv4Masks)
                    {
                        string[] blockedParts = banned.Split('.');
                        if (blockedParts.Length > 4) continue; // Not valid IP mask.

                        if (IsIpBlocked(ipParts, blockedParts))
                        {
                            return true;
                        }
                    }
                }
                else if (ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                {
                    // IPv6 address
                    string[] ipParts = ExpandIpv6Address(ipAddress).Split(':');

                    foreach (string banned in _fileContents.Ipv6Masks)
                    {
                        string bannedIP = banned.Split('/')[0]; // Take IP address part.
                        string[] blockedParts = bannedIP.Split(':');
                        if (blockedParts.Length > 8) continue; // Not valid IP mask.

                        if (IsIpBlocked(ipParts, blockedParts))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }


        private bool IsIpBlocked(string[] ipParts, string[] blockedIpParts)
        {
            for (int i = 0; i < blockedIpParts.Length; i++)
            {
                // Compare if not wildcard
                if (blockedIpParts[i] != "*")
                {
                    // Compare IP address part
                    if (ipParts[i] != blockedIpParts[i].ToLower())
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private string ExpandIpv6Address(IPAddress ipAddress)
        {
            string expanded = "", separator = "";
            byte[] bytes = ipAddress.GetAddressBytes();

            for (int i = 0; i < bytes.Length; i += 2)
            {
                expanded += separator + bytes[i].ToString("x2");
                expanded += bytes[i + 1].ToString("x2");
                separator = ":";
            }

            return expanded;
        }

        private FileContents ReadBannedIpListFile()
        {

            ObjectCache cache = MemoryCache.Default;
            FileContents fileContents = cache["filecontents"] as FileContents;

            if (fileContents == null)
            {
                FileContents tempFileContents = new FileContents();

                string cachedFilePath = HostingEnvironment.MapPath(FilePath);
                if (File.Exists(cachedFilePath))
                {
                    List<string> filePaths = new List<string>();
                    filePaths.Add(cachedFilePath);

                    CacheItemPolicy policy = new CacheItemPolicy();
                    policy.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(CACHE_EXPIRATION);
                    policy.ChangeMonitors.Add(new HostFileChangeMonitor(filePaths));

                    List<string> tempIpv4List = new List<string>();
                    List<string> tempIpv6List = new List<string>();

                    // Read the file line by line.
                    using (StreamReader file = new StreamReader(cachedFilePath))
                    {
                        string line;
                        while ((line = file.ReadLine()) != null)
                        {
                            if (line.Contains("."))
                            {
                                tempIpv4List.Add(line);
                            }
                            else if (line.Contains(":"))
                            {
                                tempIpv6List.Add(line);
                            }
                        }
                    }

                    tempFileContents.Ipv4Masks = tempIpv4List.ToArray();
                    tempFileContents.Ipv6Masks = tempIpv6List.ToArray();

                    cache.Set("filecontents", tempFileContents, policy);
                }

                fileContents = tempFileContents;
            }

            return fileContents;
        }
    }
}
