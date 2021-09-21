using System;
using System.Collections.Generic;
using System.Text;
/*
    Version: 1.0
    Author: Pedro Pablo Soto
*/
namespace Repository.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
    }
}
