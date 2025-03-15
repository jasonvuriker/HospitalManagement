namespace HospitalManagement.Settings;

public class DoctorsSettings
{
    public WorkTime WorkTime { get; set; }
}

public class WorkTime
{
    public TimeOnly Start { get; set; }

    public TimeOnly End { get; set; }
}
