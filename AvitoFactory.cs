using avito.Models;
using Avito.Models;
using System;

namespace Avito
{
    interface IAvitoFactory
    {
        IPayer CreatePayer();
    }

    public class CashFactory : IAvitoFactory
    {
        public IPayer CreatePayer()
        {
            return new CashPayer();
               
        }

    }

    public class WebMoneyFactory : IAvitoFactory
    {
        public IPayer CreatePayer()
        {
            return new WebMoneyPayer();

        }
    }
    public class DebitCardFactory : IAvitoFactory
    {
        public IPayer CreatePayer()
        {
            return new DebitCardPayer();

        }
    }
    public class SFPFactory : IAvitoFactory
    {
        public IPayer CreatePayer()
        {
            return new SfpPayer();

        }
    }
}

