using HospitalManagement.DataAccess.Entities;

namespace HospitalManagement.Dtos;

public class CreateDoctorDto
{
    public string Firstname { get; set; }

    public string Lastname { get; set; }

    public int SpecialtyId { get; set; }    
}
