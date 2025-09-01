using Ultimate_POS_Api.DTOS;
using Ultimate_POS_Api.Models;

namespace Ultimate_POS_Api.Repository
{
    public interface IMpesaRepository
    {
        public Task<ResponseStatus> MpesaExpress_Transaction(MpesaRequestListDto mpesapayload);

        public Task<ResponseStatus> MpesaC2B_Transaction(SimulateC2BRequest request);

        public Task<ResponseStatus> RegisterUrls(RegisterUrlsRequest registerUrlsRequest);

        public Task<ResponseStatus> MpesaValidaion(ValidationRequest validationrequest);





        // public Task<IEnumerable<Tokenmpesa>> GetAccessToken();
        Task<string> GetAccessToken();

    }
}



//
//   public Task<ResponseStatus> AddProducts(ProductListDto JsonData);

//     public Task<IEnumerable<Products>> GetProducts();