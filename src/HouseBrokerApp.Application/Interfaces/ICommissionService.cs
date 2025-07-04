using HouseBrokerApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseBrokerApp.Application.Interfaces;

public interface ICommissionService
{
    decimal CalculateCommission(decimal price);
    Task<List<CommissionRate>> GetCommissionRatesAsync();
}
