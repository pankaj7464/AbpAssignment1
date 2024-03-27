
using EMS.Departments;
using System;

using System.ComponentModel.DataAnnotations.Schema;

using Volo.Abp.Domain.Entities.Auditing;

namespace EMS.Employees
{
    public class Employee:FullAuditedEntity<Guid>
    {
        public required string Name { get; set; }

        [ForeignKey(nameof(Department))]
        public required Guid DepartmentId { get; set; }

        public virtual Department Department { get; set; }
        
    }
}
