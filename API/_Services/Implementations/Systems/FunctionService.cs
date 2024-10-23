using System.Reflection;
using API._Services.Interfaces.Systems;
using API.Data;
using API.Dtos.Systems;
using API.Models;
using LinqKit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Implementations.Systems
{
    public class FunctionService : IFunctionService
    {
        private readonly DBContext _context;

        public FunctionService(DBContext context)
        {
            _context = context;
        }

        #region Create
        public async Task<OperationResult> Create(FunctionDto dto)
        {
            if (await _context.Function.AnyAsync(x => x.Area == dto.Area && x.Controller == dto.Controller && x.Action == dto.Action))
                return new OperationResult { IsSuccess = false, Message = "Chức năng đã tồn tại !!!" };

            Function data = new()
            {
                Area = dto.Area,
                Controller = dto.Controller,
                Action = dto.Action,
                Title = dto.Title,
                Description = dto.Description,
                IsShow = dto.IsShow,
                CreateBy = dto.CreateBy,
                CreateTime = dto.CreateTime
            };
            _context.Function.Add(data);
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

        #region CreateRange
        public async Task<OperationResult> CreateRange(FunctionViewDto dto)
        {
            List<string> actions = dto.ActionDetails.Select(x => x.Action).ToList();
            List<Function> olds = await _context.Function.Where(x => x.Area == dto.Area && x.Controller == dto.Controller && actions.Contains(x.Action)).ToListAsync();
            if (olds.Any())
                return new OperationResult { IsSuccess = false, Message = $"Chức năng {string.Join(", ", olds.Select(x => x.Action))} đã tồn tại !!!" };

            List<Function> functions = dto.ActionDetails
            .Select(x => new Function
            {
                Area = dto.Area,
                Controller = dto.Controller,
                Action = x.Action,
                IsShow = x.IsShow,
                IsMenu = x.IsMenu,
                IsDelete = false,
                Title = dto.Title,
                Description = x.Description,
                Seq = x.Seq,
                CreateBy = dto.CreateBy,
                CreateTime = dto.CreateTime
            }).ToList();

            try
            {
                await _context.Function.AddRangeAsync(functions);
                await _context.SaveChangesAsync();
                return new OperationResult { IsSuccess = true };
            }
            catch (Exception ex)
            {
                return new OperationResult { IsSuccess = false, Message = ex.Message };
            }
        }
        #endregion

        #region CreateAllFunction
        public async Task<OperationResult> CreateAllFunction(long userId, string controller = "")
        {
            var predicate = PredicateBuilder.New<MethodInfo>(x => x.GetCustomAttributes<MenuMemberAttribute>(true).Any());
            if (!string.IsNullOrWhiteSpace(controller))
                predicate.And(x => x.DeclaringType.Name.Contains(controller));

            Assembly asm = Assembly.GetExecutingAssembly();

            var controlleractionlist = asm.GetTypes()
                .Where(type => typeof(ControllerBase).IsAssignableFrom(type) && type.GetCustomAttributes(typeof(IsMenuAttribute), true).Any())
                .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                .Where(predicate)
                .Select(
                    x => new
                    {
                        Area = x.DeclaringType.FullName.Split(".")[2],
                        Controller = x.DeclaringType.Name.Replace("Controller", ""),
                        Action = x.Name,
                        x.GetCustomAttribute<MenuMemberAttribute>(true).Seq
                    })
                .OrderBy(x => x.Controller).ThenBy(x => x.Action).ToList();

            controlleractionlist.Add(new
            {
                Area = "Systems",
                Controller = "Dashboard",
                Action = "Dashboard",
                Seq = 1
            });

            List<Function> olds = await _context.Function.ToListAsync();

            List<Function> functions = new();
            List<Function> functionUpdates = new();
            foreach (var item in controlleractionlist)
            {
                var function = olds.FirstOrDefault(x => x.Area == item.Area && x.Controller == item.Controller && x.Action == item.Action);
                if (function != null)
                {
                    function.IsMenu = item.Seq == 1;
                    function.Seq = item.Seq;
                    function.UpdateBy = userId;
                    function.UpdateTime = DateTime.Now;
                    functionUpdates.Add(function);
                }
                else
                {
                    functions.Add(
                        new Function
                        {
                            Area = item.Area,
                            Action = item.Action,
                            Controller = item.Controller,
                            CreateBy = userId,
                            CreateTime = DateTime.Now,
                            IsShow = true,
                            IsMenu = item.Seq == 1,
                            IsDelete = false,
                            Seq = item.Seq,
                            Title = TranslateUtility.TranslateText(item.Controller),
                            Description = TranslateUtility.TranslateText(item.Action)
                        }
                    );
                }
            };

            using var _transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Function.UpdateRange(functionUpdates);
                await _context.Function.AddRangeAsync(functions);

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
        public async Task<OperationResult> Delete(FunctionDto dto)
        {
            var data = await _context.Function.FirstOrDefaultAsync(x => x.Id == dto.Id);
            if (data is null)
                return new OperationResult { IsSuccess = false, Message = "Chức năng không tồn tại. Vui lòng thử lại !!!" };

            data.IsDelete = true;
            data.UpdateBy = dto.UpdateBy;
            data.UpdateTime = dto.UpdateTime;
            _context.Function.Update(data);
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

        #region DeleteRange
        public async Task<OperationResult> DeleteRange(FunctionViewDto dto)
        {
            List<string> actions = dto.ActionDetails.Select(x => x.Action).ToList();
            List<Function> functions = await _context.Function.Where(x => x.Area == dto.Area && x.Controller == dto.Controller && actions.Contains(x.Action)).ToListAsync();
            if (actions.Except(functions.Select(x => x.Action)).Any())
                return new OperationResult { IsSuccess = false, Message = $"Chức năng {string.Join(", ", actions.Except(functions.Select(x => x.Action)))} không tồn tại !!!" };

            functions.ForEach(x =>
            {
                x.IsDelete = true;
                x.UpdateBy = dto.UpdateBy;
                x.UpdateTime = dto.UpdateTime;
            });
            try
            {
                _context.Function.UpdateRange(functions);
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
        public async Task<PaginationUtility<FunctionViewDto>> GetDataPagination(PaginationParam pagination, FunctionSearchParam param, bool isPaging = true)
        {
            var predicate = PredicateBuilder.New<Function>(true);
            if (!string.IsNullOrWhiteSpace(param.Area))
                predicate.And(x => x.Area.ToLower() == param.Area.ToLower());
            if (!string.IsNullOrWhiteSpace(param.Controller))
                predicate.And(x => x.Controller.ToLower() == param.Controller.ToLower());

            if (!string.IsNullOrWhiteSpace(param.Keyword))
            {
                param.Keyword = param.Keyword.ToLower();
                predicate.And(x => x.Title.ToLower().Contains(param.Keyword) || x.Description.ToLower().Contains(param.Keyword) || x.Area.ToLower().Contains(param.Keyword) || x.Controller.ToLower().Contains(param.Keyword) || x.Action.ToLower().Contains(param.Keyword));
            }

            var data = _context.Function
                .Where(predicate)
                .GroupBy(x => new { x.Area, x.Controller })
                .Select(x => new FunctionViewDto
                {
                    Area = x.Key.Area,
                    Controller = x.Key.Controller,
                    Title = x.FirstOrDefault().Title,
                    CreateBy = x.FirstOrDefault().CreateBy,
                    CreateTime = x.FirstOrDefault().CreateTime,
                    UpdateTime = x.FirstOrDefault().UpdateTime ?? x.FirstOrDefault().CreateTime,
                    ActionDetails = x.Select(a => new ActionDetail()
                    {
                        Id = a.Id,
                        Action = a.Action,
                        Description = a.Description,
                        Seq = a.Seq,
                        IsShow = a.IsShow,
                        IsMenu = a.IsMenu,
                        IsDelete = a.IsDelete,
                    }).OrderBy(x => x.Seq).ToList()
                }).OrderByDescending(x => x.UpdateTime).AsNoTracking();

            return await PaginationUtility<FunctionViewDto>.CreateAsync(data, pagination.PageNumber, pagination.PageSize, isPaging);
        }
        #endregion

        #region GetDetailGroup
        public async Task<FunctionViewDto> GetDetailGroup(FunctionParam param)
        {
            var data = await _context.Function
                .Where(
                    x => x.Area == param.Area &&
                    x.Controller == param.Controller)
                .GroupBy(x => new { x.Area, x.Controller })
                .Select(x => new FunctionViewDto
                {
                    Area = x.Key.Area,
                    Controller = x.Key.Controller,
                    Title = x.FirstOrDefault().Title,
                    CreateBy = x.FirstOrDefault().CreateBy,
                    CreateTime = x.FirstOrDefault().CreateTime,
                    UpdateTime = x.FirstOrDefault().UpdateTime ?? x.FirstOrDefault().CreateTime,
                    ActionDetails = x.Select(a => new ActionDetail()
                    {
                        Id = a.Id,
                        Action = a.Action,
                        Description = a.Description,
                        Seq = a.Seq,
                        IsShow = a.IsShow,
                        IsMenu = a.IsMenu,
                        IsDelete = a.IsDelete,
                    }).ToList()
                }).AsNoTracking().FirstOrDefaultAsync();

            return data;
        }
        #endregion

        #region GetDetail
        public async Task<FunctionDto> GetDetail(FunctionParam param)
        {
            var data = await _context.Function
                .Where(
                    x => x.Area == param.Area &&
                    x.Controller == param.Controller &&
                    x.Action == param.Action)
                .Select(x => new FunctionDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Action = x.Action,
                    Area = x.Area,
                    Controller = x.Controller,
                    Description = x.Description,
                    IsShow = x.IsShow,
                    IsMenu = x.IsMenu,
                    IsDelete = x.IsDelete,
                    Seq = x.Seq,
                    UpdateTime = x.UpdateTime ?? x.CreateTime,
                }).AsNoTracking().FirstOrDefaultAsync();

            return data;
        }
        #endregion

        #region GetListArea
        public async Task<List<KeyValuePair<string, string>>> GetListArea()
        {
            Assembly asm = Assembly.GetExecutingAssembly();

            var results = asm.GetTypes()
                .Where(type => typeof(ControllerBase).IsAssignableFrom(type))
                .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                .Where(x => !x.DeclaringType.FullName.Contains("Auth") && !x.Name.StartsWith("GetList") && !x.Name.StartsWith("GetAll"))
                .Select(x => new KeyValuePair<string, string>
                    (x.DeclaringType.FullName.Split(".")[2], x.DeclaringType.FullName.Split(".")[2]))
                .DistinctBy(x => x.Key).ToList();

            return await Task.FromResult(results);
        }
        #endregion

        #region GetListController
        public async Task<List<KeyValuePair<string, string>>> GetListController(string area)
        {
            List<string> funcs = await _context.Function
                .Where(x => x.IsMenu == true).AsNoTracking()
                .Select(x => x.Controller).ToListAsync();

            Assembly asm = Assembly.GetExecutingAssembly();

            var controllers = asm.GetTypes()
                .Where(type => typeof(ControllerBase).IsAssignableFrom(type) && type.GetCustomAttributes(typeof(IsMenuAttribute), true).Any())
                .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                .Where(
                    x => x.DeclaringType.FullName.Contains(area) &&
                    x.GetCustomAttributes<MenuMemberAttribute>(true).Any() &&
                    !funcs.Contains(x.DeclaringType.Name.Replace("Controller", "")))
                .Select(x => x.DeclaringType.Name.Replace("Controller", ""))
                .Distinct().ToList();

            var results = controllers.Select(x => new KeyValuePair<string, string>(x, TranslateUtility.TranslateText(x))).ToList();

            return await Task.FromResult(results);
        }
        #endregion

        #region GetListAction
        public async Task<List<KeyValuePair<string, string>>> GetListAction(string area, string controller)
        {
            Assembly asm = Assembly.GetExecutingAssembly();

            var actions = asm.GetTypes()
                .Where(type => typeof(ControllerBase).IsAssignableFrom(type))
                .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                .Where(x => x.DeclaringType.FullName.Contains(area) && x.DeclaringType.Name.Contains(controller) && !x.Name.StartsWith("GetList") && !x.Name.StartsWith("GetAll"))
                .Select(x => x.Name)
                .Distinct().ToList();

            var results = actions.Select(x => new KeyValuePair<string, string>(x, TranslateUtility.TranslateText(x))).ToList();

            return await Task.FromResult(results);
        }
        #endregion

        #region Update
        public async Task<OperationResult> Update(FunctionDto dto)
        {
            var data = await _context.Function.FirstOrDefaultAsync(x => x.Id == dto.Id);
            if (data is null)
                return new OperationResult { IsSuccess = false, Message = "Chức năng không tồn tại. Vui lòng thử lại !!!" };

            data.Description = dto.Description;
            data.IsShow = dto.IsShow;
            data.IsMenu = dto.IsMenu;
            data.Seq = dto.Seq;
            data.UpdateBy = dto.UpdateBy;
            data.UpdateTime = dto.UpdateTime;

            _context.Function.Update(data);
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

        #region UpdateRange
        public async Task<OperationResult> UpdateRange(FunctionViewDto dto)
        {
            using var _transaction = await _context.Database.BeginTransactionAsync();
            {
                List<Function> olds = await _context.Function.Where(x => x.Area == dto.Area && x.Controller == dto.Controller).AsNoTracking().ToListAsync();

                List<Function> deletes = olds.ExceptBy(dto.ActionDetails.Select(x => x.Action), x => x.Action).ToList();
                List<ActionDetail> creates = dto.ActionDetails.ExceptBy(olds.Select(x => x.Action), x => x.Action).ToList();
                List<ActionDetail> updates = dto.ActionDetails.IntersectBy(olds.Select(x => x.Action), x => x.Action).ToList();

                try
                {
                    if (deletes.Any())
                    {
                        deletes.ForEach(x =>
                        {
                            x.IsDelete = true;
                            x.UpdateBy = dto.UpdateBy;
                            x.UpdateTime = dto.UpdateTime;
                        });

                        _context.Function.UpdateRange(deletes);
                    }

                    if (updates.Any())
                    {
                        List<Function> funcUpdates = new();
                        updates.ForEach(x =>
                        {
                            funcUpdates.Add(new Function
                            {
                                Id = x.Id,
                                Area = dto.Area,
                                Controller = dto.Controller,
                                Action = x.Action,
                                Title = dto.Title,
                                Description = x.Description,
                                IsMenu = x.IsMenu,
                                IsShow = x.IsShow,
                                IsDelete = false,
                                Seq = x.Seq,
                                CreateBy = dto.CreateBy,
                                CreateTime = dto.CreateTime,
                                UpdateBy = dto.UpdateBy,
                                UpdateTime = dto.UpdateTime
                            });
                        });

                        _context.Function.UpdateRange(funcUpdates);
                    }

                    if (creates.Any())
                    {
                        List<Function> funcCreates = new();
                        creates.ForEach(x =>
                        {
                            funcCreates.Add(new Function
                            {
                                Area = dto.Area,
                                Controller = dto.Controller,
                                Action = x.Action,
                                Title = dto.Title,
                                Description = x.Description,
                                IsDelete = false,
                                IsMenu = x.IsMenu,
                                IsShow = x.IsShow,
                                Seq = x.Seq,
                                CreateBy = dto.UpdateBy.Value,
                                CreateTime = dto.UpdateTime.Value
                            });
                        });

                        await _context.Function.AddRangeAsync(funcCreates);
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
        }
        #endregion
    }
}