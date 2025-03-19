using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Settings;

public class DoctorsSettings
{
    [Required]
    public WorkTime? WorkTime { get; set; }
}

public class WorkTime
{
    [Required] 
    public TimeOnly Start { get; set; }

    [Required] 
    public TimeOnly End { get; set; }
}
