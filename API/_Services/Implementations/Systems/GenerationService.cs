using API._Services.Interfaces.Systems;
using API.Data;
using API.Dtos.Systems;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Implementations.Systems
{
    public class GenerationService : IGenerationService
    {
        private readonly DBContext _context;

        public GenerationService(DBContext context)
        {
            _context = context;
        }

        #region Create
        public async Task<OperationResult> Create(GenerationDto dto)
        {
            if (await _context.Generation.AnyAsync(x => x.Code.Trim() == dto.Code.Trim() && x.IsDelete == false))
                return new OperationResult { IsSuccess = false, Message = "Thế hệ đã tồn tại. Vui lòng thử lại !!!" };

            Generation data = new()
            {
                Code = dto.Code,
                Title = dto.Title,
                Description = dto.Description,
                CreateBy = dto.CreateBy,
                CreateTime = dto.CreateTime,
                IsDelete = false,
                Status = true
            };
            _context.Generation.Add(data);
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
        public async Task<OperationResult> Delete(GenerationDto dto)
        {
            Generation data = await _context.Generation.FirstOrDefaultAsync(x => x.Id == dto.Id && x.IsDelete == false);
            if (data is null)
                return new OperationResult { IsSuccess = false, Message = "Thế hệ không tồn tại. Vui lòng thử lại !!!" };

            data.IsDelete = true;
            data.UpdateBy = dto.UpdateBy;
            data.UpdateTime = dto.UpdateTime;

            _context.Generation.Update(data);
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
        public async Task<PaginationUtility<GenerationDto>> GetDataPagination(PaginationParam pagination, string keyword)
        {
            var predicate = PredicateBuilder.New<Generation>(x => x.IsDelete == false);
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.ToLower();
                predicate.And(x => x.Title.ToLower().Contains(keyword) || x.Code.ToLower().Contains(keyword));
            }

            var data = _context.Generation.Where(predicate)
                .Select(x => new GenerationDto
                {
                    Id = x.Id,
                    Code = x.Code,
                    Title = x.Title,
                    Description = x.Description,
                    Status = x.Status,
                    UpdateTime = x.UpdateTime ?? x.CreateTime
                }).OrderByDescending(x => x.UpdateTime).AsNoTracking();

            return await PaginationUtility<GenerationDto>.CreateAsync(data, pagination.PageNumber, pagination.PageSize);
        }
        #endregion

        #region GetDetail
        public async Task<GenerationDto> GetDetail(long id)
        {
            var data = await _context.Generation.FirstOrDefaultAsync(x => x.Id == id);

            return new GenerationDto
            {
                Id = data.Id,
                Code = data.Code,
                Title = data.Title,
                Description = data.Description,
                IsDelete = data.IsDelete,
                Status = data.Status,
                CreateBy = data.CreateBy,
                CreateTime = data.CreateTime,
                UpdateBy = data.UpdateBy,
                UpdateTime = data.UpdateTime
            };
        }
        #endregion

        #region GetListGeneration
        public async Task<List<KeyValuePair<long, string>>> GetListGeneration()
        {
            return await _context.Generation.Where(x => x.IsDelete == false && x.Status == true)
                .OrderBy(x => x.Code)
                .ThenBy(x => x.Title)
                .Select(x => new KeyValuePair<long, string>(x.Id, $"{x.Code} - {x.Title}"))
                .Distinct().ToListAsync();
        }
        #endregion

        #region Update
        public async Task<OperationResult> Update(GenerationDto dto)
        {
            Generation data = await _context.Generation.FirstOrDefaultAsync(x => x.Id == dto.Id && x.IsDelete == false);
            if (data is null)
                return new OperationResult { IsSuccess = false, Message = "Thế hệ không tồn tại. Vui lòng thử lại !!!" };

            data.Title = dto.Title;
            data.Description = dto.Description;
            data.Status = dto.Status;
            data.UpdateBy = dto.UpdateBy;
            data.UpdateTime = dto.UpdateTime;

            _context.Generation.Update(data);
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