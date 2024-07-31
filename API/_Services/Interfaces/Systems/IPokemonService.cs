using API.Dtos.Systems;
using API.Helpers.Attributes;

namespace API._Services.Interfaces.Systems
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface IPokemonService
    {
        Task<OperationResult> Create(PokemonDto dto);
        Task<OperationResult> Update(PokemonDto dto);
        Task<OperationResult> Delete(PokemonDto dto);
        Task<PaginationUtility<PokemonDto>> GetDataPagination(PaginationParam pagination, string keyword);
        Task<List<KeyValuePair<long, string>>> GetListPokemon();
        Task<PokemonDto> GetDetail(long id);
    }
}