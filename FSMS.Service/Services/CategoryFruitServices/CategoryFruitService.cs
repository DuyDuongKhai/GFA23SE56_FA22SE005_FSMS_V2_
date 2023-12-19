using AutoMapper;
using FSMS.Entity.Models;
using FSMS.Entity.Repositories.CategoryFruitRepositories;
using FSMS.Service.Enums;
using FSMS.Service.ViewModels.CategoryFruits;

namespace FSMS.Service.Services.CategoryFruitServices
{
    public class CategoryFruitService : ICategoryFruitService
    {
        private ICategoryFruitRepository _categoryFruitRepository;
        private IMapper _mapper;
        public CategoryFruitService(ICategoryFruitRepository categoryFruitRepository, IMapper mapper)
        {
            _categoryFruitRepository = categoryFruitRepository;
            _mapper = mapper;
        }

        public async Task CreateCategoryFruitAsync(CreateCategoryFruit createCategoryFruit)
        {
            try
            {
                int lastId = (await _categoryFruitRepository.GetAsync()).Max(x => x.CategoryFruitId);
                CategoryFruit categoryFruit = new CategoryFruit()
                {
                    CategoryFruitName = createCategoryFruit.CategoryFruitName,
                    Status = StatusEnums.Active.ToString(),
                    CreatedDate = DateTime.Now,
                    CategoryFruitId = lastId + 1
                };

                await _categoryFruitRepository.InsertAsync(categoryFruit);
                await _categoryFruitRepository.CommitAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteCategoryFruitAsync(int key)
        {
            try
            {
                CategoryFruit existedCategoryFruit = await _categoryFruitRepository.GetByIDAsync(key);

                if (existedCategoryFruit == null)
                {
                    throw new Exception("CategoryFruit ID does not exist in the system.");
                }

                if (existedCategoryFruit.Status != StatusEnums.Active.ToString())
                {
                    throw new Exception("Category Fruit is not active.");
                }
                existedCategoryFruit.Status = StatusEnums.InActive.ToString();

                await _categoryFruitRepository.UpdateAsync(existedCategoryFruit);
                await _categoryFruitRepository.CommitAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }




        public async Task<GetCategoryFruit> GetAsync(int key)
        {
            try
            {
                CategoryFruit categoryFruit = await _categoryFruitRepository.GetByIDAsync(key);

                if (categoryFruit == null)
                {
                    throw new Exception("Category Fruit ID does not exist in the system.");
                }
                if (categoryFruit.Status != StatusEnums.Active.ToString())
                {
                    throw new Exception("Category Fruit is not active.");
                }

                GetCategoryFruit result = _mapper.Map<GetCategoryFruit>(categoryFruit);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<List<GetCategoryFruit>> GetAllAsync(string? categoryName = null, bool activeOnly = false)
        {
            try
            {
                IEnumerable<CategoryFruit> categoryFruits = await _categoryFruitRepository.GetAsync();

                if (activeOnly)
                {
                    categoryFruits = categoryFruits.Where(categoryProduct => categoryProduct.Status == StatusEnums.Active.ToString());
                }

                if (!string.IsNullOrEmpty(categoryName))
                {
                    categoryFruits = categoryFruits.Where(categoryProduct => categoryProduct.CategoryFruitName == categoryName);
                }

                List<GetCategoryFruit> result = categoryFruits.Select(categoryProduct => _mapper.Map<GetCategoryFruit>(categoryProduct)).ToList();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while fetching category products.", ex);
            }
        }

        public async Task UpdateCategoryFruitAsync(int key, UpdateCategoryFruit updateCategoryFruit)
        {
            try
            {
                CategoryFruit existedCategoryFruit = await _categoryFruitRepository.GetByIDAsync(key);

                if (existedCategoryFruit == null)
                {
                    throw new Exception("CategoryFruit ID does not exist in the system.");
                }

                if (!string.IsNullOrEmpty(updateCategoryFruit.CategoryFruitName))
                {
                    existedCategoryFruit.CategoryFruitName = updateCategoryFruit.CategoryFruitName;
                }
                if (!string.IsNullOrEmpty(updateCategoryFruit.Status))
                {
                    if (updateCategoryFruit.Status != "Active" && updateCategoryFruit.Status != "InActive")
                    {
                        throw new Exception("Status must be 'Active' or 'InActive'.");
                    }
                    existedCategoryFruit.Status = updateCategoryFruit.Status;
                }

                existedCategoryFruit.UpdateDate = DateTime.Now;


                await _categoryFruitRepository.UpdateAsync(existedCategoryFruit);
                await _categoryFruitRepository.CommitAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
