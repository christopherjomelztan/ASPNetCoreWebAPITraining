using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entities
{
    public class Person : BaseEntity
    {
        protected Person(int id) : base(id)
        {
        }

        public required string FirstName { get; set; }

        public required string LastName { get; set; }
    }
}