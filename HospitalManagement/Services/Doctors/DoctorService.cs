using HospitalManagement.DataAccess.Entities;
using HospitalManagement.Dtos;
using HospitalManagement.Enums;
using HospitalManagement.Repository;
using HospitalManagement.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Services.Doctors;

public interface IDoctorService
{
    Task CreateDoctor(CreateDoctorDto doctorDto);

    IList<DoctorDto> GetAllDoctors();

    Task SendPatientsStatus();
}


public class DoctorService : IDoctorService
{
    private readonly IDoctorRepository _doctorRepository;
    private readonly IPatientRepository _patientRepository;
    private readonly INotificationService _notificationService;

    public DoctorService(
        IDoctorRepository doctorRepository, 
        IPatientRepository patientRepository,
        INotificationService notificationService)
    {
        _doctorRepository = doctorRepository;
        _patientRepository = patientRepository;
        _notificationService = notificationService;
    }

    public async Task CreateDoctor(CreateDoctorDto doctorDto)
    {
        var doctor = doctorDto.ToEntity();

        await _doctorRepository.AddAsync(doctor);
        await _doctorRepository.SaveChangesAsync();
    }

    public IList<DoctorDto> GetAllDoctors()
    {
        return _doctorRepository.GetAll().AsNoTracking().Select(r => new DoctorDto()
        {
            DoctorId = r.DoctorId,
            Firstname = r.Firstname,
            Lastname = r.Lastname,
            SpecialtyId = r.SpecialityId
        }).ToList();
    }

    public async Task SendPatientsStatus()
    {
        var highSeverityPatients = await _patientRepository
            .GetPatientsBySeverity(5);

        var doctorsId = _doctorRepository
            .GetAll()
            .AsNoTracking()
            .Where(r => r.SpecialityId == (int)SpecialtyType.Operatives)
            .Select(r => r.DoctorId)
            .ToList();

        foreach (var item in doctorsId)
        {
            await _notificationService.Notify(item, highSeverityPatients.Select(r => r.PatientId).ToArray());
        }
    }
}
