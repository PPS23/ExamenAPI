using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Repository.Models
{
    public class Log
    {
        [Required(ErrorMessage = "Id is required")]
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "RegisterDate is required")]
        public DateTime RegisterDate { get; set; }

        [Required(ErrorMessage = "Layer is required")]
        [StringLength(100, ErrorMessage = "Layer cannot be longer than 100 characters.")]
        public string Layer { get; set; }

        [Required(ErrorMessage = "FileName is required")]
        [StringLength(100, ErrorMessage = "FileName cannot be longer than 100 characters.")]
        public string FileName { get; set; }

        [Required(ErrorMessage = "MethodName is required")]
        [StringLength(100, ErrorMessage = "MethodName cannot be longer than 100 characters.")]
        public string MethodName { get; set; }

        [Required(ErrorMessage = "Message is required")]
        [StringLength(800, ErrorMessage = "Message cannot be longer than 800 characters.")]
        public string Message { get; set; }
    }
}
