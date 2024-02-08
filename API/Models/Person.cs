using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPNetCoreWebAPITraining.Models
{
    [Table("Person")]
    public class Person
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column("FirstName")]
        public required string FirstName { get; set; }

        [Required]
        [Column("LastName")]
        public required string LastName { get; set; }
    }
}