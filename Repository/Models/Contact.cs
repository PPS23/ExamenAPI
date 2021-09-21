using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
/*
    Version: 1.0
    Author: Pedro Pablo Soto
*/
namespace Repository.Models
{
    public class Contact
    {
        [Required(ErrorMessage = "Id is required")]
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "RegisterDate is required")]
        [DataType(DataType.DateTime)]
        public DateTime RegisterDate { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [DataType(DataType.Text)]
        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [DataType(DataType.Text)]
        [StringLength(150, ErrorMessage = "Address cannot be longer than 150 characters.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [DataType(DataType.Text)]
        [StringLength(15, ErrorMessage = "Phone cannot be longer than 15 characters.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "CURP is required")]
        [DataType(DataType.Text)]
        [StringLength(20, ErrorMessage = "CURP cannot be longer than 20 characters.")]
        public string CURP { get; set; }
    }
}
