using AutoMapper;
using AutoMapper.QueryableExtensions;
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

    void UpdateDoctor(UpdateDoctorDto doctorDto);

    IList<DoctorDto> GetAllDoctors();

    Task<DoctorDto> GetDoctor(int id);

    Task SendPatientsStatus();
}


public class DoctorService : IDoctorService
{
    private readonly IDoctorRepository _doctorRepository;
    private readonly IPatientRepository _patientRepository;
    private readonly INotificationService _notificationService;
    private readonly IMapper _mapper;

    public DoctorService(
        IDoctorRepository doctorRepository,
        IPatientRepository patientRepository,
        INotificationService notificationService,
        IMapper mapper)
    {
        _doctorRepository = doctorRepository;
        _patientRepository = patientRepository;
        _notificationService = notificationService;
        _mapper = mapper;
    }

    public async Task CreateDoctor(CreateDoctorDto doctorDto)
    {
        var doctor = _mapper.Map<Doctor>(doctorDto);

        await _doctorRepository.AddAsync(doctor);
        await _doctorRepository.SaveChangesAsync();
    }

    public void UpdateDoctor(UpdateDoctorDto doctorDto)
    {
        var doctor = _doctorRepository.GetById(doctorDto.Id);

        _mapper.Map<UpdateDoctorDto, Doctor>(doctorDto, doctor);

        _doctorRepository.Update(doctor);
        _doctorRepository.SaveChanges();
    }

    public IList<DoctorDto> GetAllDoctors()
    {
        var doctors = _doctorRepository
            .GetAll()
            .AsNoTracking()
            .ProjectTo<DoctorDto>(_mapper.ConfigurationProvider)
            .ToList();

        return doctors;
    }

    public async Task<DoctorDto> GetDoctor(int id)
    {
        var doctor = await _doctorRepository.GetByIdAsync(id);

        var doctorDto = _mapper.Map<DoctorDto>(doctor);

        return doctorDto;
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
