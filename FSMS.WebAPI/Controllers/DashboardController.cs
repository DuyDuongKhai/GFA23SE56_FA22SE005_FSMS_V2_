using FSMS.Entity.Models;
using FSMS.WebAPI.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FSMS.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private IConfiguration _configuration;
        private FruitSeasonManagementSystemV10Context _context;

        public DashboardController(IConfiguration configuration)
        {
            _configuration = configuration;
            _context = new FruitSeasonManagementSystemV10Context();
        }

        [HttpGet("totalSales")]
        [Cache(1000)]

        public async Task<ActionResult<decimal>> GetTotalSales(DateTime? startDate = null, DateTime? endDate = null)
        {
            if (!startDate.HasValue)
            {
                startDate = DateTime.Now.Date.AddDays(-7);
            }

            if (!endDate.HasValue)
            {
                endDate = DateTime.Now.Date;
            }

            var totalSales = await _context.Orders
                .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
                .Select(o => o.TotalAmount)
                .SumAsync();

            return totalSales;
        }

        [HttpGet("totalOrdersGroupByStatus")]
        public async Task<ActionResult<IEnumerable<OrderStatusCount>>> GetTotalOrdersGroupByStatus(DateTime? startDate = null, DateTime? endDate = null)
        {
            if (!startDate.HasValue)
            {
                startDate = DateTime.Now.Date.AddDays(-7);
            }

            if (!endDate.HasValue)
            {
                endDate = DateTime.Now.Date;
            }
            var orderStatusCounts = await _context.Orders
                .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
                .GroupBy(o => o.Status)
                .Select(g => new OrderStatusCount
                {
                    Status = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();

            return orderStatusCounts;
        }
        public class OrderStatusCount
        {
            public string Status { get; set; }
            public int Count { get; set; }
        }

        [HttpGet("totalOrders")]
        [Cache(1000)]

        public async Task<ActionResult<int>> GetTotalOrders(DateTime? startDate = null, DateTime? endDate = null)
        {
            if (!startDate.HasValue)
            {
                startDate = DateTime.Now.Date.AddDays(-7);
            }

            if (!endDate.HasValue)
            {
                endDate = DateTime.Now.Date;
            }
            var totalOrders = await _context.Orders
                .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
                .CountAsync();

            return totalOrders;
        }

        [HttpGet("newCustomers")]
        [Cache(1000)]

        public async Task<ActionResult<int>> GetNewCustomers(DateTime? startDate = null, DateTime? endDate = null)
        {
            if (!startDate.HasValue)
            {
                startDate = DateTime.Now.Date.AddDays(-7);
            }

            if (!endDate.HasValue)
            {
                endDate = DateTime.Now.Date;
            }

            var newCustomers = await _context.Users
                .Where(o => o.CreatedDate >= startDate && o.CreatedDate <= endDate)
                .Select(o => o.UserId)
                .Distinct()
                .Where(customerId => !_context.Users
                    .Where(o => o.CreatedDate < startDate)
                    .Select(o => o.UserId)
                    .Contains(customerId))
                .CountAsync();

            return newCustomers;
        }

        [HttpGet("top10SellingProducts")]
        [Cache(1000)]

        public async Task<ActionResult<IEnumerable<ProductSale>>> GetTop10SellingProducts(DateTime? startDate = null, DateTime? endDate = null)
        {
            if (!startDate.HasValue)
            {
                startDate = DateTime.Now.Date.AddDays(-7);
            }

            if (!endDate.HasValue)
            {
                endDate = DateTime.Now.Date;
            }
            var top10SellingProducts = await _context.OrderDetails
                .Where(oi => oi.CreatedDate >= startDate && oi.CreatedDate <= endDate)
                .GroupBy(oi => oi.FruitId)
                .Select(g => new ProductSale
                {
                    ProductId = g.Key,
                    ProductName = g.FirstOrDefault().Fruit.FruitName,
                    QuantitySold = g.Sum(oi => oi.Quantity),
                    QuantityAvailable = g.FirstOrDefault().Fruit.QuantityAvailable,
                    Price = g.FirstOrDefault().Fruit.Price,
                })
                .OrderByDescending(p => p.QuantitySold)
                .Take(10)
                .ToListAsync();
            return top10SellingProducts;
        }
        [HttpGet("totalPosts")]
        [Cache(1000)]

        public async Task<ActionResult<int>> GetTotalPosts(DateTime? startDate = null, DateTime? endDate = null)
        {
            if (!startDate.HasValue)
            {
                startDate = DateTime.Now.Date.AddDays(-7);
            }

            if (!endDate.HasValue)
            {
                endDate = DateTime.Now.Date;
            }
            var totalPosts = await _context.Posts
                .Where(post => post.CreatedDate >= startDate && post.CreatedDate <= endDate)
                .CountAsync();

            return totalPosts;
        }
        [HttpGet("badRatingComments")]
        [Cache(1000)]

        public async Task<ActionResult<IEnumerable<BadRatingComment>>> GetBadRatingComments(DateTime? startDate = null, DateTime? endDate = null)
        {
            if (!startDate.HasValue)
            {
                startDate = DateTime.Now.Date.AddDays(-7);
            }

            if (!endDate.HasValue)
            {
                endDate = DateTime.Now.Date;
            }
            var badRatingComments = await _context.ReviewFruits
                .Join(_context.Fruits, rf => rf.FruitId, f => f.FruitId, (rf, f) => new
                {
                    ReviewFruitId = rf.ReviewId,
                    FruitId = f.FruitId,
                    FruitName = f.FruitName,
                    Rating = rf.Rating,
                    ReviewComment = rf.ReviewComment,
                    ReviewImageUrl = rf.ReviewImageUrl,
                    CreatedDate = rf.CreatedDate
                })
                .Where(rf => rf.Rating <= 3 && rf.CreatedDate >= startDate && rf.CreatedDate <= endDate)
                .Select(rf => new BadRatingComment
                {
                    ReviewFruitId = rf.ReviewFruitId,
                    FruitId = rf.FruitId,
                    FruitName = rf.FruitName,
                    Rating = rf.Rating,
                    ReviewComment = rf.ReviewComment,
                    ReviewImageUrl = rf.ReviewImageUrl,
                    CreatedDate = rf.CreatedDate
                })
                .OrderByDescending(rf => rf.Rating)
                .ToListAsync();

            return badRatingComments;
        }
        [HttpGet("totalPlants")]
        [Cache(1000)]

        public async Task<ActionResult<int>> GetTotalPlants()
        {
            var totalPlants = await _context.Plants.CountAsync();

            return totalPlants;
        }
        [HttpGet("plantsPlantedToday")]
        [Cache(1000)]

        public async Task<ActionResult<IEnumerable<Plant>>> GetPlantsPlantedToday()
        {
            var plantsPlantedToday = await _context.Plants
                .Where(p => p.PlantingDate == DateTime.Today)
                .ToListAsync();

            return plantsPlantedToday;
        }
        [HttpGet("plantsDueForHarvestToday")]
        [Cache(1000)]

        public async Task<ActionResult<IEnumerable<Plant>>> GetPlantsDueForHarvestToday()
        {
            var plantsDueForHarvestToday = await _context.Plants
                .Where(p => p.HarvestingDate == DateTime.Today)
                .ToListAsync();

            return plantsDueForHarvestToday;
        }
        [HttpGet("totalGardens")]
        [Cache(1000)]

        public async Task<ActionResult<int>> GetTotalGardens()
        {
            var totalGardens = await _context.Gardens.CountAsync();

            return totalGardens;
        }
        [HttpGet("gardensCreatedToday")]
        [Cache(1000)]

        public async Task<ActionResult<IEnumerable<Garden>>> GetGardensCreatedToday()
        {
            var gardensCreatedToday = await _context.Gardens
                .Where(g => g.CreatedDate == DateTime.Today)
                .ToListAsync();

            return gardensCreatedToday;
        }
        [HttpGet("gardensWithHighPlantingQuantity")]
        [Cache(1000)]

        public async Task<ActionResult<IEnumerable<Garden>>> GetGardensWithHighPlantingQuantity()
        {
            var gardensWithHighPlantingQuantity = await _context.Gardens
                .Where(g => g.QuantityPlanted > 50)
                .ToListAsync();

            return gardensWithHighPlantingQuantity;
        }
        [HttpGet("gardensWithActivePlants")]
        [Cache(1000)]

        public async Task<ActionResult<IEnumerable<Garden>>> GetGardensWithActivePlants()
        {
            var gardensWithActivePlants = await _context.Gardens
                .Join(_context.Plants, g => g.GardenId, p => p.PlantId, (g, p) => new { Garden = g, Plant = p })
                .Where(gp => gp.Plant.Status == "Active")
                .Select(gp => gp.Garden)
                .ToListAsync();

            return gardensWithActivePlants;
        }
        public class BadRatingComment
        {
            public int ReviewFruitId { get; set; }
            public int FruitId { get; set; }
            public string FruitName { get; set; }
            public decimal Rating { get; set; }
            public string ReviewComment { get; set; }
            public string ReviewImageUrl { get; set; }
            public DateTime CreatedDate { get; set; }
        }
        public class ProductSale
        {
            public int ProductId { get; set; }
            public string ProductName { get; set; }
            public double QuantitySold { get; set; }
            public double QuantityAvailable { get; set; }
            public decimal Price { get; set; }
        }
    }
}
