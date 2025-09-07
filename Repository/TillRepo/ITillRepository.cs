using Ultimate_POS_Api.DTOS;
using Ultimate_POS_Api.DTOS.Till;

namespace Ultimate_POS_Api.Repository.TillRepo
{
    public interface ITillRepository
    {
        Task<ResponseStatus> AddTill(AddTillDto addTill);
        Task<ResponseStatus> OpenTillAsync(OpenTillDto openTillDto);
        Task<IEnumerable<GetTillDto>> GetTillAsync();
        Task<ResponseStatus> UpdateTillAsync(UpdateTillDto updateTillDto);
        Task<ResponseStatus> DeleteTillAsync(int tillId);
        //Task<GetTillDto> GetTill(Guid tillId);
    }
}
