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
                Effect = dto.Effect,
                InDepthEffect = dto.InDepthEffect,
                Level = dto.Level,
                ItemId = dto.ItemId,
                IsEgg = dto.IsEgg,
                IsTutor = dto.IsTutor,
                Power = dto.Power,
                Accuracy = dto.Accuracy,
                PP = dto.PP,
                Priority = dto.Priority,
                GenerationId = dto.GenerationId,
                Classes = dto.Classes,
                CreateBy = dto.CreateBy,
                CreateTime = dto.CreateTime,
                IsDelete = false,
                Status = true
            };

            using var _transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Skill.Add(data);
                await _context.SaveChangesAsync();

                List<SkillCondition> skillConditions = new();
                if (dto.SkillConditions is not null)
                {
                    foreach (var item in dto.SkillConditions)
                    {
                        skillConditions.Add(new()
                        {
                            SkillId = data.Id,
                            ConditionId = item.ConditionId,
                        });
                    }
                }

                List<SkillGameVersion> skillGameVersions = new();
                if (dto.SkillGameVersions is not null)
                {
                    foreach (var item in dto.SkillGameVersions)
                    {
                        skillGameVersions.Add(new()
                        {
                            SkillId = data.Id,
                            GameVersionId = item.GameVersionId,
                        });
                    }
                }

                _context.SkillCondition.AddRange(skillConditions);
                _context.SkillGameVersion.AddRange(skillGameVersions);
                await _context.SaveChangesAsync();

                await _transaction.CommitAsync();
                return new OperationResult { IsSuccess = true };
            }
            catch (Exception ex)
            {
                await _transaction.RollbackAsync();
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
            var data = await _context.Skill.Where(x => x.Id == id)
                .Include(x => x.SkillConditions)
                    .ThenInclude(x => x.Condition)
                .Include(x => x.SkillGameVersions)
                    .ThenInclude(x => x.GameVersion)
                .Select(x => new SkillDto()
                {
                    Id = x.Id,
                    Code = x.Code,
                    Title = x.Title,
                    Effect = x.Effect,
                    InDepthEffect = x.InDepthEffect,
                    Description = x.Description,
                    Level = x.Level,
                    ItemId = x.ItemId,
                    IsEgg = x.IsEgg,
                    IsTutor = x.IsTutor,
                    Power = x.Power,
                    Accuracy = x.Accuracy,
                    PP = x.PP,
                    Priority = x.Priority,
                    GenerationId = x.GenerationId,
                    Classes = x.Classes,
                    IsDelete = x.IsDelete,
                    Status = x.Status,
                    CreateBy = x.CreateBy,
                    CreateTime = x.CreateTime,
                    UpdateBy = x.UpdateBy,
                    UpdateTime = x.UpdateTime,
                    SkillConditions = x.SkillConditions.Select(y => new SkillConditionDto
                    {
                        ConditionId = y.Condition.Id,
                        ConditionCode = y.Condition.Code,
                        ConditionTitle = y.Condition.Title,
                    }).ToList(),

                    SkillGameVersions = x.SkillGameVersions.Select(y => new SkillGameVersionDto
                    {
                        GameVersionId = y.GameVersion.Id,
                        GameVersionCode = y.GameVersion.Code,
                        GameVersionTitle = y.GameVersion.Title,
                    }).ToList(),
                }).AsNoTracking().FirstOrDefaultAsync();

            return data;
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
            data.Effect = dto.Effect;
            data.InDepthEffect = dto.InDepthEffect;
            data.Description = dto.Description;
            data.Level = dto.Level;
            data.ItemId = dto.ItemId;
            data.IsEgg = dto.IsEgg;
            data.IsTutor = dto.IsTutor;
            data.Power = dto.Power;
            data.Accuracy = dto.Accuracy;
            data.PP = dto.PP;
            data.Priority = dto.Priority;
            data.GenerationId = dto.GenerationId;
            data.Classes = dto.Classes;
            data.Status = dto.Status;
            data.UpdateBy = dto.UpdateBy;
            data.UpdateTime = dto.UpdateTime;

            using var _transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Skill.Update(data);

                List<SkillCondition> skillConditionsOld = await _context.SkillCondition.Where(x => x.SkillId == data.Id).ToListAsync();
                if (skillConditionsOld is not null)
                    _context.SkillCondition.RemoveRange(skillConditionsOld);

                if (dto.SkillConditions is not null)
                {
                    List<SkillCondition> skillConditions = new();

                    foreach (var item in dto.SkillConditions)
                    {
                        skillConditions.Add(new()
                        {
                            SkillId = data.Id,
                            ConditionId = item.ConditionId,
                        });
                    }

                    _context.SkillCondition.AddRange(skillConditions);
                }

                List<SkillGameVersion> skillGameVersionsOld = await _context.SkillGameVersion.Where(x => x.SkillId == data.Id).ToListAsync();
                if (skillGameVersionsOld is not null)
                    _context.SkillGameVersion.RemoveRange(skillGameVersionsOld);

                if (dto.SkillGameVersions is not null)
                {
                    List<SkillGameVersion> skillGameVersions = new();

                    foreach (var item in dto.SkillGameVersions)
                    {
                        skillGameVersions.Add(new()
                        {
                            SkillId = data.Id,
                            GameVersionId = item.GameVersionId,
                        });
                    }

                    _context.SkillGameVersion.AddRange(skillGameVersions);
                }

                await _context.SaveChangesAsync();
                await _transaction.CommitAsync();
                return new OperationResult { IsSuccess = true };
            }
            catch (Exception ex)
            {
                await _transaction.RollbackAsync();
                return new OperationResult { IsSuccess = false, Message = ex.Message };
            }
        }
        #endregion
    }
}