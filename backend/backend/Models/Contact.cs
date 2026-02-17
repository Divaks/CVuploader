using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContactManager.Models
{
    public class Contact
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        public bool Married { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Salary { get; set; }
    }
}