using HouseBrokerApp.Application.Interfaces;
using HouseBrokerApp.Application.IRepository;
using HouseBrokerApp.Domain.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HouseBrokerApp.Application.Services;

public class CommissionService : ICommissionService
{
    private readonly IGenericRepository<CommissionRate> _repository;

    public CommissionService(IGenericRepository<CommissionRate> repository)
    {
        _repository = repository;
    }

    public async Task<List<CommissionRate>> GetCommissionRatesAsync()
    {
        return await _repository.GetAllAsync();
    }

    public decimal CalculateCommission(decimal price)
    {
        // Normally you'd cache these values — see note below
        var ratesTask = _repository.GetAllAsync();
        ratesTask.Wait(); // Blocking for simplicity here; better handled in async context
        var rates = ratesTask.Result;

        var applicableRate = rates
             .FirstOrDefault(r => price >= r.MinPrice && (r.MaxPrice == null || price < r.MaxPrice));

        if (applicableRate == null)
            throw new Exception("Commission rate not configured.");

        return price * (applicableRate.Rate / 100m);
        
    }
}
