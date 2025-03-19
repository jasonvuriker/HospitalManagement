using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Settings;

public class PdpSettings
{
    [Url]
    public string Endpoint { get; set; }

    [Required]
    [MaxLength(100)]

    public string DatabaseId { get; set; }

    [Required]
    [Range(1, 10)]
    public int BatchCount { get; set; }
}
