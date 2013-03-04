﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BalancedSharp.Clients
{
    /// <summary>
    /// Before you can debit a bank account you need to verify that you have access to it. 
    /// Balanced allows you to do this by creating a Verification for a Bank Account
    /// which will result in two random amounts being credited into the bank account. 
    /// The amounts of these two deposits must be sent back as the amount_1 and amount_2 params
    /// when subsequently updating this verification. 
    /// These deposits will appear on the bank accounts statement as Balanced verification.
    /// If you fail to verify the bank account you must create a new verification and begin 
    /// the process again. You can only create one verification at a time 
    /// and the trial deposits should show in the bank account within 2 business days.
    /// </summary>
    public interface IVerificationClient
    {
        /// <summary>
        /// Creates a new bank account verification.
        /// </summary>
        /// <param name="bankAccountId">The bank account id.</param>
        /// <returns>Verification details</returns>
        Status<Verification> Create(string bankAccountId);

        /// <summary>
        /// Gets verification information for a bank account.
        /// </summary>
        /// <param name="bankAccountId">The bank account id.</param>
        /// <param name="verificationId">The associated verification id.</param>
        /// <returns>Verification details</returns>
        Status<Verification> Get(string bankAccountId, string verificationId);

        /// <summary>
        /// Gets a list of all verification information for a bank account.
        /// </summary>
        /// <param name="bankAccountId">The bank account id.</param>
        /// <param name="verificationId">The associated verification id.</param>
        /// <param name="limit">The limit.</param>
        /// <param name="offset">The offset.</param>
        /// <returns>A list of verification details</returns>
        Status<PagedList<Verification>> List(string bankAccountId, int limit = 10, int offset = 0);

        /// <summary>
        /// Confirms the trial deposit amounts. For the test environment the trial deposit amounts are always 1 and 1.
        /// </summary>
        /// <param name="bankAccountId">The bank account id.</param>
        /// <param name="verificationId">The associated verification id.</param>
        /// <param name="amount1">The first tiral deposit amount.</param>
        /// <param name="amount2">The second trial deposit amount.</param>
        /// <returns>Verification details</returns>
        Status<Verification> Confirm(string bankAccountId, string verificationId, int amount1, int amount2);
    }

    public class VerificationClient : IVerificationClient
    {
        IBalancedService balanceService;
        IBalancedRest rest;

        public VerificationClient(IBalancedService balanceService, IBalancedRest rest)
        {
            this.balanceService = balanceService;
            this.rest = rest;
        }

        public Status<Verification> Create(string bankAccountId)
        {
            string url = string.Format("{0}{1}/bank_accounts/{2}/verifications",
                this.balanceService.BaseUri, this.balanceService.MarketplaceUrl, bankAccountId);
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("none", "");

            return rest.GetResult<Verification>(url, this.balanceService.Key, "", "post", parameters);
        }

        public Status<Verification> Get(string bankAccountId, string verificationId)
        {
            string url = string.Format("{0}{1}/bank_accounts/{2}/verifications/{3}",
                this.balanceService.BaseUri, this.balanceService.MarketplaceUrl, bankAccountId, verificationId);
            
            return rest.GetResult<Verification>(url, this.balanceService.Key, "", "get", null);
        }

        public Status<PagedList<Verification>> List(string bankAccountId, int limit = 10, int offset = 0)
        {
            string url = string.Format("{0}{1}/bank_accounts/{2}/verifications",
               this.balanceService.BaseUri, this.balanceService.MarketplaceUrl, bankAccountId);

            return rest.GetResult<PagedList<Verification>>(url, this.balanceService.Key, "", "get", null);
        }

        public Status<Verification> Confirm(string bankAccountId, string verificationId, int amount1, int amount2)
        {
            string url = string.Format("{0}{1}/bank_accounts/{2}/verifications/{3}",
                this.balanceService.BaseUri, this.balanceService.MarketplaceUrl, bankAccountId, verificationId);
            
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("amount_1", "");
            parameters.Add("amount_2", "");

            return rest.GetResult<Verification>(url, this.balanceService.Key, "", "put", parameters);
        }
    }
}
