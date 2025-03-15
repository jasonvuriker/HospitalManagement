﻿using System.Text.Json;
using HospitalManagement.Dtos;
using HospitalManagement.Filters;
using HospitalManagement.Services;
using HospitalManagement.Services.Doctors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Controllers;
[Route("api/[controller]")]
[ApiController]
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

    [HttpGet("pdp-data")]
    public async Task<IActionResult> GetSettings()
    {
        return Ok(await _pdpService.GetPdpData());
    }
}
