using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using Lasting.Bank.Interfaces;
using Lasting.Bank.Models;

namespace Lasting.Bank.Notifier
{
    public class Notifier
    {
        private readonly ISubscriberRepository _subscriberRepository;
        public Notifier(ISubscriberRepository subscriberRepository)
        {
            _subscriberRepository = subscriberRepository;
        }
        public void Notify(Client client)
        {
            var subscription = _subscriberRepository.GetAll().FirstOrDefault(s => s.IdentityNumber == client.IdentityNumber);
            if (subscription != null)
            {
                var request = (HttpWebRequest)WebRequest.Create(subscription.CallbackUri);
                request.ContentType = "application/json";
                request.Method = "POST";

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(client);

                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    // Do somehting here...
                }
            }
        }
    }
}