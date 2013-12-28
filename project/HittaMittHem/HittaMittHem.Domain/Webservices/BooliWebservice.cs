using HittaMittHem.Domain.Entities.Booli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

namespace HittaMittHem.Domain.Webservices
{
    public class BooliWebservice
    {

        public List<Ad> Search(string searchTerms)
        {
            var rawJson = string.Empty;

            var callerId = ConfigurationManager.AppSettings["BooliCallerId"];
            var privateKey = ConfigurationManager.AppSettings["BooliPrivateKey"];
            var unixTimestamp = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
            var uniqueString = this.GetRandomString();
            var hash = this.GetSha1Hash(callerId, privateKey, unixTimestamp, uniqueString);

            var authString = String.Format("&callerId={0}&time={1}&unique={2}&hash={3}",
                callerId, unixTimestamp, uniqueString, hash);

            var requestUriString = String.Format("http://api.booli.se/listings?q={0}{1}", 
                searchTerms, authString);
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
            }
            catch (WebException e)
            {
                return null;
                
            }
            //var json = JObject.Parse(rawJson);
            return JObject.Parse(rawJson)["listings"].Select(a => new Ad(a)).ToList();
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

            return randChars.ToString();
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
