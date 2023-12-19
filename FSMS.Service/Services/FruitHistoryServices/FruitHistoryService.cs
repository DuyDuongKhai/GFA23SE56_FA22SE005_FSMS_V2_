using AutoMapper;
using FSMS.Entity.Models;
using FSMS.Entity.Repositories.FruitHistoryRepositories;
using FSMS.Service.Enums;
using FSMS.Service.ViewModels.FruitHistories;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace FSMS.Service.Services.FruitHistoryServices
{
    public class FruitHistoryService : IFruitHistoryService
    {
        private IFruitHistoryRepository _fruitHistoryRepository;
        private IMapper _mapper;
        public FruitHistoryService(IFruitHistoryRepository fruitHistoryRepository, IMapper mapper)
        {
            _fruitHistoryRepository = fruitHistoryRepository;
            _mapper = mapper;
        }
        public async Task<List<GetFruitHistory>> GetAllAsync(string? location = null, DateTime? createdDate = null, int userId = 0)
        {
            try
            {
                IEnumerable<FSMS.Entity.Models.FruitHistory> fruitHistories = await _fruitHistoryRepository.GetAsync(includeProperties: "User");

                var filteredFruitHistories = fruitHistories
                    .Where(fruitHistory =>
                        (string.IsNullOrEmpty(location) || fruitHistory.Location.Contains(location)) &&
                        (!createdDate.HasValue || fruitHistory.CreatedDate.Date >= createdDate.Value.Date) &&
                        (userId == 0 || fruitHistory.UserId == userId)
                    )
                    .ToList();

                var result = filteredFruitHistories
                    .Select(FruitHistory => _mapper.Map<GetFruitHistory>(FruitHistory))
                    .ToList();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while fetching fruit histories.", ex);
            }
        }





        public async Task ScrapeFarmerMarketAndSaveToDatabaseAsync(int userId)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    string url = "https://farmersmarket.vn/collections/trai-cay-viet-nam";
                    HttpResponseMessage response = await httpClient.GetAsync(url);
                    string htmlContent = await response.Content.ReadAsStringAsync();

                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(htmlContent);

                    var productNodes = doc.DocumentNode.SelectNodes("//div[contains(@class, 'product-col')]");

                    // Create a HashSet to store scraped product names
                    HashSet<string> scrapedProductNames = new HashSet<string>();

                    foreach (var productNode in productNodes)
                    {
                        HtmlNode productNameNode = productNode.SelectSingleNode(".//h3[@class='product-name']/a");
                        string productName = productNameNode != null ? productNameNode.InnerText.Trim() : string.Empty;

                        if (scrapedProductNames.Contains(productName))
                        {
                            continue;
                        }

                        HtmlNode priceNode = productNode.SelectSingleNode(".//div[contains(@class,'price')]");
                        string priceText = priceNode != null ? priceNode.InnerText.Trim() : string.Empty;
                        decimal price;

                        if (decimal.TryParse(priceText.Replace("₫", "").Replace(",", ""), out price))
                        {
                            string location = "Farmers Market";
                            string status = StatusEnums.Active.ToString();
                            DateTime createdDate = DateTime.Now;

                            FruitHistory fruitHistory = new FruitHistory()
                            {
                                UserId = userId,
                                FruitName = productName,
                                Price = price,
                                Location = location,
                                Status = status,
                                CreatedDate = createdDate,
                            };

                            await _fruitHistoryRepository.InsertAsync(fruitHistory);

                            scrapedProductNames.Add(productName);
                        }
                        else
                        {
                        }
                    }

                    await _fruitHistoryRepository.CommitAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task ScrapeThucPhamNhanhAndSaveToDatabaseAsync(int userId)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    string url = "https://thucphamnhanh.com/trai-cay/";
                    HttpResponseMessage response = await httpClient.GetAsync(url);
                    string htmlContent = await response.Content.ReadAsStringAsync();

                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(htmlContent);

                    var productNodes = doc.DocumentNode.SelectNodes("//div[contains(@class, 'product')]");

                    // Create a set to store scraped product names
                    var scrapedProducts = new HashSet<string>();

                    foreach (var productNode in productNodes)
                    {
                        HtmlNode productNameNode = productNode.SelectSingleNode(".//p[@class='name product-title woocommerce-loop-product__title']/a");
                        string productName = productNameNode != null ? productNameNode.InnerText.Trim() : string.Empty;

                        // Check if the product name has already been scraped
                        if (scrapedProducts.Contains(productName))
                        {
                            continue;
                        }

                        HtmlNode priceNode = productNode.SelectSingleNode(".//span[@class='price']");
                        string priceText = priceNode != null ? priceNode.InnerText.Trim() : string.Empty;
                        decimal price = 0;

                        string numericPriceText = Regex.Replace(priceText, "[^0-9.]", "");

                        if (decimal.TryParse(numericPriceText, out price))
                        {
                            string location = "Thực Phẩm Nhanh";
                            string status = StatusEnums.Active.ToString();
                            DateTime createdDate = DateTime.Now;

                            FruitHistory fruitHistory = new FruitHistory()
                            {
                                UserId = userId,
                                FruitName = productName,
                                Price = price,
                                Location = location,
                                Status = status,
                                CreatedDate = createdDate,
                            };

                            await _fruitHistoryRepository.InsertAsync(fruitHistory);

                            scrapedProducts.Add(productName);
                        }
                        else
                        {
                        }
                    }

                    await _fruitHistoryRepository.CommitAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while saving data to the database.", ex);
            }
        }
        public async Task DeleteFruitHistoryAsync(int userId)
        {
            try
            {
                var fruitHistoriesToDelete = await _fruitHistoryRepository.GetAsync(fruitHistory => fruitHistory.UserId == userId);
                foreach (var fruitHistory in fruitHistoriesToDelete)
                {
                    await _fruitHistoryRepository.DeleteAsync(fruitHistory);
                }

                await _fruitHistoryRepository.CommitAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting fruit histories.", ex);
            }
        }




    }
}
