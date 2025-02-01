﻿namespace Business.Dtos;

public class CurrencyDto
{
    public int Id { get; set; }
    public string Currency { get; set; } = null!;
}

public class CurrencyRegistrationForm
{
    public string Currency { get; set; } = null!;
}

