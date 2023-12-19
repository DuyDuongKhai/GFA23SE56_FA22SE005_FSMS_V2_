using AutoMapper;
using FSMS.Entity.Models;
using FSMS.Entity.Repositories.CategoryFruitRepositories;
using FSMS.Entity.Repositories.FruitRepositories;
using FSMS.Entity.Repositories.OrderDetailRepositories;
using FSMS.Entity.Repositories.OrderRepositories;
using FSMS.Entity.Repositories.PaymentRepositories;
using FSMS.Entity.Repositories.PlantRepositories;
using FSMS.Entity.Repositories.UserRepositories;
using FSMS.Service.Enums;
using FSMS.Service.Services.FileServices;
using FSMS.Service.ViewModels.Fruits;

namespace FSMS.Service.Services.FruitServices
{
    public class FruitService : IFruitService
    {
        private IFruitRepository _fruitRepository;
        private ICategoryFruitRepository _categoryFruitRepository;
        private IPlantRepository _plantRepository;
        private IUserRepository _userRepository;
        private IPaymentRepository _paymentRepository;
        private IOrderRepository _orderRepository;
        private IOrderDetailRepository _orderDetailRepository;
        private IMapper _mapper;
        private IFileService _fileService;
        public FruitService(IFruitRepository fruitRepository, IMapper mapper, ICategoryFruitRepository categoryFruitRepository, IPlantRepository plantRepository,
            IUserRepository userRepository, IPaymentRepository paymentRepository, IOrderRepository orderRepository,
            IOrderDetailRepository orderDetailRepository, IFileService fileService)
        {
            _fruitRepository = fruitRepository;
            _mapper = mapper;
            _categoryFruitRepository = categoryFruitRepository;
            _plantRepository = plantRepository;
            _userRepository = userRepository;
            _paymentRepository = paymentRepository;
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _fileService = fileService;
        }

