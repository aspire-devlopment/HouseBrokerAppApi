using HouseBrokerApp.API.Controllers;
using HouseBrokerApp.Application.Interfaces;
using HouseBrokerApp.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework.Legacy;
using System.Security.Claims;

[TestFixture]
public class ListControllerTests
{
    private Mock<IPropertyListingService> _serviceMock = null!;
    private ListController _controller = null!;

    [SetUp]
    public void Setup()
    {
        _serviceMock = new Mock<IPropertyListingService>();
        _controller = new ListController(_serviceMock.Object);
    }

    private ClaimsPrincipal CreateUser(bool isBroker = false)
    {
        var claims = new List<Claim>();
        if (isBroker)
            claims.Add(new Claim(ClaimTypes.Role, "Broker"));

        var identity = new ClaimsIdentity(claims, "TestAuth");
        return new ClaimsPrincipal(identity);
    }

    [Test]
    public async Task Get_ReturnsOk_WhenListingExists()
    {
        var listing = new PropertyListingResponse { Id = 1, Title = "Test House" };
        _serviceMock.Setup(s => s.GetByIdAsync(1, It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(listing);

        var result = await _controller.Get(1);

        ClassicAssert.IsInstanceOf<OkObjectResult>(result);
        var okResult = (OkObjectResult)result;
        ClassicAssert.AreEqual(listing, okResult.Value);
    }

    [Test]
    public async Task Get_ReturnsNotFound_WhenListingDoesNotExist()
    {
        _serviceMock.Setup(s => s.GetByIdAsync(1, It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync((PropertyListingResponse?)null);

        var result = await _controller.Get(1);

        ClassicAssert.IsInstanceOf<NotFoundResult>(result);
    }

    [Test]
    public async Task GetAll_ReturnsOkWithListings()
    {
        var listings = new List<PropertyListingSeekerResponse>
        {
            new PropertyListingSeekerResponse { Id = 1, Title = "House 1" },
            new PropertyListingSeekerResponse { Id = 2, Title = "House 2" }
        };

        _serviceMock.Setup(s => s.GetAllAsync(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(listings);

        var result = await _controller.GetAll();

        ClassicAssert.IsInstanceOf<OkObjectResult>(result);
        var okResult = (OkObjectResult)result;
        ClassicAssert.AreEqual(listings, okResult.Value);
    }

    [Test]
    public async Task Search_ReturnsOkWithSearchResults()
    {
        var results = new List<PropertyListingSeekerResponse>
        {
            new PropertyListingSeekerResponse { Id = 5, Title = "Search House" }
        };

        _serviceMock.Setup(s => s.SearchAsync("City", 100, 1000, 1, It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(results);

        var result = await _controller.Search("City", 100, 1000, 1);

        ClassicAssert.IsInstanceOf<OkObjectResult>(result);
        var okResult = (OkObjectResult)result;
        ClassicAssert.AreEqual(results, okResult.Value);
    }
}
