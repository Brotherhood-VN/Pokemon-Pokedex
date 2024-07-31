using API._Services.Interfaces.Systems;
using API.Data;
using API.Dtos.Systems;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Implementations.Systems
{
    public class AbilityService : IAbilityService
    {
        private readonly DBContext _context;

        public AbilityService(DBContext context)
        {
            _context = context;
        }

        #region Create
        public async Task<OperationResult> Create(AbilityDto dto)
        {
            if (await _context.Ability.AnyAsync(x => x.Code.Trim() == dto.Code.Trim() && x.IsDelete == false))
                return new OperationResult { IsSuccess = false, Message = "Nội tại đã tồn tại. Vui lòng thử lại !!!" };

            Ability data = new()
            {
                Code = dto.Code,
                Title = dto.Title,
                Description = dto.Description,
                Effect = dto.Effect,
                InDepthEffect = dto.InDepthEffect,
                CreateBy = dto.CreateBy,
                CreateTime = dto.CreateTime,
                IsDelete = false,
                Status = true
            };

            _context.Ability.Add(data);
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
        public async Task<OperationResult> Delete(AbilityDto dto)
        {
            Ability data = await _context.Ability.FirstOrDefaultAsync(x => x.Id == dto.Id && x.IsDelete == false);

            if (data is null)
                return new OperationResult { IsSuccess = false, Message = "Nội tại không tồn tại. Vui lòng thử lại !!!" };

            data.IsDelete = true;
            data.UpdateBy = dto.UpdateBy;
            data.UpdateTime = dto.UpdateTime;

            _context.Ability.Update(data);
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
        public async Task<PaginationUtility<AbilityDto>> GetDataPagination(PaginationParam pagination, string keyword)
        {
            var predicate = PredicateBuilder.New<Ability>(x => x.IsDelete == false);
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.ToLower();
                predicate.And(x => x.Title.ToLower().Contains(keyword) || x.Code.ToLower().Contains(keyword));
            }

            var data = _context.Ability.Where(predicate)
                .Select(x => new AbilityDto
                {
                    Id = x.Id,
                    Code = x.Code,
                    Title = x.Title,
                    Description = x.Description,
                    Effect = x.Effect,
                    InDepthEffect = x.InDepthEffect,
                    Status = x.Status,
                    UpdateTime = x.UpdateTime ?? x.CreateTime
                }).OrderByDescending(x => x.UpdateTime).AsNoTracking();

            return await PaginationUtility<AbilityDto>.CreateAsync(data, pagination.PageNumber, pagination.PageSize);
        }
        #endregion

        #region GetDetail
        public async Task<AbilityDto> GetDetail(long id)
        {
            var data = await _context.Ability.FirstOrDefaultAsync(x => x.Id == id);

            return new AbilityDto
            {
                Id = data.Id,
                Code = data.Code,
                Title = data.Title,
                Description = data.Description,
                Effect = data.Effect,
                InDepthEffect = data.InDepthEffect,
                IsDelete = data.IsDelete,
                Status = data.Status,
                CreateBy = data.CreateBy,
                CreateTime = data.CreateTime,
                UpdateBy = data.UpdateBy,
                UpdateTime = data.UpdateTime
            };
        }
        #endregion

        #region GetListAbility
        public async Task<List<KeyValuePair<long, string>>> GetListAbility()
        {
            return await _context.Ability.Where(x => x.IsDelete == false && x.Status == true)
                .OrderBy(x => x.Code)
                .ThenBy(x => x.Title)
                .Select(x => new KeyValuePair<long, string>(x.Id, $"{x.Code} - {x.Title}"))
                .Distinct().ToListAsync();
        }
        #endregion

        #region Update
        public async Task<OperationResult> Update(AbilityDto dto)
        {
            Ability data = await _context.Ability.FirstOrDefaultAsync(x => x.Id == dto.Id && x.IsDelete == false);
            if (data is null)
                return new OperationResult { IsSuccess = false, Message = "Nội tại không tồn tại. Vui lòng thử lại !!!" };

            data.Title = dto.Title;
            data.Description = dto.Description;
            data.Effect = dto.Effect;
            data.InDepthEffect = dto.InDepthEffect;
            data.Status = dto.Status;
            data.UpdateBy = dto.UpdateBy;
            data.UpdateTime = dto.UpdateTime;

            _context.Ability.Update(data);
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