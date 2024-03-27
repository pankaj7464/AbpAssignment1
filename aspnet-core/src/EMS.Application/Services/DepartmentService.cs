using EMS.Departments;
using EMS.DTOs;
using EMS.interfaces;
using EMS.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace EMS.Services
{
    public class DepartmentService : CrudAppService<Department, DepartmentDto, Guid, PagedAndSortedResultRequestDto, DepartmentCreateUpdateDto, DepartmentCreateUpdateDto>, IDepartmentService
    {
     
        public DepartmentService(IRepository<Department, Guid> repository) : base(repository)
        {
            GetPolicyName = EMSPermissions.Admin.Default;
            GetListPolicyName = EMSPermissions.Admin.Default;
            CreatePolicyName = EMSPermissions.Admin.Create;
            UpdatePolicyName = EMSPermissions.Admin.Edit;
            DeletePolicyName = EMSPermissions.Admin.Delete;
        }
    }
}
