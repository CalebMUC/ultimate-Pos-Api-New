using Ultimate_POS_Api.DTOS;
using Ultimate_POS_Api.Models;

namespace Ultimate_POS_Api.Repository
{
    public interface ISuppliesRepository
    {

        public Task<ResponseStatus> AddSuplies(SuppliesDetailsDTO JsonData);


        public Task<IEnumerable<Supplies>> GetSupplies();

    }
}