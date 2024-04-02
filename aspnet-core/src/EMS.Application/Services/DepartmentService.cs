using EMS.Departments;
using EMS.DTOs;
using EMS.interfaces;
using EMS.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace EMS.Services
{

    [Authorize]
    public class DepartmentService : CrudAppService<Department, DepartmentDto, Guid, PagedAndSortedResultRequestDto, DepartmentCreateUpdateDto, DepartmentCreateUpdateDto>, IDepartmentService
    {
        public DepartmentService(IRepository<Department, Guid> repository) : base(repository)
        {
       
        }
    }
}
