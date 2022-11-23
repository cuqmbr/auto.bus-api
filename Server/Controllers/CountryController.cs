using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Server.Services;
using SharedModels.DataTransferObjects;
using SharedModels.QueryParameters.Objects;

namespace Server.Controllers;

[Route("api/countries")]
[ApiController]
public class CountryController : ControllerBase
{
    private readonly ICountryManagementService _countryManagementService;
    
    public CountryController(ICountryManagementService countryManagementService)
    {
        _countryManagementService = countryManagementService;
    }

    [HttpPost]
    public async Task<IActionResult> AddCountry(CreateCountryDto country)
    {
        var result = await _countryManagementService.AddCountry(country);
    
        if (!result.isSucceed)
        {
            return BadRequest(result.message);
        }
    
        return CreatedAtAction(nameof(GetCountry), new {id = result.country.Id}, result.country);
    }

    [HttpGet]
    public async Task<IActionResult> GetCountries([FromQuery] CountryParameters parameters)
    {
        var result = await _countryManagementService.GetCountries(parameters);

        if (!result.isSucceed)
        {
            return BadRequest(result.message);
        }
        
        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.pagingMetadata));
        
        return Ok(result.countries);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCountry(int id, [FromQuery] string? fields)
    {
        if (!await _countryManagementService.IsCountryExists(id))
        {
            return NotFound();
        }

        var result = await _countryManagementService.GetCountry(id, fields);

        if (!result.isSucceed)
        {
            return BadRequest(result.message);
        }

        return Ok(result.country);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCountry(int id, UpdateCountryDto country)
    {
        if (id != country.Id)
        {
            return BadRequest();
        }
        
        var result = await _countryManagementService.UpdateCountry(country);
    
        if (!result.isSucceed)
        {
            return BadRequest(result.message);
        }
    
        return Ok(result.country);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCountry(int id)
    {
        if (!await _countryManagementService.IsCountryExists(id))
        {
            return NotFound();
        }
        
        var result = await _countryManagementService.DeleteCountry(id);
        
        if (!result.isSucceed)
        {
            return BadRequest(result.message);
        }
    
        return NoContent();
    }
}
