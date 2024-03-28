using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Identity;

namespace EMS.DTOs
{
    public class ApplicationUserDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public Guid? DepartmentId { get; set; }
    }
}
