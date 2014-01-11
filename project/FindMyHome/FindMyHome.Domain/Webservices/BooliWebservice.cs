﻿using FindMyHome.Domain.Entities.Booli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using FindMyHome.Domain.Exceptions;
using FindMyHome.Domain.Helpers;

namespace FindMyHome.Domain.Webservices
{
    internal class BooliWebservice
    {
        public AdsContainer Search(string searchTerms, string objectTypes = null, int maxRent = 0, int maxPrice = 0, int? offset = 0, int? limit = 30)
        {
            StringBuilder filters = new StringBuilder();
            if (objectTypes != null)
            {
                filters.Append("&objectType=");
                filters.Append(objectTypes);
            }

            if (maxRent > 0)
            {
                filters.Append("&maxRent=");
                filters.Append(maxRent);
            }

            if (maxPrice > 0)
            {
                filters.Append("&maxListPrice=");
                filters.Append(maxPrice);
            }

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
			request.UserAgent = "Universitets projekt (Lnu), utveckling, sista användingsdag: 19/1";

            try
            {
                using (var response = request.GetResponse())
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    rawJson = reader.ReadToEnd();
                }
                var jsonRes = JObject.Parse(rawJson);

                var adsContainer = new AdsContainer();

                adsContainer.SearchTerms = searchTerms;
                adsContainer.ObjectTypes = objectTypes;
                adsContainer.MaxPrice = maxPrice;
                adsContainer.MaxRent = maxRent;
                adsContainer.Ads.AddRange(jsonRes["listings"].Select(a => new Ad(a)).ToList());
                adsContainer.CurrentCount = (int)jsonRes["count"];
                adsContainer.TotalCount = (int)jsonRes["totalCount"];
                adsContainer.Limit = (int)jsonRes["limit"];
                adsContainer.Offset = (int)jsonRes["offset"];

                return adsContainer;
            }
            catch (WebException e)
            {
				ExceptionHandler.WebException(e, Properties.Resources.BooliApiErrorSwe);
				throw;
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
