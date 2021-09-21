using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
/*
    Version: 1.0
    Author: Pedro Pablo Soto
*/
namespace ExamenAPI.Helpers
{
    public enum ResponseStatus 
    {
        Error = 0,
        Success = 1,
        ValidationError = 2
        
    }

    public class ResponseDTO
    {
        public ResponseStatus Status { get; set; }
        public string Message { get; set; }
        public object? Data { get; set; }
    }
}