        public async Task CreateFruitFarmerAsync(CreateFruitFarmer createFruitFarmer)
        {
            try
            {

                User existedUser = (await _userRepository.GetByIDAsync(createFruitFarmer.UserId));
                if (existedUser == null)
                {
                    throw new Exception("User Id does not exist in the system.");
                }

                CategoryFruit existedCategoryFruit = (await _categoryFruitRepository.GetByIDAsync(createFruitFarmer.CategoryFruitId));
                if (existedCategoryFruit == null)
                {
                    throw new Exception("CategoryFruit Id does not exist in the system.");
                }


                Plant existedPlant = (await _plantRepository.GetByIDAsync(createFruitFarmer.PlantId));
                if (existedPlant == null)
                {
                    throw new Exception("Plant Id does not exist in the system.");
                }
                if (existedPlant.Status != PlantEnum.Harvestable.ToString())
                {
                    throw new Exception("The specified CropId is not in 'Harvestable' status.");
                }
                if (createFruitFarmer.QuantityAvailable >= existedPlant.EstimatedHarvestQuantity)
                {
                    throw new Exception("QuantityAvailable must be less than or equal to Estimated Harvest Quantity.");
                }
                if (createFruitFarmer.QuantityInTransit >= createFruitFarmer.QuantityAvailable)
                {
                    throw new Exception("QuantityInTrasit must be less than or equal to Quantity Available Quantity.");
                }
                /* if (createProductFarmer.UploadFiles == null)
                 {
                     throw new Exception("Image of product is not null ");
                 }*/
                if (createFruitFarmer.PlantId != null)
                {
                    int lastId = (await _fruitRepository.GetAsync()).Max(x => x.FruitId);
                    Fruit fruit = new Fruit()
                    {
                        FruitName = createFruitFarmer.FruitName,
                        FruitDescription = createFruitFarmer.ProductDescription,
                        Price = createFruitFarmer.Price,
                        QuantityAvailable = createFruitFarmer.QuantityAvailable,
                        QuantityInTransit = createFruitFarmer.QuantityInTransit,
                        OriginCity = createFruitFarmer.OriginCity,
                        OrderType = createFruitFarmer.OrderType,
                        CategoryFruitId = createFruitFarmer.CategoryFruitId,
                        UserId = createFruitFarmer.UserId,
                        PlantId = createFruitFarmer.PlantId,
                        Status = StatusEnums.Active.ToString(),
                        CreatedDate = DateTime.Now,
                        FruitId = lastId + 1

                    };


                    await _fruitRepository.InsertAsync(fruit);
                    await _fruitRepository.CommitAsync();
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task DeleteFruitFarmerAsync(int key)
        {
            try
            {
                Fruit existedFruit = await _fruitRepository.GetByIDAsync(key);

                if (existedFruit == null)
                {
                    throw new Exception("Fruit ID does not exist in the system.");
                }
                if (existedFruit.Status != StatusEnums.Active.ToString())
                {
                    throw new Exception("Product is not active.");
                }
                if (existedFruit.PlantId != null)
                {
                    existedFruit.Status = StatusEnums.InActive.ToString();
                    await _fruitRepository.UpdateAsync(existedFruit);
                    await _fruitRepository.CommitAsync();
                }
                else
                {
                    throw new Exception("Farmers have no products and cannot be deleted.");
                }


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public async Task<GetFruitFarmer> GetFruitFarmerAsync(int key)
        {
            try
            {
                Fruit fruitFarmer = await _fruitRepository.GetProductByIDAsync(key);

                if (fruitFarmer == null)
                {
                    throw new Exception("Fruit ID does not exist in the system.");
                }
                if (fruitFarmer.Status != StatusEnums.Active.ToString())
                {
                    throw new Exception("Fruit is not active.");
                }
                if (fruitFarmer.PlantId != null)
                {


                    List<GetFruitFarmer> fruits = _mapper.Map<List<GetFruitFarmer>>(
                    await _fruitRepository.GetAllProductAsync(filter: p => p.PlantId != null, includeProperties: "Plant,CategoryFruit,User"));

                    GetFruitFarmer result = _mapper.Map<GetFruitFarmer>(fruitFarmer);
                    return result;
                }
                else
                {
                    throw new Exception("Farmers have no fruits.");
                }


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<List<GetFruitFarmer>> GetAllFruitFarmerAsync(string? fruitName = null, decimal? minPrice = null, decimal? maxPrice = null, bool activeOnly = false, DateTime? createdDate = null, DateTime? newestDate = null, int? userId = null)
        {
            try
            {
                var fruits = (await _fruitRepository.GetAllProductAsync(
                    filter: fruit =>
                        (string.IsNullOrEmpty(fruitName) || fruit.FruitName.Contains(fruitName)) &&
                        (!minPrice.HasValue || fruit.Price >= minPrice) &&
                        (!maxPrice.HasValue || fruit.Price <= maxPrice) &&
                        (!activeOnly || fruit.Status == StatusEnums.Active.ToString()) &&
                        (!createdDate.HasValue || fruit.CreatedDate.Date >= createdDate.Value.Date) &&
                        (!newestDate.HasValue || fruit.CreatedDate <= newestDate) &&
                        (!userId.HasValue || fruit.UserId == userId),
                    includeProperties: "Plant,CategoryFruit,User")).Where(product => product.PlantId != null);

                List<GetFruitFarmer> fruitFarmers = _mapper.Map<List<GetFruitFarmer>>(fruits);

                return fruitFarmers;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }




        public async Task UpdateFruitFarmerAsync(int key, UpdateFruitFarmer updateFruitFarmer)
        {
            try
            {
                Fruit existedFruitFarmer = await _fruitRepository.GetByIDAsync(key);

                if (existedFruitFarmer == null)
                {
                    throw new Exception("Fruit ID does not exist in the system.");
                }
                Plant plant = await _plantRepository.GetByIDAsync(existedFruitFarmer.PlantId);
                if (existedFruitFarmer.PlantId != null)
                {
                    if (!string.IsNullOrEmpty(updateFruitFarmer.FruitName))
                    {
                        existedFruitFarmer.FruitName = updateFruitFarmer.FruitName;
                    }

                    if (!string.IsNullOrEmpty(updateFruitFarmer.FruitDescription))
                    {
                        existedFruitFarmer.FruitDescription = updateFruitFarmer.FruitDescription;
                    }

                    existedFruitFarmer.Price = updateFruitFarmer.Price;
                    existedFruitFarmer.QuantityAvailable = updateFruitFarmer.QuantityAvailable;
                    existedFruitFarmer.QuantityInTransit = updateFruitFarmer.QuantityInTransit;
                    existedFruitFarmer.OriginCity = updateFruitFarmer.OriginCity;
                    existedFruitFarmer.OrderType = updateFruitFarmer.OrderType;

                    if (!string.IsNullOrEmpty(updateFruitFarmer.Status))
                    {
                        if (updateFruitFarmer.Status != "Active" && updateFruitFarmer.Status != "InActive")
                        {
                            throw new Exception("Status must be 'Active' or 'InActive'.");
                        }
                        existedFruitFarmer.Status = updateFruitFarmer.Status;
                    }
                    existedFruitFarmer.UpdateDate = DateTime.Now;

                    await _fruitRepository.UpdateAsync(existedFruitFarmer);
                    await _fruitRepository.CommitAsync();
                }
                else
                {
                    throw new Exception("Fruit does not have a CropId and cannot be updated.");
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> CreateFruitSupplierAsync(CreateFruitSupplier createFruitSupplier)
        {
            try
            {
                User existedUser = await _userRepository.GetByIDAsync(createFruitSupplier.UserId);
                if (existedUser == null)
                {
                    throw new Exception("User Id does not exist in the system.");
                }

                CategoryFruit existedCategoryFruit = await _categoryFruitRepository.GetByIDAsync(createFruitSupplier.CategoryFruitId);
                if (existedCategoryFruit == null)
                {
                    throw new Exception("CategoryFruit Id does not exist in the system.");
                }
                if (createFruitSupplier.QuantityInTransit >= createFruitSupplier.QuantityAvailable)
                {
                    throw new Exception("QuantityInTrasit must be less than or equal to Quantity Available Quantity.");
                }

                int lastId = (await _fruitRepository.GetAsync()).Max(x => x.FruitId);
                Fruit fruit = new Fruit()
                {
                    FruitName = createFruitSupplier.FruitName,
                    FruitDescription = createFruitSupplier.ProductDescription,
                    Price = createFruitSupplier.Price,
                    QuantityAvailable = createFruitSupplier.QuantityAvailable,
                    QuantityInTransit = createFruitSupplier.QuantityInTransit,
                    OriginCity = createFruitSupplier.OriginCity,
                    OrderType = createFruitSupplier.OrderType,
                    CategoryFruitId = createFruitSupplier.CategoryFruitId,
                    UserId = createFruitSupplier.UserId,
                    Status = StatusEnums.Active.ToString(),
                    CreatedDate = DateTime.Now,
                    FruitId = lastId + 1
                };

                await _fruitRepository.InsertAsync(fruit);
                await _fruitRepository.CommitAsync();

                return fruit.FruitId;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteFruitSupplierAsync(int key)
        {
            try
            {
                Fruit existedFruit = await _fruitRepository.GetByIDAsync(key);

                if (existedFruit == null)
                {
                    throw new Exception("Fruit ID does not exist in the system.");
                }
                if (existedFruit.Status != StatusEnums.Active.ToString())
                {
                    throw new Exception("Fruit is not active.");
                }
                if (existedFruit.PlantId == null)
                {
                    existedFruit.Status = StatusEnums.InActive.ToString();
                    await _fruitRepository.UpdateAsync(existedFruit);
                    await _fruitRepository.CommitAsync();
                }
                else
                {
                    throw new Exception("Fruit cannot be deleted because it has a CropId.");
                }


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public async Task<GetFruitSupplier> GetFruitSupplierAsync(int key)
        {
            try
            {
                Fruit fruitSupplier = await _fruitRepository.GetProductByIDAsync(key);

                if (fruitSupplier == null)
                {
                    throw new Exception("Fruit ID does not exist in the system.");
                }
                if (fruitSupplier.Status != StatusEnums.Active.ToString())
                {
                    throw new Exception("Fruit is not active.");
                }
                if (fruitSupplier.PlantId == null)
                {


                    List<GetFruitSupplier> fruits = _mapper.Map<List<GetFruitSupplier>>(
                    await _fruitRepository.GetAllProductAsync(filter: p => p.PlantId == null, includeProperties: "CategoryFruit,User"));

                    GetFruitSupplier result = _mapper.Map<GetFruitSupplier>(fruitSupplier);
                    return result;
                }
                else
                {
                    throw new Exception("Fruit has a CropId and cannot be retrieved.");
                }


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<List<GetFruitSupplier>> GetAllFruitSupplierAsync(string? fruitName = null, decimal? minPrice = null, decimal? maxPrice = null, bool activeOnly = false, DateTime? createdDate = null, DateTime? newestDate = null, int? userId = null)
        {
            try
            {
                var fruits = (await _fruitRepository.GetAllProductAsync(
                    filter: fruit =>
                        (string.IsNullOrEmpty(fruitName) || fruit.FruitName.Contains(fruitName)) &&
                        (!minPrice.HasValue || fruit.Price >= minPrice) &&
                        (!maxPrice.HasValue || fruit.Price <= maxPrice) &&
                        (!activeOnly || fruit.Status == StatusEnums.Active.ToString()) &&
                        (!createdDate.HasValue || fruit.CreatedDate.Date >= createdDate.Value.Date) &&
                        (!newestDate.HasValue || fruit.CreatedDate <= newestDate) &&
                        (!userId.HasValue || fruit.UserId == userId),
                    includeProperties: "CategoryFruit,User")).Where(product => product.PlantId == null);

                List<GetFruitSupplier> fruitSuppliers = _mapper.Map<List<GetFruitSupplier>>(fruits);

                return fruitSuppliers;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }





        public async Task UpdateFruitSupplierAsync(int key, UpdateFruitSupplier updateFruitSupplier)
        {
            try
            {
                Fruit existedFruitSupplier = await _fruitRepository.GetByIDAsync(key);

                if (existedFruitSupplier == null)
                {
                    throw new Exception("Fruit ID does not exist in the system.");
                }

                if (!string.IsNullOrEmpty(updateFruitSupplier.FruitName))
                {
                    existedFruitSupplier.FruitName = updateFruitSupplier.FruitName;
                }

                if (!string.IsNullOrEmpty(updateFruitSupplier.FruitDescription))
                {
                    existedFruitSupplier.FruitDescription = updateFruitSupplier.FruitDescription;
                }

                existedFruitSupplier.Price = updateFruitSupplier.Price;
                existedFruitSupplier.QuantityAvailable = updateFruitSupplier.QuantityAvailable;
                existedFruitSupplier.QuantityInTransit = updateFruitSupplier.QuantityInTransit;
                existedFruitSupplier.OriginCity = updateFruitSupplier.OriginCity;
                existedFruitSupplier.OrderType = updateFruitSupplier.OrderType;

                if (!string.IsNullOrEmpty(updateFruitSupplier.Status))
                {
                    if (existedFruitSupplier.Status != "Active" && updateFruitSupplier.Status != "InActive")
                    {
                        throw new Exception("Status must be 'Active' or 'InActive'.");
                    }
                    existedFruitSupplier.Status = updateFruitSupplier.Status;
                }
                existedFruitSupplier.UpdateDate = DateTime.Now;

                await _fruitRepository.UpdateAsync(existedFruitSupplier);
                await _fruitRepository.CommitAsync();


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        //private async Task<double> GetTotalPurchasedProductsBySupplierAsync(double supplierUserId)
        //{

        //    var userOrders = await _orderRepository.GetAsync(
        //        filter: uo => uo.UserId == supplierUserId
        //    );

        //    var orderIds = userOrders.Select(uo => uo.OrderId).ToList();

        //    var orderDetails = await _orderDetailRepository.GetAsync(
        //        filter: od => orderIds.Contains(od.OrderId)
        //    );

        //    double totalPurchasedProductsFromOrderDetails = orderDetails.Sum(od => od.Quantity);

        //    return totalPurchasedProductsFromOrderDetails;
        //}
    }
}
