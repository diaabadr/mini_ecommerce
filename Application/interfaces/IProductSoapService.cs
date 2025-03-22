
using System;

public interface IProductSoapService
{
    Task<string> CheckProductAvailability(string productId);
}
