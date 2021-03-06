using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DinnerSpinner.Domain.Contracts;
using DinnerSpinner.Domain.DomainServices;
using DinnerSpinner.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DinnerSpinner.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SpinnerController : ControllerBase
{
    private readonly SpinnerService _spinnerService;
    private readonly ILogger<SpinnerController> _logger;

    public SpinnerController(SpinnerService spinnerService, ILogger<SpinnerController> logger)
    {
        _spinnerService = spinnerService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IList<Spinner>> Get() => await _spinnerService.GetAll();

    [HttpGet("{id}", Name = "GetSpinner")]
    public async Task<IActionResult> Get(Guid id)
    {
        var spinner = await _spinnerService.Get(id);

        if (spinner == null)
        {
            return NotFound();
        }

        return Ok(spinner);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody]CreateSpinner spinner)
    {
        _logger.LogInformation("Create {@Spinner}", spinner);
        var result = await _spinnerService.Create(spinner);

        return CreatedAtRoute("GetSpinner", new { id = result.Id }, result);
    }

    [HttpPost("{spinnerId}/dinners")]
    public async Task<IActionResult> AddDinner([FromRoute] Guid spinnerId, [FromBody] AddDinner dinner)
    {
        _logger.LogInformation("AddDinner {@Dinner}", dinner);
        var spinner = await _spinnerService.AddDinner(spinnerId, dinner.Name, dinner.Ingredients);

        return CreatedAtRoute("GetSpinner", new { id = spinnerId }, spinner);
    }

    [HttpPatch("{spinnerId:guid}/dinners/{dinnerId:guid}")]
    public async Task<IActionResult> AddDinner([FromRoute] Guid spinnerId, Guid dinnerId, [FromBody] UpdateDinner dinner)
    {
        _logger.LogInformation("Path Dinner {@Dinner}", dinner);
        var spinner = await _spinnerService.UpdateDinner(spinnerId, dinnerId, dinner.Name);

        return Ok(spinner);
    }

    [HttpDelete("{spinnerId}/dinners/{dinnerId}")]
    public async Task<Spinner> DeleteDinner(Guid spinnerId, Guid dinnerId)
    {
        var spinner = await _spinnerService.RemoveDinner(spinnerId, dinnerId);

        return spinner;
    }

    [HttpDelete("{spinnerId}")]
    public async Task<Spinner> DeleteSpinner(Guid spinnerId)
    {
        var spinner = await _spinnerService.Remove(spinnerId);

        return spinner;
    }
}
