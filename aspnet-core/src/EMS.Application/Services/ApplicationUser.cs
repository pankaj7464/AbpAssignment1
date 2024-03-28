using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Identity;

namespace EMS.Services
{
    public  class ApplicationUser: IdentityUser
    {
        public Guid DepartmentId { get; set; }

        ApplicationUser(Guid departmentId)
        {
            DepartmentId = departmentId;
        }
    }
}
