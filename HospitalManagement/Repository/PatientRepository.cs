using HospitalManagement.DataAccess.Entities;
using HospitalManagement.DataAccess;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Repository;

public interface IPatientRepository
{
    Task<IList<Patient>> GetPatientsBySeverity(int severity);
}

public class PatientRepository : IPatientRepository
{
    private readonly HospitalContext _context;
    private readonly IMemoryCache _cache;

    private const string PatientsCacheKey = "Patients";

    public PatientRepository(HospitalContext context, IMemoryCache cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<IList<Patient>> GetPatientsBySeverity(int severity)
    {
        if (_cache.TryGetValue(PatientsCacheKey, out IList<Patient> patients))
        {
            return patients;
        }

        var patientsAll = await _context.Patients
            .Include(r => r.PatientBlank)
            .Where(r => r.PatientBlank.Severity > severity)
            .ToListAsync();

        _cache.Set(PatientsCacheKey, patientsAll);

        return patientsAll;
    }
}
