using Avito.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace avito.Models
{
    public class Advertisement
    {
        public Guid Id { get; set; }
        public User User { get; set; }
        public PaymentType PaymentType { get; set; }
        public Product Product { get; set; }
        public decimal Price { get; set; }
    }
}
