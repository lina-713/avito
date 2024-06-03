using avito;
using avito.Models;
using Avito.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;

namespace Avito
{
    public class Avito
    {
        private static AvitoService _service;
        public static User CurrentUser { get; set; }
        static void Main(string[] args)
        {
            _service = new AvitoService();
            Console.WriteLine("...Вызов метода Auth() на регистрацию/вход/вход с учетки гостя...\n");
            Auth();

            Console.ReadKey();
        }
        
        private static void Menu()
        {
            var exit = false;
            while (true)
            {
                Console.WriteLine("Нажмите 1 чтобы посмотреть список объявлений, 2 чтобы разместить объявление, 3 чтобы просмотреть свои объявления, 4 чтобы найти товар, 5 чтобы купить товар, 6 чтобы выйти в начало");
                var key = Console.ReadKey();
                Console.WriteLine();
                Console.WriteLine("...Получение запроса от пользователя...\n");
                switch (key.Key)
                {
                    case ConsoleKey.D1:
                        Console.WriteLine("...Вызов метода ShowAdsList() на просмотр всех объявлений...\n");
                        ShowAdsList();
                        break;
                    case ConsoleKey.D2:
                        Console.WriteLine("...Вызов метода PlaceNewAd() на создание объявления...\n");
                        PlaceNewAd(); 
                        break;
                    case ConsoleKey.D3:
                        Console.WriteLine("...Вызов метода ShowAdsUserList() на просмотр объявления пользователя...\n");
                        var list = ShowAdsUserList();
                        if (list == null) break;
                        Console.WriteLine("...Вызов метода ChangeAdsMenu() на изменение обновлений пользователя...\n");
                        ChangeAdsMenu(list);
                        break;
                    case ConsoleKey.D4:
                        Console.WriteLine("...Вызов метода FindAdsList() на поиск объявлений...\n");
                        FindAdsList();
                        break;
                    case ConsoleKey.D5:
                        Console.WriteLine("...Вызов метода BuyRequestAbs() на покупку товара из объявления...\n");
                        list = ReturnAdsList();
                        BuyRequestAbs(list);
                        break;
                    case ConsoleKey.D6:
                        Console.WriteLine("...Вызов на выход в авторизацию...\n");
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Неизвестная клавиша");
                        continue;
                }

                if (exit) break;
            }
            if (exit) Auth();
        }
        private static void BuyRequestAbs(List<Advertisement> ads)
        {
            Console.WriteLine($"\nВыберите объявление, которое вы хотите купить");
            var strId = Console.ReadLine();
            int.TryParse(strId, out var adId);
            Console.WriteLine("...Проверка на наличие объявления...\n");
            if (adId <= 0 || adId > ads.Count)
            {
                Console.WriteLine("Такого объявления нет");
                return;
            }

            var adToPay = ads[adId - 1];
            Console.WriteLine("...Проверка данных пользователя...\n");
            if (CurrentUser == null)
            {

                Console.WriteLine("не залогинен");
                return;
            }
            if(CurrentUser == adToPay.User)
            {
                Console.WriteLine("...Проверка данных пользователя на покупку...\n");
                Console.WriteLine("Нелья купить свой же товар!");
                return;
            }


            var payType = adToPay.PaymentType;
            var fabric = CreateFabric(payType);
            Console.WriteLine("1 чтобы оплатить товар, 2 отменить оплату");
            var key = Console.ReadLine();
            if(key == "2")
            {
                Console.WriteLine("Оплата отменена!");
            }
            else
            {
                fabric.CreatePayer().Pay();
            }
            ads.Remove(adToPay);
        }

        private static void ChangeAdsMenu(List<Advertisement> ads)
        {
            Console.WriteLine("Выберите действие: 1 - изменить, 2 - удалить, 3 - вернуться");
            var key = Console.ReadKey();
            Console.WriteLine();
            switch (key.Key)
            {
                case ConsoleKey.D1:
                    ModifyAdsMenu(ads);
                    break;
                case ConsoleKey.D2:
                    DeleteAdMenu(ads);
                    break;
                case ConsoleKey.D3:
                    break;
                default:
                    break;
            }
        }

