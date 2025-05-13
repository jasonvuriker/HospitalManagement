using System.Text.Json;
using HospitalManagement.Dtos;
using HospitalManagement.Filters;
using HospitalManagement.Services;
using HospitalManagement.Services.Doctors;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace HospitalManagement.Controllers;
[Route("api/[controller]")]
[ApiController]
[EnableCors("frontend")]
public class DoctorsController : ControllerBase
{
    private readonly IDoctorService _doctorService;
    private readonly IConfiguration _configuration;
    private readonly PdpService _pdpService;

    public DoctorsController(
        IDoctorService doctorService,
        IConfiguration configuration,
        PdpService pdpService)
    {
        _doctorService = doctorService;
        _configuration = configuration;
        _pdpService = pdpService;
    }

    [LogActionFilter]
    [HttpPost]
    public async Task<IActionResult> CreateDoctor([FromBody] CreateDoctorDto doctorDto)
    {
        await _doctorService.CreateDoctor(doctorDto);

        return Created();
    }

    [HttpGet]
    [EnableRateLimiting("tokenBucket")]
    public async Task<IActionResult> GetDoctors()
    {
        return Ok(_doctorService.GetAllDoctors());
    }

    [HttpGet("{id:int}")]
    [EnableRateLimiting("sliding")]
    public async Task<IActionResult> GetDoctor([FromRoute] int id)
    {
        return Ok(await _doctorService.GetDoctor(id));
    }

    [HttpGet("pdp-data")]
    public async Task<IActionResult> GetSettings()
    {
        return Ok(await _pdpService.GetPdpData());
    }

    [HttpPost("notify")]
    public async Task<IActionResult> NotifyDoctors()
    {
        await _doctorService.SendPatientsStatus();
        return Ok();
    }
}
