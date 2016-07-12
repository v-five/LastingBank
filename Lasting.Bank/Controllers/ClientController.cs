using Lasting.Bank.CustomExceptions;
using Lasting.Bank.DB;
using Lasting.Bank.Interfaces;
using Lasting.Bank.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Lasting.Bank.Controllers
{
    [RoutePrefix("api/clients")]
    public class ClientController : ApiController
    {
        private readonly IClientRepository _clientRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ISubscriberRepository _subscriberRepository;
        private readonly Notifier.Notifier _notifier;

        public ClientController(IClientRepository clientRepository, IAccountRepository accountRepository, ISubscriberRepository subscriberRepository)
        {
            _clientRepository = clientRepository;
            _accountRepository = accountRepository;
            _subscriberRepository = subscriberRepository;
            _notifier = new Notifier.Notifier(_subscriberRepository);
        }

        // PUT: api/clients
        [HttpPut]
        [Route("")]
        public HttpResponseMessage AddClient(Client client)
        {
            //TODO: Validation

            try
            {
                return Request.CreateResponse(HttpStatusCode.Created, _clientRepository.Add(client));
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.PreconditionFailed, e.Message);
            }
        }

        [HttpGet]
        [Route("{id}")]
        public HttpResponseMessage GetClient(int id)
        {
            try
            {
                var client = _clientRepository.Get(id);
                if(client == null)
                    throw new ClientNotFoundException();

                return Request.CreateResponse(HttpStatusCode.OK, client);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.PreconditionFailed, e.Message);
            }
        }

        [HttpGet]
        [Route("")]
        public HttpResponseMessage GetAllClients()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _clientRepository.GetAll());
        }

        [HttpDelete]
        [Route("{id}")]
        public HttpResponseMessage DeleteClient(int id)
        {
            try
            {
                _clientRepository.Delete(id);
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.PreconditionFailed, e.Message);
            }
        }

        [HttpGet]
        [Route("{id}/accounts/{accountId}")]
        public HttpResponseMessage GetClientAccount(int id, int accountId)
        {
            try
            {
                return Request.CreateResponse(
                HttpStatusCode.OK,
                _clientRepository.Get(id).Accounts
                    .FirstOrDefault(a => a.ID.Equals(accountId))
            );
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.PreconditionFailed, e.Message);
            }
        }

        [HttpGet]
        [Route("{id}/accounts")]
        public HttpResponseMessage GetClientAllAccounts(int id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _clientRepository.Get(id).Accounts);
        }

        [HttpPut]
        [Route("{id}/accounts")]
        public HttpResponseMessage AddClientAccount(int id, Account account)
        {
            //TODO: Validate

            try
            {
                return Request.CreateResponse(HttpStatusCode.Created, _clientRepository.AddAccount(id, account));
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.PreconditionFailed, e.Message);
            }
        }

        [HttpDelete]
        [Route("{id}/accounts/{accountId}")]
        public HttpResponseMessage CloseClientAccount(int id, int accountId)
        {
            try
            {
                _clientRepository.RemoveAccount(id, accountId);
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.PreconditionFailed, e.Message);
            }
        }

        [HttpPost]
        [Route("{id}/accounts/{accountId}/deposit")]
        public HttpResponseMessage DepositToClientAccount(int id, int accountId, Account account)
        {
            try
            {
                var client = _clientRepository.Get(id);

                if (client == null)
                    throw new ClientNotFoundException();

                var balance = _accountRepository.Deposit(accountId, account.Balance);
                _notifier.Notify(client);

                return Request.CreateResponse(HttpStatusCode.OK, balance);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.PreconditionFailed, e.Message);
            }
        }

        [HttpPost]
        [Route("{id}/accounts/{accountId}/withdraw")]
        public HttpResponseMessage WithdrawFromClientAccount(int id, int accountId, Account account)
        {
            try
            {
                var client = _clientRepository.Get(id);

                if (client == null)
                    throw new ClientNotFoundException();

                var balance = _accountRepository.Withdraw(accountId, account.Balance);
                _notifier.Notify(client);

                return Request.CreateResponse(HttpStatusCode.OK, balance);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.PreconditionFailed, e.Message);
            }
        }
    }
}
