using API._Services.Interfaces.Systems;
using API.Data;
using API.Dtos.Systems;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Implementations.Systems
{
    public class RoleService : IRoleService
    {
        private readonly DBContext _context;

        public RoleService(DBContext context)
        {
            _context = context;
        }

        #region Create
        public async Task<OperationResult> Create(RoleDto dto)
        {
            if (await _context.Role.AnyAsync(x => x.Code.Trim() == dto.Code.Trim()))
                return new OperationResult { IsSuccess = false, Message = "Phân quyền đã tồn tại. Vui lòng thử lại !!!" };

            using var _transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                Role data = new()
                {
                    Code = dto.Code,
                    Title = dto.Title,
                    Description = dto.Description,
                    CreateBy = dto.CreateBy,
                    CreateTime = dto.CreateTime,
                    IsDelete = false,
                    Status = true
                };

                _context.Role.Add(data);
                await _context.SaveChangesAsync();

                if (dto.FunctionIds != null)
                {
                    List<RoleFunction> functions = new();
                    dto.FunctionIds.ForEach(id =>
                    {
                        functions.Add(new RoleFunction
                        {
                            RoleId = data.Id,
                            FunctionId = id
                        });
                    });

                    await _context.RoleFunction.AddRangeAsync(functions);
                    await _context.SaveChangesAsync();
                }

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
        public async Task<OperationResult> Delete(RoleDto dto)
        {
            Role data = await _context.Role.FirstOrDefaultAsync(x => x.Id == dto.Id);
            if (data is null)
                return new OperationResult { IsSuccess = false, Message = "Phân quyền không tồn tại. Vui lòng thử lại !!!" };

            data.IsDelete = true;
            data.UpdateBy = dto.UpdateBy;
            data.UpdateTime = dto.UpdateTime;

            _context.Role.Update(data);
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
        public async Task<PaginationUtility<RoleDto>> GetDataPagination(PaginationParam pagination, string keyword)
        {
            var predicate = PredicateBuilder.New<Role>(true);
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.ToLower();
                predicate.And(x => x.Title.ToLower().Contains(keyword) || x.Code.ToLower().Contains(keyword));
            }

            var data = _context.Role.Where(predicate)
                .Select(x => new RoleDto
                {
                    Id = x.Id,
                    Code = x.Code,
                    Title = x.Title,
                    Description = x.Description,
                    Status = x.Status,
                    UpdateTime = x.UpdateTime ?? x.CreateTime
                }).AsNoTracking();

            return await PaginationUtility<RoleDto>.CreateAsync(data, pagination.PageNumber, pagination.PageSize);
        }
        #endregion

        #region GetDetail
        public async Task<RoleDto> GetDetail(long id)
        {
            var data = await _context.Role
                .Where(x => x.Id == id)
                .Map<RoleDto>()
                .AsNoTracking().FirstOrDefaultAsync();

            return data;
        }
        #endregion

        #region GetListRole
        public async Task<List<KeyValuePair<long, string>>> GetListRole()
        {
            return await _context.Role.Where(x => x.Status == true)
                .OrderBy(x => x.Code)
                .ThenBy(x => x.Title)
                .Select(x => new KeyValuePair<long, string>(x.Id, $"{x.Code} - {x.Title}"))
                .Distinct().ToListAsync();
        }
        #endregion

        #region Update
        public async Task<OperationResult> Update(RoleDto dto)
        {
            Role data = await _context.Role.FirstOrDefaultAsync(x => x.Id == dto.Id);
            if (data is null)
                return new OperationResult { IsSuccess = false, Message = "Phân quyền không tồn tại. Vui lòng thử lại !!!" };

            using var _transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                data.Title = dto.Title;
                data.Description = dto.Description;
                data.Status = dto.Status;
                data.UpdateBy = dto.UpdateBy;
                data.UpdateTime = dto.UpdateTime;

                _context.Role.Update(data);
                await _context.SaveChangesAsync();

                if (dto.FunctionIds != null)
                {
                    List<RoleFunction> olds = await _context.RoleFunction.Where(x => x.RoleId == data.Id).ToListAsync();
                    _context.RemoveRange(olds);

                    List<RoleFunction> functions = new();
                    dto.FunctionIds.ForEach(id =>
                    {
                        functions.Add(new RoleFunction
                        {
                            RoleId = data.Id,
                            FunctionId = id
                        });
                    });

                    await _context.RoleFunction.AddRangeAsync(functions);
                    await _context.SaveChangesAsync();
                }

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