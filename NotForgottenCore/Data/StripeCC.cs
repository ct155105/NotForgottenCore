using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stripe;

namespace NotForgottenCore.Data
{
    public class StripeCC
    {

        public ChargeCreateOptions Options { get; set; }
        public ChargeService Service { get; set; }
        public Charge Charge { get; set; }

        public StripeCC(int amount, string token)
        {

            Service = new ChargeService();
            Options = new ChargeCreateOptions
            {
                Amount = amount,
                Currency = "usd",
                SourceId = token
                //ReceiptEmail = receiptEmail
            };
            Charge = Service.Create(Options);
        }
    }
}
