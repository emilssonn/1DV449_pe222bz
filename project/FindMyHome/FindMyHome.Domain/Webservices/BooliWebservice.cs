using FindMyHome.Domain.Entities.Booli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

namespace FindMyHome.Domain.Webservices
{
    internal class BooliWebservice
    {

        public AdsContainer SearchRaw(string searchTerms, string objectTypes = null, int? offset = 0, int? limit = 500)
        {
            StringBuilder filters = new StringBuilder();
            if (objectTypes != null)
            {
                filters.Append("&objectType=");
                filters.Append(objectTypes);
            }

            return this.Search(searchTerms, filters.ToString(), offset, limit);
        }

        public AdsContainer Search(string searchTerms, string filters, int? offset, int? limit)
        {
            var rawJson = string.Empty;
            var callerId = ConfigurationManager.AppSettings["BooliCallerId"];
            var privateKey = ConfigurationManager.AppSettings["BooliPrivateKey"];

            var unixTimestamp = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
            var uniqueString = this.GetRandomString();
            var hash = this.GetSha1Hash(callerId, privateKey, unixTimestamp, uniqueString);
            var authString = String.Format("&callerId={0}&time={1}&unique={2}&hash={3}",
                callerId, unixTimestamp, uniqueString, hash);

            var searchString = String.Format("offset={0}&limit={1}{2}&q={3}",
                offset, limit, filters, searchTerms);

            var requestUriString = String.Format("http://api.booli.se/listings?{0}{1}",
                searchString, authString);
            var request = (HttpWebRequest)WebRequest.Create(requestUriString);
            request.Method = "GET";
            request.Accept = "application/json";

            try
            {
                using (var response = request.GetResponse())
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    rawJson = reader.ReadToEnd();
                }
                var jsonRes = JObject.Parse(rawJson);

                var adsContainer = new AdsContainer();

                adsContainer.Uri = searchString;
                adsContainer.Ads.AddRange(jsonRes["listings"].Select(a => new Ad(a)).ToList());
                adsContainer.CurrentCount = (int)jsonRes["count"];
                adsContainer.TotalCount = (int)jsonRes["totalCount"];
                adsContainer.Limit = (int)jsonRes["limit"];
                adsContainer.Offset = (int)jsonRes["offset"];

                return adsContainer;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private string GetRandomString(int stringlength = 16)
        {
            var chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            var randChars = new char[stringlength];
            var rand = new Random();

            for (int i = 0; i < stringlength; i++)
            {
                randChars[i] = chars[rand.Next(chars.Length)];
            }

            return new string(randChars);
        }

        private string GetSha1Hash(string callerId, string privateKey, int unixTimestamp, string uniqueString)
        {
            var stringToHash = String.Format("{0}{1}{2}{3}", callerId, unixTimestamp, privateKey, uniqueString);
            var bytesHash = Encoding.UTF8.GetBytes(stringToHash);
            var sha1 = new SHA1CryptoServiceProvider();
            var hash = sha1.ComputeHash(bytesHash);

            return BitConverter.ToString(hash).Replace("-", "");
        }
    }
}
