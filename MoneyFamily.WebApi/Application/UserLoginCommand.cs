﻿namespace MoneyFamily.WebApi.Application
{
    public class UserLoginCommand
    {
        public string Email { get; }
        public string Password { get; }

        public UserLoginCommand(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
