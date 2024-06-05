using API._Services.Interfaces.Systems;
using API.Data;
using API.Dtos.Systems;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Implementations.Systems
{
    public class SkillService : ISkillService
    {
        private readonly DBContext _context;

        public SkillService(DBContext context)
        {
            _context = context;
        }

        #region Create
        public async Task<OperationResult> Create(SkillDto dto)
        {
            if (await _context.Skill.AnyAsync(x => x.Code.Trim() == dto.Code.Trim() && x.IsDelete == false))
                return new OperationResult { IsSuccess = false, Message = "Kỹ năng đã tồn tại. Vui lòng thử lại !!!" };

            Skill data = new()
            {
                Code = dto.Code,
                Title = dto.Title,
                Description = dto.Description,
                Level = dto.Level,
                ItemId = dto.ItemId,
                CreateBy = dto.CreateBy,
                CreateTime = dto.CreateTime,
                IsDelete = false,
                Status = true
            };
            _context.Skill.Add(data);
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
        public async Task<OperationResult> Delete(SkillDto dto)
        {
            Skill data = await _context.Skill.FirstOrDefaultAsync(x => x.Id == dto.Id && x.IsDelete == false);
            if (data is null)
                return new OperationResult { IsSuccess = false, Message = "Kỹ năng không tồn tại. Vui lòng thử lại !!!" };

            data.IsDelete = true;
            data.UpdateBy = dto.UpdateBy;
            data.UpdateTime = dto.UpdateTime;

            _context.Skill.Update(data);
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
        public async Task<PaginationUtility<SkillDto>> GetDataPagination(PaginationParam pagination, string keyword)
        {
            var predicate = PredicateBuilder.New<Skill>(x => x.IsDelete == false);
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.ToLower();
                predicate.And(x => x.Title.ToLower().Contains(keyword) || x.Code.ToLower().Contains(keyword));
            }

            var data = _context.Skill.Where(predicate)
                .Select(x => new SkillDto
                {
                    Id = x.Id,
                    Code = x.Code,
                    Title = x.Title,
                    Description = x.Description,
                    Level = x.Level,
                    ItemId = x.ItemId,
                    Status = x.Status,
                    UpdateTime = x.UpdateTime ?? x.CreateTime
                }).OrderByDescending(x => x.UpdateTime).AsNoTracking();

            return await PaginationUtility<SkillDto>.CreateAsync(data, pagination.PageNumber, pagination.PageSize);
        }
        #endregion

        #region GetDetail
        public async Task<SkillDto> GetDetail(long id)
        {
            var data = await _context.Skill.FirstOrDefaultAsync(x => x.Id == id);

            return new SkillDto
            {
                Id = data.Id,
                Code = data.Code,
                Title = data.Title,
                Description = data.Description,
                Level = data.Level,
                ItemId = data.ItemId,
                IsDelete = data.IsDelete,
                Status = data.Status,
                CreateBy = data.CreateBy,
                CreateTime = data.CreateTime,
                UpdateBy = data.UpdateBy,
                UpdateTime = data.UpdateTime
            };
        }
        #endregion

        #region GetListSkill
        public async Task<List<KeyValuePair<long, string>>> GetListSkill()
        {
            return await _context.Skill.Where(x => x.IsDelete == false && x.Status == true)
                .OrderBy(x => x.Code)
                .ThenBy(x => x.Title)
                .Select(x => new KeyValuePair<long, string>(x.Id, $"{x.Code} - {x.Title}"))
                .Distinct().ToListAsync();
        }
        #endregion

        #region Update
        public async Task<OperationResult> Update(SkillDto dto)
        {
            Skill data = await _context.Skill.FirstOrDefaultAsync(x => x.Id == dto.Id && x.IsDelete == false);
            if (data is null)
                return new OperationResult { IsSuccess = false, Message = "Kỹ năng không tồn tại. Vui lòng thử lại !!!" };

            data.Title = dto.Title;
            data.Description = dto.Description;
            data.Level = dto.Level;
            data.ItemId = dto.ItemId;
            data.Status = dto.Status;
            data.UpdateBy = dto.UpdateBy;
            data.UpdateTime = dto.UpdateTime;

            _context.Skill.Update(data);
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