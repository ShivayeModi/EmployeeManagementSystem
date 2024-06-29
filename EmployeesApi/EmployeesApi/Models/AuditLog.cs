using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeesApi.Models
{
    public class AuditLog
    {
        public int Id { get; set;}
        public string EntityName { get; set; }
        public int EntityID { get; set; }
        public string Action { get; set; }
        public string ChangedData { get; set; }
        public DateTime Timestamp { get; set; }
        public int UserID { get; set; }
    }
}
