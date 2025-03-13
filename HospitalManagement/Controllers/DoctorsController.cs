using HospitalManagement.Dtos;
using HospitalManagement.Filters;
using HospitalManagement.Services.Doctors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Controllers;
[Route("api/[controller]")]
[ApiController]
public class DoctorsController : ControllerBase
{
    private readonly IDoctorService _doctorService;

    public DoctorsController(IDoctorService doctorService)
    {
        _doctorService = doctorService;
    }

    [LogActionFilter]
    [HttpPost("create")]
    public async Task<IActionResult> CreateDoctor([FromBody] CreateDoctorDto doctorDto)
    {
        await _doctorService.CreateDoctor(doctorDto);

        return Created();
    }

    [HttpGet("get-all")]
    public async Task<IActionResult> GetDoctors()
    {
        return Ok(_doctorService.GetAllDoctors());
    }
}
