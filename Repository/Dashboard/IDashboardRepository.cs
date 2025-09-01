using Ultimate_POS_Api.DTOS;
using Ultimate_POS_Api.Models;

namespace Ultimate_POS_Api.Repository
{
    public interface IDashboardRepository
    {
        public Task GetAllAverages();

        public Task GetGraphData();


        //   public Task<ResponseStatus> AddCategory(CategoryListDto JsonData);

        // public Task<IEnumerable<Products>> GetAllAverages();

    }
}