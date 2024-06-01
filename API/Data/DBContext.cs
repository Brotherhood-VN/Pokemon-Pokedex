using Microsoft.EntityFrameworkCore;
using API.Models;

namespace API.Data
{
	public partial class DBContext : DbContext
    {
	    public DBContext() {}

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
		public virtual DbSet<Area> Area { get; set; }
		public virtual DbSet<Classification> Classification { get; set; }
		public virtual DbSet<Condition> Condition { get; set; }
		public virtual DbSet<Function> Function { get; set; }
		public virtual DbSet<Gender> Gender { get; set; }
		public virtual DbSet<Generation> Generation { get; set; }
		public virtual DbSet<Item> Item { get; set; }
		public virtual DbSet<Menu> Menu { get; set; }
		public virtual DbSet<Pokemon> Pokemon { get; set; }
		public virtual DbSet<PokemonClass> PokemonClass { get; set; }
		public virtual DbSet<PokemonGen> PokemonGen { get; set; }
		public virtual DbSet<PokemonSkill> PokemonSkill { get; set; }
		public virtual DbSet<Rank> Rank { get; set; }
		public virtual DbSet<RefreshToken> RefreshToken { get; set; }
		public virtual DbSet<Region> Region { get; set; }
		public virtual DbSet<Role> Role { get; set; }
		public virtual DbSet<RoleFunction> RoleFunction { get; set; }
		public virtual DbSet<Skill> Skill { get; set; }
		public virtual DbSet<SkillCondition> SkillCondition { get; set; }
		public virtual DbSet<Stat> Stat { get; set; }
		public virtual DbSet<Stone> Stone { get; set; }
		public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }

	    protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Ability>(entity =>
            {
                entity.HasKey(e => new { e.Id });
                entity.HasOne(d => d.Pokemon).WithMany(p => p.Abilitys).HasForeignKey(d => d.PokemonId).HasConstraintName("FK_Ability_Pokemon");
                entity.HasOne(d => d.Gender).WithMany(p => p.Abilitys).HasForeignKey(d => d.GenderId).HasConstraintName("FK_Ability_Gender");
                entity.HasOne(d => d.Area).WithMany(p => p.Abilitys).HasForeignKey(d => d.AreaId).HasConstraintName("FK_Ability_Area");
                entity.HasOne(d => d.Region).WithMany(p => p.Abilitys).HasForeignKey(d => d.RegionId).HasConstraintName("FK_Ability_Region");
            });

            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => new { e.Id });
                entity.HasOne(d => d.AccountType).WithMany(p => p.Accounts).HasForeignKey(d => d.AccountTypeId).HasConstraintName("FK_Account_AccountType");
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
            });

            modelBuilder.Entity<Against>(entity =>
            {
                entity.HasKey(e => new { e.Id });
                entity.HasOne(d => d.Pokemon).WithMany(p => p.Againsts).HasForeignKey(d => d.PokemonId).HasConstraintName("FK_Against_Pokemon");
            });

            modelBuilder.Entity<Area>(entity =>
            {
                entity.HasKey(e => new { e.Id });
            });

            modelBuilder.Entity<Classification>(entity =>
            {
                entity.HasKey(e => new { e.Id });
            });

            modelBuilder.Entity<Condition>(entity =>
            {
                entity.HasKey(e => new { e.Id });
            });

            modelBuilder.Entity<Function>(entity =>
            {
                entity.HasKey(e => new { e.Id });
            });

            modelBuilder.Entity<Gender>(entity =>
            {
                entity.HasKey(e => new { e.Id });
            });

            modelBuilder.Entity<Generation>(entity =>
            {
                entity.HasKey(e => new { e.Id });
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.HasKey(e => new { e.Id });
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
            });

            modelBuilder.Entity<PokemonClass>(entity =>
            {
                entity.HasKey(e => new { e.Id });
                entity.HasOne(d => d.Pokemon).WithMany(p => p.PokemonClasss).HasForeignKey(d => d.PokemonId).HasConstraintName("FK_PokemonClass_Pokemon");
                entity.HasOne(d => d.Classification).WithMany(p => p.PokemonClasss).HasForeignKey(d => d.ClassificationId).HasConstraintName("FK_PokemonClass_Classification");
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
                entity.HasOne(d => d.Pokemon).WithMany(p => p.PokemonSkills).HasForeignKey(d => d.PokemonId).HasConstraintName("FK_PokemonSkill_Pokemon");
                entity.HasOne(d => d.Skill).WithMany(p => p.PokemonSkills).HasForeignKey(d => d.SkillId).HasConstraintName("FK_PokemonSkill_Skill");
            });

            modelBuilder.Entity<Rank>(entity =>
            {
                entity.HasKey(e => new { e.Id });
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
                entity.HasOne(d => d.Area).WithMany(p => p.Regions).HasForeignKey(d => d.AreaId).HasConstraintName("FK_Region_Region");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => new { e.Id });
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
            });

            modelBuilder.Entity<SkillCondition>(entity =>
            {
                entity.HasKey(e => new { e.Id });
                entity.HasOne(d => d.Condition).WithMany(p => p.SkillConditions).HasForeignKey(d => d.ConditionId).HasConstraintName("FK_SkillCondition_Condition");
                entity.HasOne(d => d.Skill).WithMany(p => p.SkillConditions).HasForeignKey(d => d.SkillId).HasConstraintName("FK_SkillCondition_Skill");
            });

            modelBuilder.Entity<Stat>(entity =>
            {
                entity.HasKey(e => new { e.Id });
                entity.HasOne(d => d.Pokemon).WithMany(p => p.Stats).HasForeignKey(d => d.PokemonId).HasConstraintName("FK_Stat_Pokemon");
                entity.HasOne(d => d.Area).WithMany(p => p.Stats).HasForeignKey(d => d.AreaId).HasConstraintName("FK_Stat_Area");
                entity.HasOne(d => d.Region).WithMany(p => p.Stats).HasForeignKey(d => d.RegionId).HasConstraintName("FK_Stat_Region");
            });

            modelBuilder.Entity<Stone>(entity =>
            {
                entity.HasKey(e => new { e.Id });
            });

            modelBuilder.Entity<sysdiagrams>(entity =>
            {
                entity.HasKey(e => new { e.diagram_id });
            });

	        OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
