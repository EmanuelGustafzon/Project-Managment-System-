using Business.Dtos;
using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ProjectManagementAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CurrencyController : Controller
{
    private readonly ICurrencyService _currencyService;

    public CurrencyController(ICurrencyService currencyService)
    {
        _currencyService = currencyService;
    }

    [HttpPost]
    public async Task<ObjectResult> AddCurrency([FromBody] CurrencyDto currencyForm)
    {
        try
        {
            var result = await _currencyService.CreateCurrencyAsync(currencyForm);
            return StatusCode(result.StatusCode, result.ErrorMessage);

        } catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return StatusCode(500, "An internal error occurred.");
        }
    }

    [HttpGet]
    public async Task<ObjectResult> GetAllCurrencies()
    {
        try
        {
            var result = await _currencyService.GetAllCurrenciesAsync();
            return StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return StatusCode(500, "An internal error occurred.");
        }
    }

    [HttpGet("{id}")]
    public async Task<ObjectResult> GetCurrencyById(int id)
    {
        try
        {
            var result = await _currencyService.GetCurrencyAsync(id);
            return StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return StatusCode(500, "An internal error occurred.");
        }
    }
    [HttpPut("{id}")]
    public async Task<ObjectResult> UpdateCurrency(int id, [FromBody] CurrencyDto updatedCurrency)
    {
        try
        {
            var result = await _currencyService.UpdateCurrencyAsync(id, updatedCurrency);
            return StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return StatusCode(500, "An internal error occurred.");
        }
    }
    [HttpDelete("{id}")]
    public async Task<ObjectResult> UpdateCurrency(int id)
    {
        try
        {
            var result = await _currencyService.DeleteCurrencyAsync(id);
            return StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return StatusCode(500, "An internal error occurred.");
        }
    }
}
