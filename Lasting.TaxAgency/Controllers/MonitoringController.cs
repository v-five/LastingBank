using Lasting.TaxAgency.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Web.Http;

namespace Lasting.TaxAgency.Controllers
{
    [RoutePrefix("api/monitoring")]
    public class MonitoringController : ApiController
    {
        private const string SubscribeUrl = "http://localhost:2348/api/notifications/subscribe";
        private const string UnsubscribeUrl = "http://localhost:2348/api/notifications/unsubscribe/";
        private const string CallbackUrl = "http://localhost:13951/api/monitoring";
        public static readonly ConcurrentDictionary<int, Client> ClientsData = new ConcurrentDictionary<int, Client>();

        [HttpPost]
        [Route("")]
        public HttpResponseMessage PostNotification(Client client)
        {
            var diffs = new List<string>();
            
            ClientsData.AddOrUpdate(client.IdentityNumber, client, (key, value) => {
                foreach(var account in client.Accounts)
                {
                    var acc = value.Accounts.FirstOrDefault(a => a.ID.Equals(account.ID));
                    if(acc == null)
                    {
                        diffs.Add("Created new account with id: " + account.ID);
                    }
                    else if(acc.Balance < account.Balance)
                    {
                        diffs.Add($"{account.Balance - acc.Balance} {acc.Currency} deposited to the account with id: {acc.ID}");
                    }
                    else if (acc.Balance > account.Balance)
                    {
                        diffs.Add($"{acc.Balance - account.Balance} {acc.Currency} withdrawed from the account with id: {acc.ID}");
                    }
                }

                client.ModifiedDate = DateTime.Now;
                return client;
            });

            return Request.CreateResponse(HttpStatusCode.OK, diffs);
        }
        
        [HttpPut]
        [Route("{identityNumber}")]
        public HttpResponseMessage SubscribeForClient(int identityNumber)
        {
            var request = (HttpWebRequest)WebRequest.Create(SubscribeUrl);
            request.ContentType = "application/json";
            request.Method = "PUT";
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    CallbackUri = CallbackUrl,
                    IdentityNumber = identityNumber
                });

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    return Request.CreateResponse(HttpStatusCode.NoContent);
                }

                var rs = response.GetResponseStream();
                if (rs == null) return Request.CreateResponse(response.StatusCode);

                var streamReader = new StreamReader(rs, true);
                return Request.CreateResponse(response.StatusCode, streamReader.ReadToEnd());
            }
        }

        [HttpDelete]
        [Route("{identityNumber}")]
        public HttpResponseMessage UnsubscribeForClient(int identityNumber)
        {
            var request = (HttpWebRequest)WebRequest.Create(UnsubscribeUrl + identityNumber);
            request.ContentType = "application/json";
            request.Method = "DELETE";
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    return Request.CreateResponse(HttpStatusCode.NoContent);
                }

                var rs = response.GetResponseStream();
                if (rs == null) return Request.CreateResponse(response.StatusCode);

                var streamReader = new StreamReader(rs, true);
                return Request.CreateResponse(response.StatusCode, streamReader.ReadToEnd());
            }
        }
    }
}
