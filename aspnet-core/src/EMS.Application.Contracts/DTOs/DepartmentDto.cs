using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace EMS.DTOs
{
    public class DepartmentDto : IEntityDto<Guid>
    {
        public string DepartmentName { get; set; }
        public string DepartmentDescription { get; set; }
        public Guid Id { get ; set ; }
    }
}
