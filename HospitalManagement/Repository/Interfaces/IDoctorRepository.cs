using HospitalManagement.DataAccess.Entities;

namespace HospitalManagement.Repository.Interfaces;

public interface IDoctorRepository : IRepository<Doctor>
{
    Task<Doctor> GetByIdCachedAsync(int id);
}
