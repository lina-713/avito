using Avito.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace avito
{
    public class UserBuilder
    {
        private string _username;
        private string _password;
        private string _email;
        private string _name;
        public UserBuilder() { }

        public UserBuilder Username(string username)
        {
            _username = username;
            return this;
        }
        public UserBuilder Password(string password)
        {
            _password = password;
            //здесь можно реализовать логику хэширования
            return this;
        }
        public UserBuilder Email(string email)
        {
            _email = email;
            return this;
        }
        public UserBuilder Name(string name)
        {
            _name = name;   
            return this;
        }
        public User Build()
        {
            if (_username.IsEmpty() || _password.IsEmpty() || _email.IsEmpty())
                throw new ArgumentException("Поля логина, пароля и эл. почты обязательны для заполнения");

            return new User { Email = _email, Name = _name, Id = Guid.NewGuid(), Password = _password, UserName = _username };
        }
    }

}
