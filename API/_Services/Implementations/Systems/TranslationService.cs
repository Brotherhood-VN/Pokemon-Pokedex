using API._Services.Interfaces.Systems;
using API.Data;
using API.Dtos.Systems;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Implementations.Systems
{
    public class TranslationService : ITranslationService
    {
        private readonly DBContext _context;

        public TranslationService(DBContext context)
        {
            _context = context;
        }

        #region Create
        public async Task<OperationResult> Create(TranslationDto dto)
        {
            if (await _context.Translation.AnyAsync(x => x.Language == dto.Language && x.Key.Trim() == dto.Key.Trim()))
                return new OperationResult { IsSuccess = false, Message = "Nội tại đã tồn tại. Vui lòng thử lại !!!" };

            Translation data = new()
            {
                FromTable = dto.FromTable,
                Language = dto.Language,
                Key = dto.Key,
                Value = dto.Value,
                CreateBy = dto.CreateBy,
                CreateTime = dto.CreateTime,
                IsDelete = false,
                Status = true
            };

            _context.Translation.Add(data);
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

        #region Delete
        public async Task<OperationResult> Delete(TranslationDto dto)
        {
            Translation data = await _context.Translation.FirstOrDefaultAsync(x => x.Id == dto.Id);

            if (data is null)
                return new OperationResult { IsSuccess = false, Message = "Nội tại không tồn tại. Vui lòng thử lại !!!" };

            data.IsDelete = true;
            data.UpdateBy = dto.UpdateBy;
            data.UpdateTime = dto.UpdateTime;

            _context.Translation.Update(data);
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

        #region GetDataPagination
        public async Task<PaginationUtility<TranslationDto>> GetDataPagination(PaginationParam pagination, string keyword)
        {
            var predicate = PredicateBuilder.New<Translation>(true);
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.ToLower();
                predicate.And(x => x.Key.ToLower().Contains(keyword)
                                || x.Value.ToLower().Contains(keyword));
            }

            var data = _context.Translation.Where(predicate)
                .Select(x => new TranslationDto
                {
                    Id = x.Id,
                    FromTable = x.FromTable,
                    Language = x.Language,
                    Key = x.Key,
                    Value = x.Value,
                    Status = x.Status,
                    UpdateTime = x.UpdateTime ?? x.CreateTime
                }).OrderByDescending(x => x.UpdateTime).AsNoTracking();

            return await PaginationUtility<TranslationDto>.CreateAsync(data, pagination.PageNumber, pagination.PageSize);
        }
        #endregion

        #region GetDetail
        public async Task<TranslationDto> GetDetail(long id)
        {
            var data = await _context.Translation
                .Where(x => x.Id == id)
                .Map<TranslationDto>()
                .AsNoTracking().FirstOrDefaultAsync();

            return data;
        }
        #endregion

        #region GetListTranslation
        public async Task<List<KeyValuePair<long, string>>> GetListTranslation()
        {
            return await _context.Translation.Where(x => x.Status == true)
                .OrderBy(x => x.Key)
                .ThenBy(x => x.Language)
                .Select(x => new KeyValuePair<long, string>(x.Id, $"{x.Language} - {x.Key}"))
                .Distinct().ToListAsync();
        }
        #endregion

        #region Update
        public async Task<OperationResult> Update(TranslationDto dto)
        {
            Translation data = await _context.Translation.FirstOrDefaultAsync(x => x.Id == dto.Id);
            if (data is null)
                return new OperationResult { IsSuccess = false, Message = "Nội tại không tồn tại. Vui lòng thử lại !!!" };

            data.Key = dto.Key;
            data.Value = dto.Value;
            data.Status = dto.Status;
            data.UpdateBy = dto.UpdateBy;
            data.UpdateTime = dto.UpdateTime;

            _context.Translation.Update(data);
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
    }

}