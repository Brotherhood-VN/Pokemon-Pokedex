using API._Services.Interfaces.Systems;
using API.Data;
using API.Dtos.Systems;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Implementations.Systems
{
    abstract public class BaseSystemService<TTable, TDto, TParam> : IBaseSystemService<TDto, TParam> where TTable : class, new()
                                                                                                     where TDto : class
    {
        private readonly DBContext _context;

        public BaseSystemService(DBContext context)
        {
            _context = context;
        }

        #region Create
        public async Task<OperationResult> Create(TDto dto)
        {
            if (await _context.Database.SqlQueryRaw<bool>("SELECT CASE WHEN COUNT(*) > 0 THEN 1 ELSE 0 END FROM").FirstOrDefaultAsync())
                return new OperationResult { IsSuccess = false, Message = "Nội tại đã tồn tại. Vui lòng thử lại !!!" };
            // if (await _context.Ability.AnyAsync(x => x.Code.Trim() == dto.Code.Trim()))
            //     return new OperationResult { IsSuccess = false, Message = "Nội tại đã tồn tại. Vui lòng thử lại !!!" };

            TTable data = (TTable)dto.Map<TTable>();

            _context.Add(data);
            try
            {
                await _context.SaveChangesAsync();
                return new OperationResult { IsSuccess = true };
            }
            catch (Exception ex)
            {
                return new OperationResult { IsSuccess = false, Message = ex.Message };
            }
        }
        #endregion

        public Task<OperationResult> Delete(TDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<PaginationUtility<TDto>> GetDataPagination(PaginationParam pagination, TParam param)
        {
            throw new NotImplementedException();
        }

        public Task<TDto> GetDetail(long id)
        {
            throw new NotImplementedException();
        }

        public Task<List<KeyValuePair<long, string>>> GetListEntities(string entity)
        {
            throw new NotImplementedException();
            // return await _context.GetType<TTable>();
            // return await _context.Ability.Where(x => x.Status == true)
            // .OrderBy(x => x.Code)
            // .ThenBy(x => x.Title)
            // .Select(x => new KeyValuePair<long, string>(x.Id, $"{x.Code} - {x.Title}"))
            // .Distinct().ToListAsync();
        }

        public Task<OperationResult> Update(TDto dto)
        {
            throw new NotImplementedException();
        }
    }
}