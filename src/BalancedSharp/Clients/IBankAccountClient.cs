﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BalancedSharp.Clients
{
    public interface IBankAccountClient : IBalancedServiceObject
    {
        Status<BankAccount> Save(BankAccount bankAccount);

        /// <summary>
        /// You'll eventually want to be able to credit bank accounts 
        /// without having to ask your users for their information 
        /// over and over again. To do this, you'll need to create a bank account object.
        /// </summary>
        /// <param name="name">Name on the bank account. Length must be greater than or equal to 2.</param>
        /// <param name="accountNumber">The bank account number. Length must be greater than or equal to 1.</param>
        /// <param name="routingNumber">The bank routing number. Length must be equal to 9.</param>
        /// <param name="type">The type of bank account.</param>
        /// <param name="meta">Single level mapping from string keys to string values.</param>
        /// <returns>BankAccount details</returns>
        BankAccount New(string name, string accountNumber, string routingNumber,
            BankAccountType type, Dictionary<string, string> meta = null);

        /// <summary>
        /// Retrieves the details of a bank account that has previously been created. 
        /// The same information is returned when creating the bank account.
        /// </summary>
        /// <param name="bankAccountId">The bank account id.</param>
        /// <returns>BankAccount details</returns>
        Status<BankAccount> Get(string bankAccountUri);

        /// <summary>
        /// Returns a list of bank accounts that you've created but haven't deleted.
        /// </summary>
        /// <param name="limit">The limit.</param>
        /// <param name="offset">The offset.</param>
        /// <returns>PagedList of BankAccount details</returns>
        Status<PagedList<BankAccount>> List(int limit = 10, int offset = 0);

        /// <summary>
        /// Permanently deletes a bank account.
        /// This cannot be undone.
        /// All associated credits with a deleted bank
        /// account will not be affected.
        /// </summary>
        /// <param name="bankAccountId">The bank account id.</param>
        /// <returns></returns>
        Status Delete(string bankAccountUri);
    }

    public class BankAccountClient : IBankAccountClient
    {
        IBalancedRest rest;

        public BankAccountClient(IBalancedService balanceService, IBalancedRest rest)
        {
            this.Service = balanceService;
            this.rest = rest;
        }

        public BankAccount New(string name, string accountNumber, string routingNumber, 
            BankAccountType type, Dictionary<string, string> meta = null)
        {
            return new BankAccount
            {
                Name = name,
                AccountNumber = accountNumber,
                RoutingNumber = routingNumber,
                Type = type,
                Meta = meta,
                Service = this.Service
            };
        }

        public Status<BankAccount> Save(BankAccount bankAccount)
        {
            string url = string.Format("{0}/v1/bank_accounts", this.Service.BaseUri);
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("name", bankAccount.Name);
            parameters.Add("account_number", bankAccount.AccountNumber);
            parameters.Add("routing_number", bankAccount.RoutingNumber);
            parameters.Add("type", bankAccount.Type.ToString().ToLower());
            return this.rest.GetResult<BankAccount>(url, this.Service.Key, null, "post", parameters);
        }

        public Status<BankAccount> Get(string bankAccountUri)
        {
            string url = string.Format("{0}{1}", this.Service.BaseUri, bankAccountUri);
            return this.rest.GetResult<BankAccount>(url, this.Service.Key, null, "get", null);
        }

        public Status<PagedList<BankAccount>> List(int limit = 10, int offset = 0)
        {
            string url = string.Format("{0}/v1/bank_accounts", this.Service.BaseUri);
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("limit", limit.ToString());
            parameters.Add("offset", offset.ToString());
            return this.rest.GetResult<PagedList<BankAccount>>(url, this.Service.Key, null, "get", parameters);
        }

        public Status Delete(string bankAccountUri)
        {
            string url = string.Format("{0}{1}",
                this.Service.BaseUri, bankAccountUri);
            return this.rest.GetResult(url, this.Service.Key, null, "delete", null);
        }

        public IBalancedService Service
        {
            get;
            set;
        }
    }
}