        private static void DeleteAdMenu(List<Advertisement> ads)
        {
            Console.WriteLine("Выберите порядковый номер объявления");
            var strId = Console.ReadLine();
            int.TryParse(strId, out var adId);
            if (adId <= 0 || adId > ads.Count)
            {
                Console.WriteLine("Такого объявления нет");
                return;
            }

            var adToDelete = ads[adId - 1];


            var res =  _service.DeleteAd(adToDelete);
            Console.WriteLine("...Вызов метода DeleteAd(adToDelete) на удаление объявления...\n");

            if (res.IsSuccess)
            {
                Console.WriteLine("Объект успешно удален");
            }
            else
            {
                Console.WriteLine(res.ErrorMessage);
            }
        }

        private static void ModifyAdsMenu(List<Advertisement> ads)
        {
            Console.WriteLine("Выберите порядковый номер объявления");
            var strId = Console.ReadLine();
            int.TryParse(strId, out var adId);
            if (adId <= 0 ||  adId > ads.Count)
            {
                Console.WriteLine("Такого объявления нет");
                return;
            }

            var adToChange = ads[adId - 1];

            if (CurrentUser == null)
            {
                Console.WriteLine("не залогинен");
                return;
            }

            Console.WriteLine("...Вызов метода CreateAdMenu() на изменение объявления...\n");

            Advertisement ad = CreateAdMenu();

            ad.Id = adToChange.Id;

            var res = _service.SaveAd(ad);
            Console.WriteLine("...Вызов метода SaveAd(ad) на сохранения изменений объявлений...\n");

            if (res.IsSuccess)
            {
                Console.WriteLine("Объект успешно сохранен");
            }
            else
            {
                Console.WriteLine(res.ErrorMessage);
            }
        }

        private static void PlaceNewAd()
        {
            if (CurrentUser == null)
            {
                Console.WriteLine("не залогинен");
                return;
            }
            Advertisement ad = CreateAdMenu();
            Console.WriteLine("...Вызов метода CreateAdMenu() на создание объявлений...\n");
            _service.SaveAd(ad);
        }
        private static IAvitoFactory CreateFabric(PaymentType type)
        {
            switch (type)
            {
                case PaymentType.Cash:
                    return new CashFactory();
                case PaymentType.Webmoney:
                    return new WebMoneyFactory();
                case PaymentType.DebitCard:
                    return new DebitCardFactory();
                case PaymentType.SFP:
                    return new SFPFactory();
                default:
                    throw new NotImplementedException();

            }
        }

        private static Advertisement CreateAdMenu()
        {
            var adBuilder = new AdBuilder().User(CurrentUser);
            

            Console.WriteLine("Введите имя товара");
            adBuilder.ProductName(Console.ReadLine());

            Console.WriteLine("Введите описание товара");
            adBuilder.ProductDescription(Console.ReadLine());

            Console.WriteLine("Введите цену товара");
            adBuilder.Price(decimal.Parse(Console.ReadLine()));//добавить обработку неудачного парсинга

            Console.WriteLine("1 - наличка, 2 - эл.деньги, 3 - дебет, 4 - сбп, по умолчанию - наличка");
            var key = Console.ReadKey();
            Console.WriteLine();
            switch (key.Key)
            {
                case ConsoleKey.D1:
                    adBuilder.PaymentType(PaymentType.Cash);
                    break;
                case ConsoleKey.D2:
                    adBuilder.PaymentType(PaymentType.Webmoney);
                    break;
                case ConsoleKey.D3:
                    adBuilder.PaymentType(PaymentType.DebitCard);
                    break;
                case ConsoleKey.D4:
                    adBuilder.PaymentType(PaymentType.SFP);
                    break;
                default:
                    adBuilder.PaymentType(PaymentType.Cash);
                    break;
            }

            var ad = adBuilder.Build();
            return ad;
        }

