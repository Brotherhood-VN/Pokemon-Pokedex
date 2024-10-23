using Microsoft.EntityFrameworkCore;
using API.Models;

namespace API.Data
{
    public partial class DBContext : DbContext
    {
        public DBContext() { }

        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
            Database.SetCommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds);
        }

        public virtual DbSet<Ability> Ability { get; set; }
        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<AccountFunction> AccountFunction { get; set; }
        public virtual DbSet<AccountRole> AccountRole { get; set; }
        public virtual DbSet<AccountType> AccountType { get; set; }
        public virtual DbSet<Against> Against { get; set; }
        public virtual DbSet<AgainstForPokemon> AgainstForPokemon { get; set; }
        public virtual DbSet<Area> Area { get; set; }
        public virtual DbSet<Breeding> Breeding { get; set; }
        public virtual DbSet<Classification> Classification { get; set; }
        public virtual DbSet<Condition> Condition { get; set; }
        public virtual DbSet<Form> Form { get; set; }
        public virtual DbSet<Function> Function { get; set; }
        public virtual DbSet<GameVersion> GameVersion { get; set; }
        public virtual DbSet<Gender> Gender { get; set; }
        public virtual DbSet<Generation> Generation { get; set; }
        public virtual DbSet<Item> Item { get; set; }
        public virtual DbSet<Menu> Menu { get; set; }
        public virtual DbSet<Pokemon> Pokemon { get; set; }
        public virtual DbSet<PokemonAbility> PokemonAbility { get; set; }
        public virtual DbSet<PokemonClass> PokemonClass { get; set; }
        public virtual DbSet<PokemonGameVersion> PokemonGameVersion { get; set; }
        public virtual DbSet<PokemonGen> PokemonGen { get; set; }
        public virtual DbSet<PokemonSkill> PokemonSkill { get; set; }
        public virtual DbSet<Rank> Rank { get; set; }
        public virtual DbSet<RefreshToken> RefreshToken { get; set; }
        public virtual DbSet<Region> Region { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<RoleFunction> RoleFunction { get; set; }
        public virtual DbSet<Skill> Skill { get; set; }
        public virtual DbSet<SkillCondition> SkillCondition { get; set; }
        public virtual DbSet<SkillGameVersion> SkillGameVersion { get; set; }
        public virtual DbSet<Stat> Stat { get; set; }
        public virtual DbSet<StatType> StatType { get; set; }
        public virtual DbSet<Stone> Stone { get; set; }
        public virtual DbSet<Training> Training { get; set; }
        public virtual DbSet<Translation> Translation { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Ability>(entity =>
            {
                entity.HasKey(e => new { e.Id });
                entity.HasQueryFilter(x => x.IsDelete == false);
            });

            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => new { e.Id });
                entity.HasOne(d => d.AccountType).WithMany(p => p.Accounts).HasForeignKey(d => d.AccountTypeId).HasConstraintName("FK_Account_AccountType");
                entity.HasQueryFilter(x => x.IsDelete == false);
            });

            modelBuilder.Entity<AccountFunction>(entity =>
            {
                entity.HasKey(e => new { e.Id });
                entity.HasOne(d => d.Function).WithMany(p => p.AccountFunctions).HasForeignKey(d => d.FunctionId).HasConstraintName("FK_AccountFunction_Function");
                entity.HasOne(d => d.Account).WithMany(p => p.AccountFunctions).HasForeignKey(d => d.AccountId).HasConstraintName("FK_AccountFunction_Account");
            });

            modelBuilder.Entity<AccountRole>(entity =>
            {
                entity.HasKey(e => new { e.Id });
                entity.HasOne(d => d.Role).WithMany(p => p.AccountRoles).HasForeignKey(d => d.RoleId).HasConstraintName("FK_AccountRole_Role");
                entity.HasOne(d => d.Account).WithMany(p => p.AccountRoles).HasForeignKey(d => d.AccountId).HasConstraintName("FK_AccountRole_Account");
            });

            modelBuilder.Entity<AccountType>(entity =>
            {
                entity.HasKey(e => new { e.Id });
                entity.HasQueryFilter(x => x.IsDelete == false);
            });

            modelBuilder.Entity<Against>(entity =>
            {
                entity.HasKey(e => new { e.Id });
                entity.HasOne(d => d.Pokemon).WithMany(p => p.Againsts).HasForeignKey(d => d.PokemonId).HasConstraintName("FK_Against_Pokemon");
            });

            modelBuilder.Entity<AgainstForPokemon>(entity =>
            {
                entity.HasKey(e => new { e.Id });
            });

            modelBuilder.Entity<Area>(entity =>
            {
                entity.HasKey(e => new { e.Id });
                entity.HasOne(d => d.Region).WithMany(p => p.Areas).HasForeignKey(d => d.RegionId).HasConstraintName("FK_Area_Region");
                entity.HasQueryFilter(x => x.IsDelete == false);
            });

            modelBuilder.Entity<Breeding>(entity =>
            {
                entity.HasKey(e => new { e.Id });
            });

            modelBuilder.Entity<Classification>(entity =>
            {
                entity.HasKey(e => new { e.Id });
                entity.HasQueryFilter(x => x.IsDelete == false);
            });

            modelBuilder.Entity<Condition>(entity =>
            {
                entity.HasKey(e => new { e.Id });
                entity.HasQueryFilter(x => x.IsDelete == false);
            });

            modelBuilder.Entity<Form>(entity =>
            {
                entity.HasKey(e => new { e.Id });
            });

            modelBuilder.Entity<Function>(entity =>
            {
                entity.HasKey(e => new { e.Id });
                entity.HasQueryFilter(x => x.IsDelete == false);
            });

            modelBuilder.Entity<GameVersion>(entity =>
            {
                entity.HasKey(e => new { e.Id });
                entity.HasQueryFilter(x => x.IsDelete == false);
            });

            modelBuilder.Entity<Gender>(entity =>
            {
                entity.HasKey(e => new { e.Id });
                entity.HasQueryFilter(x => x.IsDelete == false);
            });

            modelBuilder.Entity<Generation>(entity =>
            {
                entity.HasKey(e => new { e.Id });
                entity.HasQueryFilter(x => x.IsDelete == false);
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.HasKey(e => new { e.Id });
                entity.HasQueryFilter(x => x.IsDelete == false);
            });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.HasKey(e => new { e.Id });
            });

            modelBuilder.Entity<Pokemon>(entity =>
            {
                entity.HasKey(e => new { e.Id });
                entity.HasOne(d => d.Stone).WithMany(p => p.Pokemons).HasForeignKey(d => d.StoneId).HasConstraintName("FK_Pokemon_Stone");
                entity.HasOne(d => d.Rank).WithMany(p => p.Pokemons).HasForeignKey(d => d.RankId).HasConstraintName("FK_Pokemon_Rank");
                entity.HasQueryFilter(x => x.IsDelete == false);
            });

            modelBuilder.Entity<PokemonAbility>(entity =>
            {
                entity.HasKey(e => new { e.Id });
                entity.HasOne(d => d.Pokemon).WithMany(p => p.PokemonAbilities).HasForeignKey(d => d.PokemonId).HasConstraintName("FK_PokemonAbility_Pokemon");
                entity.HasOne(d => d.Ability).WithMany(p => p.PokemonAbilities).HasForeignKey(d => d.AbilityId).HasConstraintName("FK_PokemonAbility_Ability");
            });

            modelBuilder.Entity<PokemonClass>(entity =>
            {
                entity.HasKey(e => new { e.Id });
                entity.HasOne(d => d.Classification).WithMany(p => p.PokemonClasses).HasForeignKey(d => d.ClassificationId).HasConstraintName("FK_PokemonClass_Classification");
                entity.HasOne(d => d.Pokemon).WithMany(p => p.PokemonClasses).HasForeignKey(d => d.PokemonId).HasConstraintName("FK_PokemonClass_Pokemon");
            });

            modelBuilder.Entity<PokemonGameVersion>(entity =>
            {
                entity.HasKey(e => new { e.Id });
                entity.HasOne(d => d.Pokemon).WithMany(p => p.PokemonGameVersions).HasForeignKey(d => d.PokemonId).HasConstraintName("FK_PokemonGameVersion_Pokemon");
                entity.HasOne(d => d.GameVersion).WithMany(p => p.PokemonGameVersions).HasForeignKey(d => d.GameVersionId).HasConstraintName("FK_PokemonGameVersion_GameVersion");
            });

            modelBuilder.Entity<PokemonGen>(entity =>
            {
                entity.HasKey(e => new { e.Id });
                entity.HasOne(d => d.Pokemon).WithMany(p => p.PokemonGens).HasForeignKey(d => d.PokemonId).HasConstraintName("FK_PokemonGen_Pokemon");
                entity.HasOne(d => d.Generation).WithMany(p => p.PokemonGens).HasForeignKey(d => d.GenerationId).HasConstraintName("FK_PokemonGen_Generation");
            });

            modelBuilder.Entity<PokemonSkill>(entity =>
            {
                entity.HasKey(e => new { e.Id });
                entity.HasOne(d => d.Skill).WithMany(p => p.PokemonSkills).HasForeignKey(d => d.SkillId).HasConstraintName("FK_PokemonSkill_Skill");
                entity.HasOne(d => d.Pokemon).WithMany(p => p.PokemonSkills).HasForeignKey(d => d.PokemonId).HasConstraintName("FK_PokemonSkill_Pokemon");
            });

            modelBuilder.Entity<Rank>(entity =>
            {
                entity.HasKey(e => new { e.Id });
                entity.HasQueryFilter(x => x.IsDelete == false);
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasKey(e => new { e.Id });
                entity.Property(e => e.Token).IsUnicode(false);
                entity.Property(e => e.ReplacedByToken).IsUnicode(false);
                entity.Property(e => e.ReasonRevoked).IsUnicode(false);
            });

            modelBuilder.Entity<Region>(entity =>
            {
                entity.HasKey(e => new { e.Id });
                entity.HasQueryFilter(x => x.IsDelete == false);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => new { e.Id });
                entity.HasQueryFilter(x => x.IsDelete == false);
            });

            modelBuilder.Entity<RoleFunction>(entity =>
            {
                entity.HasKey(e => new { e.Id });
                entity.HasOne(d => d.Role).WithMany(p => p.RoleFunctions).HasForeignKey(d => d.RoleId).HasConstraintName("FK_RoleFunction_Role");
                entity.HasOne(d => d.Function).WithMany(p => p.RoleFunctions).HasForeignKey(d => d.FunctionId).HasConstraintName("FK_RoleFunction_Function");
            });

            modelBuilder.Entity<Skill>(entity =>
            {
                entity.HasKey(e => new { e.Id });
                entity.HasOne(d => d.Item).WithMany(p => p.Skills).HasForeignKey(d => d.ItemId).HasConstraintName("FK_Skill_Item");
                entity.HasOne(d => d.Generation).WithMany(p => p.Skills).HasForeignKey(d => d.GenerationId).HasConstraintName("FK_Skill_Generation");
                entity.HasQueryFilter(x => x.IsDelete == false);
            });

            modelBuilder.Entity<SkillCondition>(entity =>
            {
                entity.HasKey(e => new { e.Id });
                entity.HasOne(d => d.Skill).WithMany(p => p.SkillConditions).HasForeignKey(d => d.SkillId).HasConstraintName("FK_SkillCondition_Skill");
                entity.HasOne(d => d.Condition).WithMany(p => p.SkillConditions).HasForeignKey(d => d.ConditionId).HasConstraintName("FK_SkillCondition_Condition");
            });

            modelBuilder.Entity<SkillGameVersion>(entity =>
            {
                entity.HasKey(e => new { e.Id });
                entity.HasOne(d => d.Skill).WithMany(p => p.SkillGameVersions).HasForeignKey(d => d.SkillId).HasConstraintName("FK_SkillGameVersion_Skill");
                entity.HasOne(d => d.GameVersion).WithMany(p => p.SkillGameVersions).HasForeignKey(d => d.GameVersionId).HasConstraintName("FK_SkillGameVersion_GameVersion");
            });

            modelBuilder.Entity<Stat>(entity =>
            {
                entity.HasKey(e => new { e.Id });
                entity.HasOne(d => d.Area).WithMany(p => p.Stats).HasForeignKey(d => d.AreaId).HasConstraintName("FK_Stat_Area");
                entity.HasOne(d => d.Region).WithMany(p => p.Stats).HasForeignKey(d => d.RegionId).HasConstraintName("FK_Stat_Region");
                entity.HasOne(d => d.Pokemon).WithMany(p => p.Stats).HasForeignKey(d => d.PokemonId).HasConstraintName("FK_Stat_Pokemon");
                entity.HasOne(d => d.StatType).WithMany(p => p.Stats).HasForeignKey(d => d.StatTypeId).HasConstraintName("FK_Stat_StatType");
            });

            modelBuilder.Entity<StatType>(entity =>
            {
                entity.HasKey(e => new { e.Id });
                entity.HasQueryFilter(x => x.IsDelete == false);
            });

            modelBuilder.Entity<Stone>(entity =>
            {
                entity.HasKey(e => new { e.Id });
                entity.HasQueryFilter(x => x.IsDelete == false);
            });

            modelBuilder.Entity<Training>(entity =>
            {
                entity.HasKey(e => new { e.Id });
            });

            modelBuilder.Entity<Translation>(entity =>
            {
                entity.HasKey(e => new { e.Id });
                entity.HasQueryFilter(x => x.IsDelete == false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
