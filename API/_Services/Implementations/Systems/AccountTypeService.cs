using API._Services.Interfaces.Systems;
using API.Data;
using API.Dtos.Systems;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Implementations.Systems
{
    public class AccountTypeService : IAccountTypeService
    {
        private readonly DBContext _context;

        public AccountTypeService(DBContext context)
        {
            _context = context;
        }

        #region Create
        public async Task<OperationResult> Create(AccountTypeDto dto)
        {
            if (await _context.AccountType.AnyAsync(x => x.Code.Trim() == dto.Code.Trim()))
                return new OperationResult { IsSuccess = false, Message = "Loại tài khoản đã tồn tại. Vui lòng thử lại !!!" };

            AccountType data = new()
            {
                Code = dto.Code,
                Title = dto.Title,
                Description = dto.Description,
                CreateBy = dto.CreateBy,
                CreateTime = dto.CreateTime,
                IsDelete = false,
                Status = true
            };

            _context.AccountType.Add(data);
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
        public async Task<OperationResult> Delete(AccountTypeDto dto)
        {
            AccountType data = await _context.AccountType.FirstOrDefaultAsync(x => x.Id == dto.Id);

            if (data is null)
                return new OperationResult { IsSuccess = false, Message = "Loại tài khoản không tồn tại. Vui lòng thử lại !!!" };

            data.IsDelete = true;
            data.UpdateBy = dto.UpdateBy;
            data.UpdateTime = dto.UpdateTime;

            _context.AccountType.Update(data);
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
        public async Task<PaginationUtility<AccountTypeDto>> GetDataPagination(PaginationParam pagination, string keyword)
        {
            var predicate = PredicateBuilder.New<AccountType>(true);
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.ToLower();
                predicate.And(x => x.Title.ToLower().Contains(keyword) || x.Code.ToLower().Contains(keyword));
            }

            var data = _context.AccountType.Where(predicate)
                .Select(x => new AccountTypeDto
                {
                    Id = x.Id,
                    Code = x.Code,
                    Title = x.Title,
                    Description = x.Description,
                    Status = x.Status,
                    UpdateTime = x.UpdateTime ?? x.CreateTime
                }).OrderByDescending(x => x.UpdateTime).AsNoTracking();

            return await PaginationUtility<AccountTypeDto>.CreateAsync(data, pagination.PageNumber, pagination.PageSize);
        }
        #endregion

        #region GetDetail
        public async Task<AccountTypeDto> GetDetail(long id)
        {
            var data = await _context.AccountType
                .Where(x => x.Id == id)
                .Map<AccountTypeDto>()
                .AsNoTracking().FirstOrDefaultAsync();

            return data;
        }
        #endregion

        #region GetListAccountType
        public async Task<List<KeyValuePair<long, string>>> GetListAccountType()
        {
            return await _context.AccountType.Where(x => x.Status == true)
                .OrderBy(x => x.Code)
                .ThenBy(x => x.Title)
                .Select(x => new KeyValuePair<long, string>(x.Id, $"{x.Code} - {x.Title}"))
                .Distinct().ToListAsync();
        }
        #endregion

        #region Update
        public async Task<OperationResult> Update(AccountTypeDto dto)
        {
            AccountType data = await _context.AccountType.FirstOrDefaultAsync(x => x.Id == dto.Id);
            if (data is null)
                return new OperationResult { IsSuccess = false, Message = "Loại tài khoản không tồn tại. Vui lòng thử lại !!!" };

            data.Title = dto.Title;
            data.Description = dto.Description;
            data.Status = dto.Status;
            data.UpdateBy = dto.UpdateBy;
            data.UpdateTime = dto.UpdateTime;

            _context.AccountType.Update(data);
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