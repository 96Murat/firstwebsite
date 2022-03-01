using MyProje.DAL;
using MyProje.DAL.User;
using System;
using System.Collections.Generic;
using MyProje.Models;
using MyProje.Models.User;

namespace MyProje.BusinessLogic
{
    public class UserBL
    {
        private readonly UserDA _user;

        public UserBL(UserDA user)
        {
            _user = user;

        }
        public Response List(UserRequest.List credentials)
        {
            return _user.List(credentials);
        }

        public Response Create(UserRequest.Create credentials)
        {
            return _user.Create(credentials);
        }

        public Response Edit(UserRequest.Edit credentials)
        {
            return _user.Edit(credentials);
        }

        public Response Delete(UserRequest.Delete credentials)
        {
            return _user.Delete(credentials);
        }

    }
}
