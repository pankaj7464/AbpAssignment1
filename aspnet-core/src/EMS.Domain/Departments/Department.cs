using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace EMS.Departments
{
    public class Department:FullAuditedEntity<Guid>
    {
        public required string DepartmentName {  get; set; }
        public string DepartmentDescription { get; set;}
    }
}
