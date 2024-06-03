using avito.Models;
using Avito.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace avito
{
    public class AdBuilder
    {
        private User _user;
        private PaymentType? _type;
        private string _productName;
        private string _productDesc;
        private decimal? _price;
        public AdBuilder() { }

        public AdBuilder User(User user)
        {
            _user = user;
            return this;
        }
        public AdBuilder PaymentType(PaymentType type)
        {
            _type = type;
            return this;
        }
        public AdBuilder ProductName(string productName)
        {
            _productName = productName;
            return this;
        }
        public AdBuilder ProductDescription(string productDesc)
        {
            _productDesc = productDesc;
            return this;
        }

        public AdBuilder Price(decimal price)
        {
            _price = price;
            return this;
        }
        public Advertisement Build()
        {
            if (_user == null || _type == null || _productName.IsEmpty() || _price == null )
                throw new ArgumentException("Не все поля заполнены");

            return new Advertisement { PaymentType = _type.Value, Id = Guid.NewGuid(), Price = _price.Value, Product = new Product() { Id = Guid.NewGuid(), Description = _productDesc, Name = _productName }, User = _user };
        }
    }
}
