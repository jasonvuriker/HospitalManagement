﻿using HospitalManagement.Dtos;
using HospitalManagement.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Serilog.Context;

namespace HospitalManagement.Controllers;
[Route("api/[controller]")]
[ApiController]
public class PatientsController : ControllerBase
{
    private readonly ILogger<PatientsController> _logger;
    private readonly DoctorsSettings _doctorsSettings;
    private readonly DoctorsSettings _doctorsSettings1;

    public PatientsController(
        IOptions<DoctorsSettings> doctorsSettings,
        IConfiguration configuration,
        ILogger<PatientsController> logger)
    {
        _logger = logger;
        _doctorsSettings = doctorsSettings.Value;

        _doctorsSettings1 = configuration.GetSection("DoctorsSettings").Get<DoctorsSettings>();
    }

    [HttpPost("arrange-appointment")]
    public async Task<IActionResult> ArrangeAppointment([FromBody] ArrangeAppointmentDto requestDto)
    {
        using (LogContext.PushProperty("PatientId", requestDto.PatientId))
        {
            var time = TimeOnly.FromDateTime(requestDto.AppointmentDate);

            if (!time.IsBetween(_doctorsSettings.WorkTime.Start, _doctorsSettings.WorkTime.End))
            {
                _logger.LogWarning("Doctor is not available at this time");

                return BadRequest("Doctor is not available at this time");
            }

            _logger.LogWarning("{@Request}, Patient with PassportSerial {PassportSerial}", requestDto, requestDto.PassportSerial);
        }

        return Ok("Your appointment is arranged");
    }
}
