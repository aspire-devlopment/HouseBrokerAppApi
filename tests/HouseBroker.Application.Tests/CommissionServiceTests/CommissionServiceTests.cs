using HouseBrokerApp.Application.IRepository;
using HouseBrokerApp.Application.Services;
using HouseBrokerApp.Domain.Entities;
using Moq;
using NUnit.Framework.Legacy;

[TestFixture]
public class CommissionServiceTests
{
    private Mock<IGenericRepository<CommissionRate>> _repoMock = null!;
    private CommissionService _service = null!;

    [SetUp]
    public void Setup()
    {
        _repoMock = new Mock<IGenericRepository<CommissionRate>>();
        _service = new CommissionService(_repoMock.Object);
    }

    [Test]
    public async Task GetCommissionRatesAsync_ReturnsRates()
    {
        var rates = new List<CommissionRate>
        {
            new CommissionRate { MinPrice = 0, MaxPrice = 1000, Rate = 5 },
            new CommissionRate { MinPrice = 1000, MaxPrice = 5000, Rate = 3 }
        };

        _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(rates);

        var result = await _service.GetCommissionRatesAsync();

        ClassicAssert.AreEqual(2, result.Count);
        ClassicAssert.AreEqual(5, result[0].Rate);
    }

    [Test]
    public void CalculateCommission_ReturnsCorrectCommission()
    {
        var rates = new List<CommissionRate>
        {
            new CommissionRate { MinPrice = 0, MaxPrice = 1000, Rate = 5 },
            new CommissionRate { MinPrice = 1000, MaxPrice = 5000, Rate = 3 }
        };

        _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(rates);

        decimal price = 800m;
        var commission = _service.CalculateCommission(price);

        // 5% of 800 = 40
        ClassicAssert.AreEqual(40m, commission);
    }

    [Test]
    public void CalculateCommission_ThrowsException_WhenNoRateFound()
    {
        var rates = new List<CommissionRate>
        {
            new CommissionRate { MinPrice = 0, MaxPrice = 1000, Rate = 5 }
        };

        _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(rates);

        decimal price = 2000m;

        var ex = Assert.Throws<Exception>(() => _service.CalculateCommission(price));
        Assert.That(ex.Message, Is.EqualTo("Commission rate not configured."));
    }
}
