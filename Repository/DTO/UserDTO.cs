using Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;
/*
    Version: 1.0
    Author: Pedro Pablo Soto
*/
namespace Repository.DTO
{
    public class UserDTO: User
    {
        public string Token { get; set; }
    }
}
