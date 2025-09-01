using Ultimate_POS_Api.DTOS;
using Ultimate_POS_Api.Models;

namespace Ultimate_POS_Api.Repository
{
    public interface ISuppliersRepository
    {

        public Task<IEnumerable<Supplier>> GetSupplier();

        public Task<ResponseStatus> AddSuplier(SuppliersDetailsDTO JsonData);
        public Task<ResponseStatus> EditSuplierAsync(SuppliersDetailsDTO suppliers);
    }
}