        private static void ShowAdsList()
        {
            var list = _service.GetAdsInfoList();
            Console.WriteLine("...Вызов метода GetAdsInfoList() для получения объявлений...\n");
            if (list.Count == 0)
            {
                Console.WriteLine("Пусто");
                return;
            }
            foreach (var item in list)
            {
                Console.WriteLine(item);
            }
        }
        private static List<Advertisement> ReturnAdsList()
        {
            var list = _service.GetInfoAdsList();
            Console.WriteLine("...Вызов метода GetAdsInfoList() для получения объявлений...\n");
            if (list.Count == 0)
            {
                Console.WriteLine("Пусто");
                return null;
            }
            else
            {
                foreach(var item in list.ToList())
                {
                    Console.WriteLine($"{item.Product.Name}, {item.Product.Description}, {item.Price}p. {item.PaymentType},  {item.User.UserName}");
                }
            }
            return list;
        }

        private static void FindAdsList()
        {
            Console.WriteLine("Введите название товара");
            var nameAds = Console.ReadLine();
            var list = _service.GetFindAdsList(nameAds);
            Console.WriteLine("...Вызов метода GetFindAdsList(nameAds) для поиска объявлений...\n");
            if (list.Count == 0)
            {
                Console.WriteLine("Пусто");
                return;
            }
            foreach (var item in list)
            {
                Console.WriteLine(item);
            }
        }

        private static List<Advertisement> ShowAdsUserList()
        {
            var ads = _service.GetUserAdsList(CurrentUser);
            Console.WriteLine("...Вызов метода GetUserAdsList(CurrentUser) для получения списка объявлений...\n");
            var info = _service.GetUserAdsInfoList(ads);
            Console.WriteLine("...Вызов метода GetUserAdsInfoList(ads) для вывода объявлений пользователя...\n");
            if (info.Count == 0)
            {
                Console.WriteLine("Пусто");
                return null;
            }
            foreach (var item in info)
            {
                Console.WriteLine(item);
            }

            return ads;
        }

        private static void Auth()
        {
            var exit = false;
            while (true)
            {
                Console.WriteLine("Нажмите 1 чтобы зарегистрироваться, 2 чтобы авторизоваться, 3 чтобы войти как гость");
                var key = Console.ReadKey();
                Console.WriteLine();
                switch (key.Key)
                {
                    case ConsoleKey.D1:
                        Register();
                        break;
                    case ConsoleKey.D2:
                        if (Login()) exit = true;
                        break;
                    case ConsoleKey.D3:
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Неизвестная клавиша");
                        continue;
                }
                if (exit) break;
                Console.WriteLine("...Отмена регистрации...\n");
            }
            
            if (exit) Menu();
        }

        private static bool Register()
        {
            var userBuilder = new UserBuilder();
            Console.WriteLine("Введите логин");
            userBuilder.Username(Console.ReadLine());

            Console.WriteLine("Введите пароль");
            userBuilder.Password(ReadPassword());

            Console.WriteLine("Введите эл. почту");
            userBuilder.Email(Console.ReadLine());

            Console.WriteLine("...Отправка пользователем данных...\n");

            try
            {
                var user = userBuilder.Build();

                var result = _service.Register(user);
                Console.WriteLine("...Вызов метода Register(user) для записи информации о пользователе...\n");

                if (result.IsSuccess)
                {
                    Console.WriteLine("...Отправка системой сообщения об успешной регистрации...\n");
                    Console.WriteLine("Аккаунт успешно зарегистрирован");
                }
                else
                {
                    Console.WriteLine(result.ErrorMessage);
                }

                return result.IsSuccess;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }          
        }

        private static bool Login()
        {
            Console.WriteLine("Введите логин");
            var username = Console.ReadLine();
            Console.WriteLine("Введите пароль");
            var password = ReadPassword();
            Console.WriteLine("...Отправка пользователем данных...\n");

            var (result, user) = _service.Login(username, password);

            if (result.IsSuccess)
            {
                CurrentUser = user;
                Console.WriteLine("...Вызов метода Login(user) для входа пользователя в аккаунт...\n");
                Console.WriteLine("Вы успешно авторизованы");
            }
            else
            {
                Console.WriteLine(result.ErrorMessage);
            }

            return result.IsSuccess;
        }

        private static string ReadPassword()
        {
            var password = new StringBuilder();

            while (true)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter) break;
                Console.Write("*");
                password.Append(key.KeyChar);
            }
            Console.WriteLine();
            return password.ToString();
        }
    }
}
