namespace HospitalManagement.Services;

public interface INotificationService
{
    Task Notify(int doctorId, int[] patientIds);
}

public class NotificationService : INotificationService
{
    public async Task Notify(int doctorId, int[] patientIds)
    {
    }
}
