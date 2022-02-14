﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DinnerSpinner.Api.Domain.Contracts;
using DinnerSpinner.Api.Domain.Models;
using DinnerSpinner.Web.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DinnerSpinner.Api.Controllers
{
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
        public async Task<List<Spinner>> Get() => await _spinnerService.Get();

        [HttpGet("{id:length(24)}", Name = "GetSpinner")]
        public async Task<IActionResult> Get(string id)
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
        public async Task<IActionResult> AddDinner([FromRoute] string spinnerId, [FromBody] AddDinner dinner)
        {
            _logger.LogInformation("AddDinner {@Dinner}", dinner);
            var spinner = await _spinnerService.AddDinner(spinnerId, dinner.Name, dinner.Ingredients);

            return CreatedAtRoute("GetSpinner", new { id = spinnerId }, spinner);
        }

        [HttpDelete("{spinnerId:length(24)}/dinners/{dinnerId}")]
        public async Task<Spinner> DeleteDinner(string spinnerId, Guid dinnerId)
        {
            var spinner = await _spinnerService.RemoveDinner(spinnerId, dinnerId);

            return spinner;
        }

        [HttpDelete("{spinnerId:length(24)}")]
        public async Task<Spinner> DeleteSpinner(string spinnerId)
        {
            var spinner = await _spinnerService.Remove(spinnerId);

            return spinner;
        }
    }
}
