using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace avito.Models
{
    public interface IPayer
    {
        Response Pay();
    }

    public class SfpPayer : IPayer
    {
        public Response Pay()
        {
            Console.WriteLine("Произведена оплата с помощью СБП");
            return Response.Ok();
        }
    }

    public class WebMoneyPayer : IPayer
    {
        public Response Pay()
        {
            Console.WriteLine("Произведена оплата с помощью эл. кошелька");
            return Response.Ok();
        }
    }

    public class DebitCardPayer : IPayer
    {
        public Response Pay()
        {
            Console.WriteLine("Произведена оплата с помощью карты");
            return Response.Ok();
        }
    }

    public class CashPayer : IPayer
    {
        public Response Pay()
        {
            Console.WriteLine("Выбрана оплата наличными при встрече");
            return Response.Ok();
        }
    }
}
