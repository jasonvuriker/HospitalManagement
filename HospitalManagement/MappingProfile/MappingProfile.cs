using AutoMapper;
using HospitalManagement.DataAccess.Entities;
using HospitalManagement.Dtos;

namespace HospitalManagement.MappingProfile;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        //CreateMap<Source,Destination>

        CreateMap<Doctor, DoctorDto>()
            .ForMember(
                dest => dest.Fullname,
                opt => opt.MapFrom(src => $"{src.Firstname} {src.Lastname}"))
            .ForMember(r => r.SpecialtyName, opt => opt.MapFrom(src => src.Speciality.Name));

        CreateMap<Appointment, AppointmentDto>()
            .ForMember(dest => dest.PatientName,
                opt => opt.MapFrom(src => $"{src.Patient.Firstname} {src.Patient.Lastname}"));

        CreateMap<CreateDoctorDto, Doctor>();

        CreateMap<UpdateDoctorDto, Doctor>();
    }
}
