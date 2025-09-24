using Ultimate_POS_Api.DTOS;
using Ultimate_POS_Api.DTOS.Purchases;

namespace Ultimate_POS_Api.Repository.Purchases
{
    public interface IPurchaseRepo
    {
        //public Task<GetPurchaseOrderItems> GetPurchaseOrderItemsAsync();
        public Task<List<GetPurchaseOrder>> GetAllPurchaseOrdersAsync();
        public Task<ResponseStatus> AddPurchaseOrderAsync(AddPurchaseOrderDto dto);
        public Task<ResponseStatus> EditPurchaseOrderAsync(EditPurchaseOrder dto);    
        public Task<ResponseStatus> DeletePurchaseOrderAsync(Guid purchaseOrderId);
    }
}
