using AutoMapper;
using EMS.Departments;
using EMS.DTOs;

namespace EMS;

public class EMSApplicationAutoMapperProfile : Profile
{
    public EMSApplicationAutoMapperProfile()
    {

        CreateMap<DepartmentCreateUpdateDto, Department>();
        CreateMap<Department, DepartmentDto>().ReverseMap();

        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
    }
}
