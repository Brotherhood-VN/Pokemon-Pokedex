using API._Services.Interfaces.Systems;
using API.Data;
using API.Dtos.Systems;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Implementations.Systems
{
    public class PokemonService : IPokemonService
    {
        private readonly DBContext _context;

        public PokemonService(DBContext context)
        {
            _context = context;
        }

        #region Create
        public async Task<OperationResult> Create(PokemonDto dto)
        {
            if (await _context.Pokemon.AnyAsync(x => x.Index.Trim() == dto.Index.Trim() && x.IsDelete == false))
                return new OperationResult { IsSuccess = false, Message = "Khả năng đã tồn tại. Vui lòng thử lại !!!" };

            Pokemon data = new()
            {
                Index = dto.Index,
                FullName = dto.FullName,
                Height = dto.Height,
                Weight = dto.Weight,
                Description = dto.Description,
                Story = dto.Story,
                Note = dto.Note,
                BeforeIndex = dto.BeforeIndex,
                RankId = dto.RankId,
                ConditionId = dto.ConditionId,
                Level = dto.Level,
                StoneId = dto.StoneId,
                CreateBy = dto.CreateBy,
                CreateTime = dto.CreateTime,
                IsDelete = false,
                Status = true
            };

            using var _transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Pokemon.Add(data);
                await _context.SaveChangesAsync();

                Against against = new()
                {
                    PokemonId = data.Id,
                    Zero = dto.Against.Zero,
                    Quarter = dto.Against.Quarter,
                    Half = dto.Against.Half,
                    Two = dto.Against.Two,
                    Four = dto.Against.Four
                };

                Breeding breeding = new()
                {
                    PokemonId = data.Id,
                    EggGroup = dto.Breeding.EggGroup,
                    GenderDistribution = dto.Breeding.GenderDistribution,
                    EggCycle = dto.Breeding.EggCycle
                };

                Form form = new()
                {
                    PokemonId = data.Id,
                    AlternativeForm = dto.Form.AlternativeForm,
                    GenderDifference = dto.Form.GenderDifference
                };

                Stat stat = new()
                {
                    PokemonId = data.Id,
                    AreaId = dto.Stat.AreaId,
                    RegionId = dto.Stat.RegionId,
                    Attack = dto.Stat.Attack,
                    Defence = dto.Stat.Defence,
                    Speed = dto.Stat.Speed,
                    SpeedAttack = dto.Stat.SpeedAttack,
                    SpeedDefence = dto.Stat.SpeedDefence,
                    StatTypeId = dto.Stat.StatTypeId,
                };

                Training training = new()
                {
                    PokemonId = data.Id,
                    EVYield = dto.Training.EVYield,
                    CatchRate = dto.Training.CatchRate,
                    Base = dto.Training.Base,
                    FriendShip = dto.Training.FriendShip,
                    BaseExp = dto.Training.BaseExp,
                    GrowthRate = dto.Training.GrowthRate,
                    HeldItem = dto.Training.HeldItem
                };

                List<PokemonAbility> pokemonAbilities = new();
                if (dto.PokemonAbilities is not null)
                {
                    foreach (var item in dto.PokemonAbilities)
                    {
                        pokemonAbilities.Add(new()
                        {
                            AbilityId = item.AbilityId,
                            PokemonId = data.Id,
                        });
                    }
                }

                List<PokemonGameVersion> pokemonGameVersions = new();
                if (dto.PokemonGameVersions is not null)
                {
                    foreach (var item in dto.PokemonGameVersions)
                    {
                        pokemonGameVersions.Add(new()
                        {
                            PokemonId = data.Id,
                            GameVersionId = item.GameVersionId,
                            Description = item.Description,
                        });
                    }
                }

                List<PokemonGen> pokemonGens = new();
                if (dto.PokemonGens is not null)
                {
                    foreach (var item in dto.PokemonGens)
                    {
                        pokemonGens.Add(new()
                        {
                            PokemonId = data.Id,
                            GenerationId = item.GenId,
                        });
                    }
                }

                List<PokemonClass> pokemonClasses = new();
                if (dto.PokemonClasses is not null)
                {
                    foreach (var item in dto.PokemonClasses)
                    {
                        pokemonClasses.Add(new()
                        {
                            PokemonId = data.Id,
                            ClassificationId = item.ClassId,
                            IsDefault = item.IsDefault
                        });
                    }
                }

                List<PokemonSkill> pokemonSkills = new();
                if (dto.PokemonSkills is not null)
                {
                    foreach (var item in dto.PokemonSkills)
                    {
                        pokemonSkills.Add(new()
                        {
                            PokemonId = data.Id,
                            SkillId = item.SkillId
                        });
                    }
                }

                await _context.Against.AddAsync(against);
                await _context.Breeding.AddAsync(breeding);
                await _context.Form.AddAsync(form);
                await _context.Stat.AddAsync(stat);
                await _context.Training.AddAsync(training);

                await _context.PokemonAbility.AddRangeAsync(pokemonAbilities);
                await _context.PokemonGameVersion.AddRangeAsync(pokemonGameVersions);
                await _context.PokemonGen.AddRangeAsync(pokemonGens);
                await _context.PokemonClass.AddRangeAsync(pokemonClasses);
                await _context.PokemonSkill.AddRangeAsync(pokemonSkills);
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
        public async Task<OperationResult> Delete(PokemonDto dto)
        {
            Pokemon data = await _context.Pokemon.FirstOrDefaultAsync(x => x.Id == dto.Id && x.IsDelete == false);

            if (data is null)
                return new OperationResult { IsSuccess = false, Message = "Khả năng không tồn tại. Vui lòng thử lại !!!" };

            data.IsDelete = true;
            data.UpdateBy = dto.UpdateBy;
            data.UpdateTime = dto.UpdateTime;

            _context.Pokemon.Update(data);
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
        public async Task<PaginationUtility<PokemonDto>> GetDataPagination(PaginationParam pagination, string keyword)
        {
            var predicate = PredicateBuilder.New<Pokemon>(x => x.IsDelete == false);
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.ToLower();
                predicate.And(x => x.Index.ToLower().Contains(keyword) || x.FullName.ToLower().Contains(keyword));
            }

            var data = _context.Pokemon.Where(predicate)
                .Select(x => new PokemonDto
                {
                    Id = x.Id,
                    Index = x.Index,
                    Height = x.Height,
                    Weight = x.Weight,
                    FullName = x.FullName,
                    Description = x.Description,
                    Status = x.Status,
                    UpdateTime = x.UpdateTime ?? x.CreateTime
                }).OrderByDescending(x => x.UpdateTime).AsNoTracking();

            return await PaginationUtility<PokemonDto>.CreateAsync(data, pagination.PageNumber, pagination.PageSize);
        }
        #endregion

        #region GetDetail
        public async Task<PokemonDto> GetDetail(long id)
        {
            var data = await _context.Pokemon.Where(x => x.Id == id)
                .Include(x => x.Againsts)
                .Include(x => x.Stats)
                    .ThenInclude(x => x.StatType)
                .Include(x => x.PokemonAbilities)
                    .ThenInclude(x => x.Ability)
                .Include(x => x.PokemonGameVersions)
                    .ThenInclude(x => x.GameVersion)
                .Include(x => x.PokemonGens)
                    .ThenInclude(x => x.Generation)
                .Include(x => x.PokemonClasses)
                    .ThenInclude(x => x.Classification)
                .Include(x => x.PokemonSkills)
                    .ThenInclude(x => x.Skill)
                .GroupJoin(_context.Breeding,
                    x => x.Id,
                    y => y.PokemonId,
                    (x, y) => new { pokemon = x, breeding = y })
                .SelectMany(x => x.breeding.DefaultIfEmpty(),
                    (x, y) => new { x.pokemon, breeding = y })
                .GroupJoin(_context.Form,
                    x => x.pokemon.Id,
                    y => y.PokemonId,
                    (x, y) => new { x.pokemon, x.breeding, form = y })
                .SelectMany(x => x.form.DefaultIfEmpty(),
                    (x, y) => new { x.pokemon, x.breeding, form = y })
                .GroupJoin(_context.Training,
                    x => x.pokemon.Id,
                    y => y.PokemonId,
                    (x, y) => new { x.pokemon, x.breeding, x.form, training = y })
                .SelectMany(x => x.training.DefaultIfEmpty(),
                    (x, y) => new { x.pokemon, x.breeding, x.form, training = y })
                .Select(x => new PokemonDto
                {
                    Id = x.pokemon.Id,
                    FullName = x.pokemon.FullName,
                    Height = x.pokemon.Height,
                    Weight = x.pokemon.Weight,
                    Description = x.pokemon.Description,
                    Story = x.pokemon.Story,
                    Note = x.pokemon.Note,
                    BeforeIndex = x.pokemon.BeforeIndex,
                    RankId = x.pokemon.RankId,
                    ConditionId = x.pokemon.ConditionId,
                    Level = x.pokemon.Level,
                    StoneId = x.pokemon.StoneId,
                    IsDelete = x.pokemon.IsDelete,
                    Status = x.pokemon.Status,
                    CreateBy = x.pokemon.CreateBy,
                    CreateTime = x.pokemon.CreateTime,
                    UpdateBy = x.pokemon.UpdateBy,
                    UpdateTime = x.pokemon.UpdateTime,

                    Against = new()
                    {
                        Id = x.pokemon.Againsts.FirstOrDefault().Id,
                        PokemonId = x.pokemon.Againsts.FirstOrDefault().PokemonId,
                        Zero = x.pokemon.Againsts.FirstOrDefault().Zero,
                        Quarter = x.pokemon.Againsts.FirstOrDefault().Quarter,
                        Half = x.pokemon.Againsts.FirstOrDefault().Half,
                        Two = x.pokemon.Againsts.FirstOrDefault().Two,
                        Four = x.pokemon.Againsts.FirstOrDefault().Four,
                    },

                    Stat = new()
                    {
                        Id = x.pokemon.Stats.FirstOrDefault().Id,
                        PokemonId = x.pokemon.Stats.FirstOrDefault().PokemonId,
                        Attack = x.pokemon.Stats.FirstOrDefault().Attack,
                        Defence = x.pokemon.Stats.FirstOrDefault().Defence,
                        Speed = x.pokemon.Stats.FirstOrDefault().Speed,
                        SpeedAttack = x.pokemon.Stats.FirstOrDefault().SpeedAttack,
                        SpeedDefence = x.pokemon.Stats.FirstOrDefault().SpeedDefence,
                        StatTypeId = x.pokemon.Stats.FirstOrDefault().StatType.Id,
                        StatTypeCode = x.pokemon.Stats.FirstOrDefault().StatType.Code,
                        StatTypeTitle = x.pokemon.Stats.FirstOrDefault().StatType.Title,
                    },

                    Breeding = x.breeding == null ? null : new()
                    {
                        Id = x.breeding.Id,
                        EggGroup = x.breeding.EggGroup,
                        GenderDistribution = x.breeding.GenderDistribution,
                        EggCycle = x.breeding.EggCycle,
                        PokemonId = x.breeding.PokemonId
                    },

                    Form = x.form == null ? null : new()
                    {
                        Id = x.form.Id,
                        AlternativeForm = x.form.AlternativeForm,
                        GenderDifference = x.form.GenderDifference,
                        PokemonId = x.form.PokemonId
                    },

                    Training = x.training == null ? null : new()
                    {
                        Id = x.training.Id,
                        EVYield = x.training.EVYield,
                        CatchRate = x.training.CatchRate,
                        Base = x.training.Base,
                        FriendShip = x.training.FriendShip,
                        BaseExp = x.training.BaseExp,
                        GrowthRate = x.training.GrowthRate,
                        HeldItem = x.training.HeldItem,
                        PokemonId = x.training.PokemonId
                    },

                    PokemonAbilities = x.pokemon.PokemonAbilities.Select(x => new PokemonAbilityDto
                    {
                        AbilityId = x.Ability.Id,
                        AbilityCode = x.Ability.Code,
                        AbilityTitle = x.Ability.Title
                    }).ToList(),

                    PokemonGameVersions = x.pokemon.PokemonGameVersions.Select(x => new PokemonGameVersionDto
                    {
                        GameVersionId = x.GameVersion.Id,
                        GameVersionCode = x.GameVersion.Code,
                        GameVersionTitle = x.GameVersion.Title,
                        Description = x.Description
                    }).ToList(),

                    PokemonGens = x.pokemon.PokemonGens.Select(x => new PokemonGenDto
                    {
                        GenId = x.Generation.Id,
                        GenCode = x.Generation.Code,
                        GenTitle = x.Generation.Title
                    }).ToList(),

                    PokemonClasses = x.pokemon.PokemonClasses.Select(x => new PokemonClassDto
                    {
                        ClassId = x.Classification.Id,
                        ClassCode = x.Classification.Code,
                        ClassTitle = x.Classification.Title,
                        Icon = x.Classification.Icon,
                        IsDefault = x.IsDefault
                    }).ToList(),

                    PokemonSkills = x.pokemon.PokemonSkills.Select(x => new PokemonSkillDto
                    {
                        SkillId = x.Skill.Id,
                        SkillCode = x.Skill.Code,
                        SkillTitle = x.Skill.Title
                    }).ToList(),
                }).AsNoTracking().FirstOrDefaultAsync();

            return data;
        }
        #endregion

        #region GetListPokemon
        public async Task<List<KeyValuePair<long, string>>> GetListPokemon()
        {
            return await _context.Pokemon.Where(x => x.IsDelete == false && x.Status == true)
                .OrderBy(x => x.Index)
                .ThenBy(x => x.FullName)
                .Select(x => new KeyValuePair<long, string>(x.Id, $"{x.Index} - {x.FullName}"))
                .Distinct().ToListAsync();
        }
        #endregion

        #region Update
        public async Task<OperationResult> Update(PokemonDto dto)
        {
            Pokemon data = await _context.Pokemon.FirstOrDefaultAsync(x => x.Id == dto.Id && x.IsDelete == false);
            if (data is null)
                return new OperationResult { IsSuccess = false, Message = "Khả năng không tồn tại. Vui lòng thử lại !!!" };

            data.Index = dto.Index;
            data.FullName = dto.FullName;
            data.Height = dto.Height;
            data.Weight = dto.Weight;
            data.Description = dto.Description;
            data.Story = dto.Story;
            data.Note = dto.Note;
            data.BeforeIndex = dto.BeforeIndex;
            data.RankId = dto.RankId;
            data.ConditionId = dto.ConditionId;
            data.Level = dto.Level;
            data.StoneId = dto.StoneId;
            data.Status = dto.Status;
            data.UpdateBy = dto.UpdateBy;
            data.UpdateTime = dto.UpdateTime;

            using var _transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Pokemon.Update(data);

                List<Against> againstOld = await _context.Against.Where(x => x.PokemonId == data.Id).ToListAsync();
                if (againstOld is not null)
                    _context.Against.RemoveRange(againstOld);
                if (dto.Against is not null)
                {
                    Against against = new()
                    {
                        PokemonId = data.Id,
                        Zero = dto.Against.Zero,
                        Quarter = dto.Against.Quarter,
                        Half = dto.Against.Half,
                        Two = dto.Against.Two,
                        Four = dto.Against.Four
                    };

                    _context.Against.Add(against);
                }

                List<Breeding> breedingOld = await _context.Breeding.Where(x => x.PokemonId == data.Id).ToListAsync();
                if (breedingOld is not null)
                    _context.Breeding.RemoveRange(breedingOld);
                if (dto.Breeding is not null)
                {
                    Breeding breeding = new()
                    {
                        PokemonId = data.Id,
                        EggGroup = dto.Breeding.EggGroup,
                        GenderDistribution = dto.Breeding.GenderDistribution,
                        EggCycle = dto.Breeding.EggCycle,
                    };

                    _context.Breeding.Add(breeding);
                }

                List<Form> formOld = await _context.Form.Where(x => x.PokemonId == data.Id).ToListAsync();
                if (formOld is not null)
                    _context.Form.RemoveRange(formOld);
                if (dto.Form is not null)
                {
                    Form form = new()
                    {
                        PokemonId = data.Id,
                        AlternativeForm = dto.Form.AlternativeForm,
                        GenderDifference = dto.Form.GenderDifference
                    };

                    _context.Form.Add(form);
                }

                List<Stat> statOld = await _context.Stat.Where(x => x.PokemonId == data.Id).ToListAsync();
                if (statOld is not null)
                    _context.Stat.RemoveRange(statOld);
                if (dto.Stat is not null)
                {
                    Stat stat = new()
                    {
                        PokemonId = data.Id,
                        AreaId = dto.Stat.AreaId,
                        RegionId = dto.Stat.RegionId,
                        Attack = dto.Stat.Attack,
                        Defence = dto.Stat.Defence,
                        Speed = dto.Stat.Speed,
                        SpeedAttack = dto.Stat.SpeedAttack,
                        SpeedDefence = dto.Stat.SpeedDefence,
                        StatTypeId = dto.Stat.StatTypeId,
                    };

                    _context.Stat.Add(stat);
                }

                List<Training> trainingOld = await _context.Training.Where(x => x.PokemonId == data.Id).ToListAsync();
                if (trainingOld is not null)
                    _context.Training.RemoveRange(trainingOld);
                if (dto.Training is not null)
                {
                    Training training = new()
                    {
                        PokemonId = data.Id,
                        EVYield = dto.Training.EVYield,
                        CatchRate = dto.Training.CatchRate,
                        Base = dto.Training.Base,
                        FriendShip = dto.Training.FriendShip,
                        BaseExp = dto.Training.BaseExp,
                        GrowthRate = dto.Training.GrowthRate,
                        HeldItem = dto.Training.HeldItem
                    };

                    _context.Training.Add(training);
                }

                List<PokemonAbility> pokemonAbilitiesOld = await _context.PokemonAbility.Where(x => x.PokemonId == data.Id).ToListAsync();
                if (pokemonAbilitiesOld is not null)
                    _context.PokemonAbility.RemoveRange(pokemonAbilitiesOld);

                if (dto.PokemonAbilities is not null)
                {
                    List<PokemonAbility> pokemonAbilities = new();

                    foreach (var item in dto.PokemonAbilities)
                    {
                        pokemonAbilities.Add(new()
                        {
                            PokemonId = data.Id,
                            AbilityId = item.AbilityId
                        });
                    }

                    _context.PokemonAbility.AddRange(pokemonAbilities);
                }

                List<PokemonGameVersion> pokemonGameVersionsOld = await _context.PokemonGameVersion.Where(x => x.PokemonId == data.Id).ToListAsync();
                if (pokemonGameVersionsOld is not null)
                    _context.PokemonGameVersion.RemoveRange(pokemonGameVersionsOld);

                if (dto.PokemonGameVersions is not null)
                {
                    List<PokemonGameVersion> pokemonGameVersions = new();

                    foreach (var item in dto.PokemonGameVersions)
                    {
                        pokemonGameVersions.Add(new()
                        {
                            PokemonId = data.Id,
                            GameVersionId = item.GameVersionId,
                            Description = item.Description,
                        });
                    }

                    _context.PokemonGameVersion.AddRange(pokemonGameVersions);
                }

                List<PokemonGen> pokemonGensOld = await _context.PokemonGen.Where(x => x.PokemonId == data.Id).ToListAsync();
                if (pokemonGensOld is not null)
                    _context.PokemonGen.RemoveRange(pokemonGensOld);

                if (dto.PokemonGens is not null)
                {
                    List<PokemonGen> pokemonGens = new();

                    foreach (var item in dto.PokemonGens)
                    {
                        pokemonGens.Add(new()
                        {
                            PokemonId = data.Id,
                            GenerationId = item.GenId
                        });
                    }

                    _context.PokemonGen.AddRange(pokemonGens);
                }

                List<PokemonClass> pokemonClassesOld = await _context.PokemonClass.Where(x => x.PokemonId == data.Id).ToListAsync();
                if (pokemonClassesOld is not null)
                    _context.PokemonClass.RemoveRange(pokemonClassesOld);

                if (dto.PokemonClasses is not null)
                {
                    List<PokemonClass> pokemonClasses = new();

                    foreach (var item in dto.PokemonClasses)
                    {
                        pokemonClasses.Add(new()
                        {
                            PokemonId = data.Id,
                            ClassificationId = item.ClassId,
                            IsDefault = item.IsDefault
                        });
                    }

                    _context.PokemonClass.AddRange(pokemonClasses);
                }

                List<PokemonSkill> pokemonSkillsOld = await _context.PokemonSkill.Where(x => x.PokemonId == data.Id).ToListAsync();
                if (pokemonSkillsOld is not null)
                    _context.PokemonSkill.RemoveRange(pokemonSkillsOld);

                if (dto.PokemonSkills is not null)
                {
                    List<PokemonSkill> pokemonSkills = new();

                    foreach (var item in dto.PokemonSkills)
                    {
                        pokemonSkills.Add(new()
                        {
                            PokemonId = data.Id,
                            SkillId = item.SkillId
                        });
                    }

                    _context.PokemonSkill.AddRange(pokemonSkills);
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