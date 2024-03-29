using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace FilterCore.Constants
{
    public static class BaseTypeDataProvider
    {
        private const string DataFileUrl = "https://www.filterblade.xyz/datafiles/other/BasetypeStorage.csv";
        private static Dictionary<string, Dictionary<string, string>> data;
        public static Dictionary<string, Dictionary<string, string>> BaseTypeData => data ?? (data = LoadData());

        private static Dictionary<string, Dictionary<string, string>> LoadData()
        {
            // this client somehow stopped working. on one hand it suddenly needed a user agent specified, on the other it
            // crashed because of endless redirects.
            //            var client = new WebClient();
            //            client.Headers.Add("user-agent", "any");  
            //            var fullString = client.DownloadString(DataFileUrl);

            var client = (HttpWebRequest)WebRequest.Create(DataFileUrl);
            client.UserAgent = "any";
            client.CookieContainer = new CookieContainer();

            var fullString = "";
            var reader = new StreamReader(client.GetResponse().GetResponseStream());
            while (!reader.EndOfStream)
            {
                fullString += (reader.ReadLine()) + "\n";
            }

            string[] stats = null;
            var baseTypeData = new Dictionary<string, Dictionary<string, string>>();

            var isFirstLine = true;
            foreach (var line in fullString.Split('\n'))
            {
                if (string.IsNullOrEmpty(line)) continue;

                var words = line.Split(',');

                if (isFirstLine)
                {
                    stats = words;
                    isFirstLine = false;
                    continue;
                }

                string baseType = null;
                var statDic = new Dictionary<string, string>();
                for (var i = 0; i < words.Length; i++)
                {
                    var value = words[i];
                    var stat = stats[i];

                    if (stat == "BaseType") baseType = value;

                    statDic.Add(stat, value);
                }

                baseTypeData.Add(baseType, statDic);
            }

            return baseTypeData;
        }
    }
}