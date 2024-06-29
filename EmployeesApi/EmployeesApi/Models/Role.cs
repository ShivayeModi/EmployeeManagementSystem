using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeesApi.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [NotMapped]
        public DateTime CreatedAt { get; set; }
        [NotMapped]
        public DateTime UpdatedAt { get; set; }
    }
}
