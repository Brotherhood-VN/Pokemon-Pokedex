using API._Services.Interfaces.Systems;
using API.Data;
using API.Dtos.Systems;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Implementations.Systems
{
    public class AccountService : IAccountService
    {
        private readonly DBContext _context;

        public AccountService(DBContext context)
        {
            _context = context;
        }

        public async Task<OperationResult> Create(AccountDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.UserName))
                return new OperationResult { IsSuccess = false, Message = "Tài khoản không được để trống !!!" };

            if (string.IsNullOrWhiteSpace(dto.Password))
                return new OperationResult { IsSuccess = false, Message = "Mật khẩu không được để trống !!!" };

            if (await _context.Account.AnyAsync(x => x.UserName.Trim() == dto.UserName.Trim()))
                return new OperationResult { IsSuccess = false, Message = "Tài khoản đã tồn tại. Vui lòng thử lại !!!" };

            using var _transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                Account data = new()
                {
                    UserName = dto.UserName,
                    Password = EncryptorUtility.EncryptUserPassword(dto.Password),
                    CreateBy = dto.CreateBy,
                    CreateTime = dto.CreateTime,
                    IsDelete = false,
                    Status = true,
                    AccountTypeId = dto.AccountTypeId
                };
                _context.Account.Add(data);
                await _context.SaveChangesAsync();

                if (dto.RoleIds != null)
                {
                    List<AccountRole> roles = new();
                    dto.RoleIds.ForEach(id =>
                    {
                        roles.Add(new AccountRole
                        {
                            AccountId = data.Id,
                            RoleId = id
                        });
                    });

                    _context.AccountRole.AddRange(roles);
                    await _context.SaveChangesAsync();
                }

                if (dto.FunctionIds != null)
                {
                    List<AccountFunction> functions = new();
                    dto.FunctionIds.ForEach(id =>
                    {
                        functions.Add(new AccountFunction
                        {
                            AccountId = data.Id,
                            FunctionId = id
                        });
                    });

                    _context.AccountFunction.AddRange(functions);
                    await _context.SaveChangesAsync();
                }

                await _transaction.CommitAsync();
                return new OperationResult { IsSuccess = true, Data = data.Id };
            }
            catch (Exception ex)
            {
                await _transaction.RollbackAsync();
                return new OperationResult { IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<OperationResult> ChangePassword(AccountChangePassword dto)
        {
            Account data = await _context.Account.FirstOrDefaultAsync(x => x.Id == dto.Id);
            if (data is null)
                return new OperationResult { IsSuccess = false, Message = "Tài khoản không tồn tại. Vui lòng thử lại !!!" };
            if (data.Password != EncryptorUtility.EncryptUserPassword(dto.Password))
                return new OperationResult { IsSuccess = false, Message = "Mật khẩu cũ không chính xác !!!" };
            if (string.IsNullOrWhiteSpace(dto.NewPassword) || string.IsNullOrWhiteSpace(dto.ConfirmPassword))
                return new OperationResult { IsSuccess = false, Message = "Mật khẩu mới và nhập lại mật khẩu mới không được để trống !!!" };
            if (dto.NewPassword != dto.ConfirmPassword)
                return new OperationResult { IsSuccess = false, Message = "Nhập lại mật khẩu không chính xác !!!" };

            data.Password = EncryptorUtility.EncryptUserPassword(dto.NewPassword);
            data.UpdateBy = dto.UpdateBy;
            data.UpdateTime = dto.UpdateTime;

            _context.Account.Update(data);
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

        public async Task<OperationResult> ResetPassword(long id)
        {
            Account data = await _context.Account.FirstOrDefaultAsync(x => x.Id == id);
            if (data is null)
                return new OperationResult { IsSuccess = false, Message = "Tài khoản không tồn tại. Vui lòng thử lại !!!" };

            string password = $"RS{GenerateCodeUtility.RandomNumber(6)}";
            data.Password = EncryptorUtility.EncryptUserPassword(password);
            _context.Account.Update(data);
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

        public async Task<OperationResult> Delete(AccountDto dto)
        {
            Account data = await _context.Account.FirstOrDefaultAsync(x => x.Id == dto.Id);
            if (data is null)
                return new OperationResult { IsSuccess = false, Message = "Tài khoản không tồn tại. Vui lòng thử lại !!!" };

            data.IsDelete = true;
            data.UpdateBy = dto.UpdateBy;
            data.UpdateTime = dto.UpdateTime;

            _context.Account.Update(data);
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

        public async Task<PaginationUtility<AccountDto>> GetDataPagination(PaginationParam pagination, string keyword)
        {
            var predicate = PredicateBuilder.New<Account>(true);
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.ToLower();
                predicate.And(x => x.UserName.ToLower().Contains(keyword));
            }

            var data = _context.Account.Where(predicate)
                .Include(x => x.AccountType)
                .Select(x => new AccountDto
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    AccountTypeTitle = x.AccountType.Title,
                    Status = x.Status,
                    UpdateTime = x.UpdateTime ?? x.CreateTime
                }).AsNoTracking();

            return await PaginationUtility<AccountDto>.CreateAsync(data, pagination.PageNumber, pagination.PageSize);
        }

        public async Task<AccountDto> GetDetail(long id)
        {
            var data = await _context.Account
                .Include(x => x.AccountType)
                .FirstOrDefaultAsync(x => x.Id == id);

            return new AccountDto
            {
                Id = data.Id,
                UserName = data.UserName,
                AccountTypeTitle = data.AccountType.Title,
                IsDelete = data.IsDelete,
                Status = data.Status,
                CreateBy = data.CreateBy,
                CreateTime = data.CreateTime,
                UpdateBy = data.UpdateBy,
                UpdateTime = data.UpdateTime
            };
        }

        public async Task<OperationResult> Update(AccountDto dto)
        {
            Account data = await _context.Account.FirstOrDefaultAsync(x => x.Id == dto.Id);
            if (data is null)
                return new OperationResult { IsSuccess = false, Message = "Tài khoản không tồn tại. Vui lòng thử lại !!!" };

            using var _transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                data.Status = dto.Status;
                data.UpdateBy = dto.UpdateBy;
                data.UpdateTime = dto.UpdateTime;

                _context.Account.Update(data);
                await _context.SaveChangesAsync();

                if (dto.RoleIds != null)
                {
                    List<AccountRole> olds = await _context.AccountRole.Where(x => x.AccountId == data.Id).ToListAsync();
                    _context.RemoveRange(olds);

                    List<AccountRole> roles = new();
                    dto.RoleIds.ForEach(id =>
                    {
                        roles.Add(new AccountRole
                        {
                            AccountId = data.Id,
                            RoleId = id
                        });
                    });

                    _context.AccountRole.AddRange(roles);
                    await _context.SaveChangesAsync();
                }

                if (dto.FunctionIds != null)
                {
                    List<AccountFunction> olds = await _context.AccountFunction.Where(x => x.AccountId == data.Id).ToListAsync();
                    _context.RemoveRange(olds);

                    List<AccountFunction> functions = new();
                    dto.FunctionIds.ForEach(id =>
                    {
                        functions.Add(new AccountFunction
                        {
                            AccountId = data.Id,
                            FunctionId = id
                        });
                    });

                    _context.AccountFunction.AddRange(functions);
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
    }
}