using Ultimate_POS_Api.DTOS;
using Ultimate_POS_Api.Models;

namespace Ultimate_POS_Api.Repository
{
    public interface ICategoryRepository
    {
        public Task<IEnumerable<Categories>> GetCategory();

        public Task<ResponseStatus> AddCategory(CategoryDTOs JsonData);
        public Task<ResponseStatus> EditCategoriesAsync(EditCategoryDTO editCategoryDTO);

    }
}