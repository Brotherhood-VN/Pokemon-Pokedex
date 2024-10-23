namespace API._Services.Interfaces.Systems
{
    public interface IBaseSystemService<TDto, TParam> where TDto : class
    {
        Task<OperationResult> Create(TDto dto);
        Task<OperationResult> Update(TDto dto);
        Task<OperationResult> Delete(TDto dto);
        Task<PaginationUtility<TDto>> GetDataPagination(PaginationParam pagination, TParam param);
        Task<List<KeyValuePair<long, string>>> GetListEntities(string entity);
        Task<TDto> GetDetail(long id);
    }
}