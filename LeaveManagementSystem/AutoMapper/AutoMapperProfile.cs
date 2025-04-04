using AutoMapper;
using LeaveManagementSystem.Migrations;
using LeaveManagementSystem.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LeaveManagementSystem.Helper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<EmployeDTO, Employe>();
            CreateMap<Employe, EmployeDTO>();
            CreateMap<LeavDTO, LeavRequestess>();  // Mapping for ApplyLeave
            CreateMap<LeavRequestess, LeavDTO>();  // Reverse Mapping (if needed)
            CreateMap<LeavRequestess, LeavRequestess>(); // If needed for DTO conversion
          


        }
    }
}
  