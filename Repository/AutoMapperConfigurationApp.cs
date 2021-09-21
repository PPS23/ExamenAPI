using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Repository.Models;
using Repository.DTO;
/*
    Version: 1.0
    Author: Pedro Pablo Soto
*/
namespace Repository
{
    public class AutoMapperConfigurationApp: Profile
    {
        public AutoMapperConfigurationApp()
        {
            CreateMap<Contact, ContactDTO>();
            CreateMap<ContactDTO, Contact>();

            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();

            CreateMap<Log, LogDTO>();
            CreateMap<LogDTO, Log>();
        }
    }
}
