using avito.Models;
using Avito;
using Avito.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace avito
{
    public class AvitoService
    {
        private readonly IAvitoFactory _factory;
        private readonly IAvitoRepository _repository;
        public AvitoService()
        {
            var services = new ServiceCollection();
            services.AddTransient<IAvitoRepository, AvitoRepository>();
            services.AddTransient<IAvitoFactory, CashFactory>();
            var provider = services.BuildServiceProvider();

            _factory = provider.GetService<IAvitoFactory>();
            _repository = provider.GetService<IAvitoRepository>();
        }

        public (Response, User) Login(string username, string password)
        {
            var user = _repository.FindUser(username);
            //для простоты опустим момент хэширования пароля
            if (user == null || user.Password != password)
            {
                return (Response.Error("Неверное имя пользователя или пароль"), null);
            }

            return (Response.Ok(), user);
        }

        public Response Register(User user)
        {
            var userExist = _repository.UserExist(user.UserName);

            if (userExist)
            {
                return Response.Error("Пользователь с таким username уже существует");
            }

            return _repository.SaveUser(user);
        }

        public List<string> GetAdsInfoList()
        {
            var ads = _repository.GetAllAdvertisement();
            
            return ads.Select(x => $"{x.Product.Name}, {x.Product.Description}, {x.Price}p. {x.PaymentType},  {x.User.UserName}").ToList();
        }
        public List<Advertisement> GetInfoAdsList()
        {
            var ads = _repository.GetAllAdvertisement();

            return ads;
        }

        public List<Advertisement> GetAdsInfoListAd()
        {
            var ads = _repository.GetAllAdvertisement();

            return ads;

        }
        public List<Advertisement> GetUserAdsList(User user)
        {
            var ads = _repository.GetUserAdvertisements(user.UserName);
            return ads;
        }
        public List<string> GetFindAdsList(string name)
        {
            var ads = _repository.GetAllAdvertisement().FindAll(x => x.Product.Name == name);

            return ads.Select(x => $"{x.Product.Name}, {x.Product.Description}, {x.Price}p. {x.PaymentType},  {x.User.UserName}").ToList();

        }
        public List<string> GetUserAdsInfoList(List<Advertisement> ads)
        {
           return ads.Select(x => $"{x.Product.Name}, {x.Product.Description}, {x.Price}p. {x.PaymentType},  {x.User.UserName}").ToList();
        }

        public Response SaveAd(Advertisement ad)
        {
            return _repository.SaveAdvertisement(ad);
        }

        public Response DeleteAd(Advertisement ad)
        {
            return _repository.DeleteAdvertisement(ad);
        }
    }
}
