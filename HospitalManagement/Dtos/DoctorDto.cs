namespace HospitalManagement.Dtos;

public class DoctorDto
{
    public int DoctorId { get; set; }

    public string Fullname { get; set; }

    public int SpecialityId { get; set; }

    public string SpecialtyName { get; set; }

    public IList<AppointmentDto> Appointments { get; set; }
}
