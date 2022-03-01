using MyProje.DAL;
using MyProje.DAL.UserAddress;
using MyProje.Models.Address;
using MyProje.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyProje.BusinessLogic
{
    public class UserAddressBL
    {
        private readonly UserAddressDA _userAddress;
        public UserAddressBL(UserAddressDA userAddress)
        {
            _userAddress = userAddress;
        }

        public Response List(AddressRequest.List credentials)
        {
            return _userAddress.List(credentials);
        }

        public Response Create(AddressRequest.Create credentials)
        {
            return _userAddress.Create(credentials);
        }

        public Response Edit(AddressRequest.Edit credentials)
        {
            return _userAddress.Edit(credentials);
        }

        public Response Delete(AddressRequest.Delete credentials)
        {
            return _userAddress.Delete(credentials);
        }

    }
}
