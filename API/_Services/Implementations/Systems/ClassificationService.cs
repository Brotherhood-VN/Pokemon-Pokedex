using API._Services.Interfaces.Systems;
using API.Data;
using API.Dtos.Systems;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Implementations.Systems
{
    public class ClassificationService : IClassificationService
    {
        private readonly DBContext _context;

        public ClassificationService(DBContext context)
        {
            _context = context;
        }

        #region Create
        public async Task<OperationResult> Create(ClassificationDto dto)
        {
            if (await _context.Classification.AnyAsync(x => x.Code.Trim() == dto.Code.Trim()))
                return new OperationResult { IsSuccess = false, Message = "Hệ đã tồn tại. Vui lòng thử lại !!!" };

            Classification data = new()
            {
                Code = dto.Code,
                Title = dto.Title,
                Description = dto.Description,
                Icon = dto.Icon,
                CreateBy = dto.CreateBy,
                CreateTime = dto.CreateTime,
                IsDelete = false,
                Status = true
            };
            _context.Classification.Add(data);
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
        public async Task<OperationResult> Delete(ClassificationDto dto)
        {
            Classification data = await _context.Classification.FirstOrDefaultAsync(x => x.Id == dto.Id);
            if (data is null)
                return new OperationResult { IsSuccess = false, Message = "Hệ không tồn tại. Vui lòng thử lại !!!" };

            data.IsDelete = true;
            data.UpdateBy = dto.UpdateBy;
            data.UpdateTime = dto.UpdateTime;

            _context.Classification.Update(data);
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
        public async Task<PaginationUtility<ClassificationDto>> GetDataPagination(PaginationParam pagination, string keyword)
        {
            var predicate = PredicateBuilder.New<Classification>(true);
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.ToLower();
                predicate.And(x => x.Title.ToLower().Contains(keyword) || x.Code.ToLower().Contains(keyword));
            }

            var data = _context.Classification.Where(predicate)
                .Select(x => new ClassificationDto
                {
                    Id = x.Id,
                    Code = x.Code,
                    Title = x.Title,
                    Description = x.Description,
                    Icon = x.Icon,
                    Status = x.Status,
                    UpdateTime = x.UpdateTime ?? x.CreateTime
                }).OrderByDescending(x => x.UpdateTime).AsNoTracking();

            return await PaginationUtility<ClassificationDto>.CreateAsync(data, pagination.PageNumber, pagination.PageSize);
        }
        #endregion

        #region GetDetail
        public async Task<ClassificationDto> GetDetail(long id)
        {
            var data = await _context.Classification
                .Where(x => x.Id == id)
                .Map<ClassificationDto>()
                .AsNoTracking().FirstOrDefaultAsync();

            return data;
        }
        #endregion

        #region GetListClassification
        public async Task<List<KeyValuePair<long, string>>> GetListClassification()
        {
            return await _context.Classification.Where(x => x.Status == true)
                .OrderBy(x => x.Code)
                .ThenBy(x => x.Title)
                .Select(x => new KeyValuePair<long, string>(x.Id, $"{x.Code} - {x.Title}"))
                .Distinct().ToListAsync();
        }
        #endregion

        #region Update
        public async Task<OperationResult> Update(ClassificationDto dto)
        {
            Classification data = await _context.Classification.FirstOrDefaultAsync(x => x.Id == dto.Id);
            if (data is null)
                return new OperationResult { IsSuccess = false, Message = "Hệ không tồn tại. Vui lòng thử lại !!!" };

            data.Title = dto.Title;
            data.Description = dto.Description;
            data.Icon = dto.Icon;
            data.Status = dto.Status;
            data.UpdateBy = dto.UpdateBy;
            data.UpdateTime = dto.UpdateTime;

            _context.Classification.Update(data);
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