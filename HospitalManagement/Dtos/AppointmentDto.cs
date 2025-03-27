namespace HospitalManagement.Dtos;

public class AppointmentDto
{
    public int AppointmentId { get; set; }

    public DateTime AppointmentDate { get; set; }

    public int PatientId { get; set; }

    public string PatientName { get; set; }

    public int DoctorId { get; set; }
}
