using Ultimate_POS_Api.DTOS;
using Ultimate_POS_Api.DTOS.Till;

namespace Ultimate_POS_Api.Repository.TillRepo
{
    public interface ITillRepository
    {
        Task<ResponseStatus> AddTill(AddTillDto addTill);
        Task<ResponseStatus> OpenTillAsync(OpenTillDto openTillDto);
        Task<ResponseStatus> CloseTillAsync(CloseTillDto closeTillDto);
        Task<ResponseStatus> AssignTillAsync(AssignTillDto assignTillDto);
        Task<IEnumerable<GetTillDto>> GetTillAsync();

        Task<IEnumerable<GetTillDto>> GetTillsUnderReviewAsync();
        Task<ResponseStatus> UpdateTillAsync(UpdateTillDto updateTillDto);
        Task<ResponseStatus> SubmitTillClosureAsync(TillClosureDto dto);
        Task<ResponseStatus> SuperviseTillAsync(ApproveTillDto dto);
        Task<ResponseStatus> DeleteTillAsync(int tillId);
        //Task<GetTillDto> GetTill(Guid tillId);
    }
}
