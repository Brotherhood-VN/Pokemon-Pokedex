using API._Services.Interfaces.Systems;
using API.Data;
using API.Dtos.Systems;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Implementations.Systems
{
    public class StoneService : IStoneService
    {
        private readonly DBContext _context;

        public StoneService(DBContext context)
        {
            _context = context;
        }

        #region Create
        public async Task<OperationResult> Create(StoneDto dto)
        {
            if (await _context.Stone.AnyAsync(x => x.Code.Trim() == dto.Code.Trim()))
                return new OperationResult { IsSuccess = false, Message = "Đá tiến hoá đã tồn tại. Vui lòng thử lại !!!" };

            Stone data = new()
            {
                Code = dto.Code,
                Title = dto.Title,
                Description = dto.Description,
                CreateBy = dto.CreateBy,
                CreateTime = dto.CreateTime,
                IsDelete = false,
                Status = true
            };
            _context.Stone.Add(data);
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
        public async Task<OperationResult> Delete(StoneDto dto)
        {
            Stone data = await _context.Stone.FirstOrDefaultAsync(x => x.Id == dto.Id);
            if (data is null)
                return new OperationResult { IsSuccess = false, Message = "Đá tiến hoá không tồn tại. Vui lòng thử lại !!!" };

            data.IsDelete = true;
            data.UpdateBy = dto.UpdateBy;
            data.UpdateTime = dto.UpdateTime;

            _context.Stone.Update(data);
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
        public async Task<PaginationUtility<StoneDto>> GetDataPagination(PaginationParam pagination, string keyword)
        {
            var predicate = PredicateBuilder.New<Stone>(true);
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.ToLower();
                predicate.And(x => x.Title.ToLower().Contains(keyword) || x.Code.ToLower().Contains(keyword));
            }

            var data = _context.Stone.Where(predicate)
                .Select(x => new StoneDto
                {
                    Id = x.Id,
                    Code = x.Code,
                    Title = x.Title,
                    Description = x.Description,
                    Status = x.Status,
                    UpdateTime = x.UpdateTime ?? x.CreateTime
                }).OrderByDescending(x => x.UpdateTime).AsNoTracking();

            return await PaginationUtility<StoneDto>.CreateAsync(data, pagination.PageNumber, pagination.PageSize);
        }
        #endregion

        #region GetDetail
        public async Task<StoneDto> GetDetail(long id)
        {
            var data = await _context.Stone
                .Where(x => x.Id == id)
                .Map<StoneDto>()
                .AsNoTracking().FirstOrDefaultAsync();

            return data;
        }
        #endregion

        #region GetListStone
        public async Task<List<KeyValuePair<long, string>>> GetListStone()
        {
            return await _context.Stone.Where(x => x.Status == true)
                .OrderBy(x => x.Code)
                .ThenBy(x => x.Title)
                .Select(x => new KeyValuePair<long, string>(x.Id, $"{x.Code} - {x.Title}"))
                .Distinct().ToListAsync();
        }
        #endregion

        #region Update
        public async Task<OperationResult> Update(StoneDto dto)
        {
            Stone data = await _context.Stone.FirstOrDefaultAsync(x => x.Id == dto.Id);
            if (data is null)
                return new OperationResult { IsSuccess = false, Message = "Đá tiến hoá không tồn tại. Vui lòng thử lại !!!" };

            data.Title = dto.Title;
            data.Description = dto.Description;
            data.Status = dto.Status;
            data.UpdateBy = dto.UpdateBy;
            data.UpdateTime = dto.UpdateTime;

            _context.Stone.Update(data);
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