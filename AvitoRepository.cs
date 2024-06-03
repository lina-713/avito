using avito.Models;
using Avito.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Avito
{
    interface IAvitoRepository
    {
        List<User> GetAllUsers();
        User FindUser(string username);
        bool UserExist(string username);
        Response SaveUser(User user);
        List<Advertisement> GetUserAdvertisements(string username);
        List<Advertisement> GetFindAdvertisements(string name);
        List<Advertisement> GetAllAdvertisement();
        Advertisement FindAdvertisement(Guid guid);
        Response SaveAdvertisement(Advertisement product);
        Response DeleteAdvertisement(Advertisement advertisement);
    }

    public class AvitoRepository : IAvitoRepository 
    {
        private readonly DbMockSingleton _dbMockSingleton;
        public AvitoRepository() 
        {
            _dbMockSingleton = DbMockSingleton.Instance;
        }

        public List<User> GetAllUsers()
        {
            return _dbMockSingleton.Users;
        }

        public User FindUser(string username)
        {
            return _dbMockSingleton.Users.Find(x => x.UserName == username);
        }

        public List<Advertisement> GetAllAdvertisement()
        {
            return _dbMockSingleton.Advertisements;
        }

        public Response SaveUser(User user)
        {
            try
            {
                _dbMockSingleton.Users.Add(user);
                return Response.Ok();
            }
            catch (Exception ex)
            {
                return Response.Error(ex.Message);
            }
        }

        public Advertisement FindAdvertisement(Guid guid)
        {
            return _dbMockSingleton.Advertisements.Find(x => x.Id == guid);
        }
        public Advertisement FindUserAdvertisement(Guid guid, string username)
        {

            return _dbMockSingleton.Advertisements.Find(x => x.Id == guid);
        }
        public Response SaveAdvertisement(Advertisement product)
        {
            try
            {
                var oldAd =  _dbMockSingleton.Advertisements.Find(x => x.Id == product.Id);

                if (oldAd != null)
                {
                    oldAd.Price = product.Price;
                    oldAd.Product.Description = product.Product.Description;
                    oldAd.PaymentType = product.PaymentType;
                    oldAd.Product.Name = product.Product.Name;
                }
                else
                {
                    _dbMockSingleton.Advertisements.Add(product);
                }

                return Response.Ok();
            }
            catch (Exception ex)
            {
                return Response.Error(ex.Message);
            }
        }

        public bool UserExist(string username)
        {
            return _dbMockSingleton.Users.Exists(x => x.Name == username);
        }

        public List<Advertisement> GetUserAdvertisements(string username)
        {
            return _dbMockSingleton.Advertisements.FindAll(x => x.User.UserName == username);
        }

        public Response DeleteAdvertisement(Advertisement advertisement)
        {
            var res = _dbMockSingleton.Advertisements.Remove(advertisement);
            if (res)
            {
                return Response.Ok();
            }
            else
            {
                return Response.Error("Объект не был удален");
            }
        }

        public List<Advertisement> GetFindAdvertisements(string name)
        {
            return _dbMockSingleton.Advertisements.FindAll(x => x.Product.Name == name);
        }
    }
}
