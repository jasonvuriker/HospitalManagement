using HospitalManagement.Dtos;
using HospitalManagement.Repository;
using HospitalManagement.Repository.Interfaces;

namespace HospitalManagement.Services;

public interface IPatientService
{
    Task<IList<PatientDto>> GetHighSeverityPatients();
}


public class PatientService : IPatientService
{
    private readonly IPatientRepository _patientRepository;
    private readonly IDoctorRepository _doctorRepository;

    public PatientService(IPatientRepository patientRepository, IDoctorRepository doctorRepository)
    {
        _patientRepository = patientRepository;
        _doctorRepository = doctorRepository;
    }

    public async Task<IList<PatientDto>> GetHighSeverityPatients()
    {
        var highSeverityPatients = await _patientRepository.GetPatientsBySeverity(5);

        return highSeverityPatients.Select(r => new PatientDto
        { 
            Name = $"{r.Firstname} {r.Lastname}",
            Severity = r.PatientBlank.Severity,
        }).ToList();
    }
}
