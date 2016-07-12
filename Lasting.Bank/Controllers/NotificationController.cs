using Lasting.Bank.CustomExceptions;
using Lasting.Bank.DB;
using Lasting.Bank.Interfaces;
using Lasting.Bank.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Lasting.Bank.Controllers
{
    [RoutePrefix("api/notifications")]
    public class NotificationController : ApiController
    {
        private readonly ISubscriberRepository _subscriberRepository;

        public NotificationController(ISubscriberRepository subscriberRepository)
        {
            _subscriberRepository = subscriberRepository;
        }

        [HttpPut]
        [Route("subscribe")]
        public HttpResponseMessage SubscribeForClient(Subscriber subscriber)
        {
            return Request.CreateResponse(HttpStatusCode.Created, _subscriberRepository.Add(subscriber));
        }

        [HttpDelete]
        [Route("unsubscribe/{identityNumber}")]
        public HttpResponseMessage UnSubscribeForClient(int identityNumber)
        {
            _subscriberRepository.Delete(identityNumber);
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        [HttpGet]
        [Route("subscribers")]
        public HttpResponseMessage GetAllSubscribers()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _subscriberRepository.GetAll());
        }
    }
}
