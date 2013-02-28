﻿using BalancedSharp.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BalancedSharp
{
    public interface IBalancedService
    {
        IAccountClient Account { get; }

        IBankAccountClient Bank { get; }
        
        ICardClient Card { get; }
        
        ICreditClient Credit { get; }
        
        IDebitClient Debit { get; }
        
        IEventClient Event { get; }
        
        IHoldClient Hold { get; }
        
        IRefundClient Refund { get; }
        
        IVerificationClient Verification { get; }

        string BaseUri { get; }

        string Key { get; }
    }

    public class BalancedService : IBalancedService
    {
        IAccountClient accountClient;
        IBankAccountClient bankClient;
        ICardClient cardClient;
        IHoldClient holdClient;
        ICreditClient creditClient;
        IDebitClient debitClient;
        IRefundClient refundClient;
        IEventClient eventClient;
        IVerificationClient verificationClient;
        string key;

        public BalancedService(string key)
        {
            IBalancedRest rest = new HttpWebRequestRest(new DcJsonBalancedSerializer());
            this.accountClient = new AccountClient(this, rest);
            this.bankClient = new BankAccountClient(this);
            this.cardClient = new CardClient(this);
            this.holdClient = new HoldClient(this);
            this.creditClient = new CreditClient(this);
            this.debitClient = new DebitClient(this);
            this.refundClient = new RefundClient(this);
            this.eventClient = new EventClient(this);
            this.verificationClient = new VerificationClient(this);
            this.key = key;
        }

        public IAccountClient Account
        {
            get { return this.accountClient; }
        }

        public IBankAccountClient Bank
        {
            get { return this.bankClient; }
        }

        public ICardClient Card
        {
            get { return this.cardClient; }
        }

        public IHoldClient Hold
        {
            get { return this.holdClient; }
        }

        public ICreditClient Credit
        {
            get { return this.creditClient; }
        }

        public IDebitClient Debit
        {
            get { return this.debitClient; }
        }

        public IRefundClient Refund
        {
            get { return this.refundClient; }
        }

        public IEventClient Event
        {
            get { return this.eventClient; }
        }

        public IVerificationClient Verification
        {
            get { return this.verificationClient; }
        }

        public string BaseUri
        {
            get { return "https://api.balancedpayments.com/v1/"; }
        }

        public string Key
        {
            get { return this.key; }
        }
    }
}