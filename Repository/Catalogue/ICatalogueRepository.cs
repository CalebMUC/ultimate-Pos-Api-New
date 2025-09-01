using Ultimate_POS_Api.DTOS;
using Ultimate_POS_Api.Models;

namespace Ultimate_POS_Api.Repository
{
    public interface ICatalogueRepository
    {
        public Task<IEnumerable<ProductCatalogueDTO>> GetCatelogue();

        public Task<IEnumerable<ProductCatalogueDTO>> GetActiveCatelogue();

        public Task<ResponseStatus> AddCatalog(CatalogueListDTO JsonData);

        


        // public Task<ResponseStatus> UploadCatalogue(List<CatalogueDTOs> catalogueDTs);//CatalogueListDTOs JsonData);


    }
}