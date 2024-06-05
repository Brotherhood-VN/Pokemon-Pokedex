USE [Pokedex]
GO
/****** Object:  Table [dbo].[Ability]    Script Date: 05/06/2024 15:35:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ability](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[PokemonId] [bigint] NOT NULL,
	[GenderId] [bigint] NULL,
	[AreaId] [bigint] NULL,
	[RegionId] [bigint] NULL,
	[Height] [decimal](7, 2) NULL,
	[Weight] [decimal](7, 2) NULL,
 CONSTRAINT [PK_Ability] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Account]    Script Date: 05/06/2024 15:35:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Account](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](1000) NOT NULL,
	[Password] [nvarchar](1000) NOT NULL,
	[Status] [bit] NULL,
	[IsDelete] [bit] NULL,
	[CreateBy] [bigint] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdateBy] [bigint] NULL,
	[UpdateTime] [datetime] NULL,
	[AccountTypeId] [bigint] NOT NULL,
 CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AccountFunction]    Script Date: 05/06/2024 15:35:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccountFunction](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[AccountId] [bigint] NOT NULL,
	[FunctionId] [bigint] NOT NULL,
 CONSTRAINT [PK_AccountFunction] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AccountRole]    Script Date: 05/06/2024 15:35:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccountRole](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[AccountId] [bigint] NOT NULL,
	[RoleId] [bigint] NOT NULL,
 CONSTRAINT [PK_AccountRole] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AccountType]    Script Date: 05/06/2024 15:35:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccountType](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](10) NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Status] [bit] NOT NULL,
	[IsDelete] [bit] NOT NULL,
	[CreateBy] [bigint] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdateBy] [bigint] NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_AccountType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Against]    Script Date: 05/06/2024 15:35:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Against](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[PokemonId] [bigint] NOT NULL,
	[Good] [nvarchar](max) NOT NULL,
	[Weak] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Against] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Area]    Script Date: 05/06/2024 15:35:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Area](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](10) NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Status] [bit] NOT NULL,
	[IsDelete] [bit] NOT NULL,
	[CreateBy] [bigint] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdateBy] [bigint] NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_Area] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Classification]    Script Date: 05/06/2024 15:35:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Classification](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](10) NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Status] [bit] NOT NULL,
	[IsDelete] [bit] NOT NULL,
	[CreateBy] [bigint] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdateBy] [bigint] NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_Classification] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Condition]    Script Date: 05/06/2024 15:35:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Condition](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](10) NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Status] [bit] NOT NULL,
	[IsDelete] [bit] NOT NULL,
	[CreateBy] [bigint] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdateBy] [bigint] NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_Condition] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Function]    Script Date: 05/06/2024 15:35:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Function](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Area] [nvarchar](250) NOT NULL,
	[Controller] [nvarchar](250) NOT NULL,
	[Action] [nvarchar](250) NOT NULL,
	[Title] [nvarchar](250) NULL,
	[Description] [nvarchar](250) NULL,
	[IsShow] [bit] NOT NULL,
	[IsMenu] [bit] NULL,
	[Seq] [int] NULL,
	[IsDelete] [bit] NULL,
	[CreateBy] [bigint] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdateBy] [bigint] NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_Function] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Gender]    Script Date: 05/06/2024 15:35:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Gender](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](10) NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Status] [bit] NOT NULL,
	[IsDelete] [bit] NOT NULL,
	[CreateBy] [bigint] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdateBy] [bigint] NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_Gender] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Generation]    Script Date: 05/06/2024 15:35:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Generation](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](10) NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Status] [bit] NOT NULL,
	[IsDelete] [bit] NOT NULL,
	[CreateBy] [bigint] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdateBy] [bigint] NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_Generation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Item]    Script Date: 05/06/2024 15:35:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Item](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](10) NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Status] [bit] NOT NULL,
	[IsDelete] [bit] NOT NULL,
	[CreateBy] [bigint] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdateBy] [bigint] NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_Item] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Menu]    Script Date: 05/06/2024 15:35:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Menu](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Controller] [nvarchar](250) NULL,
	[Label] [nvarchar](100) NULL,
	[Icon] [nvarchar](100) NULL,
	[Url] [nvarchar](500) NULL,
	[RouterLink] [nvarchar](500) NULL,
	[Visible] [bit] NOT NULL,
	[Separator] [bit] NOT NULL,
	[Target] [nvarchar](50) NULL,
	[Badge] [nvarchar](100) NULL,
	[Title] [nvarchar](250) NULL,
	[Class] [nvarchar](250) NULL,
	[Seq] [int] NOT NULL,
	[ParentId] [bigint] NULL,
 CONSTRAINT [PK_Menu] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Pokemon]    Script Date: 05/06/2024 15:35:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pokemon](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Index] [nvarchar](50) NOT NULL,
	[FullName] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Story] [nvarchar](max) NULL,
	[Note] [nvarchar](max) NULL,
	[BeforeIndex] [nvarchar](50) NULL,
	[RankId] [bigint] NULL,
	[ConditionId] [bigint] NULL,
	[Level] [int] NULL,
	[StoneId] [bigint] NULL,
	[Status] [bit] NOT NULL,
	[IsDelete] [bit] NOT NULL,
	[CreateBy] [bigint] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdateBy] [bigint] NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_Pokemon] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PokemonClass]    Script Date: 05/06/2024 15:35:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PokemonClass](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[PokemonId] [bigint] NOT NULL,
	[ClassificationId] [bigint] NOT NULL,
	[IsDefault] [bit] NOT NULL,
 CONSTRAINT [PK_PokemonClass] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PokemonGen]    Script Date: 05/06/2024 15:35:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PokemonGen](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[PokemonId] [bigint] NOT NULL,
	[GenerationId] [bigint] NOT NULL,
 CONSTRAINT [PK_PokemonGen] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PokemonSkill]    Script Date: 05/06/2024 15:35:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PokemonSkill](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[PokemonId] [bigint] NOT NULL,
	[SkillId] [bigint] NOT NULL,
 CONSTRAINT [PK_PokemonSkill] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Rank]    Script Date: 05/06/2024 15:35:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rank](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](10) NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Status] [bit] NOT NULL,
	[IsDelete] [bit] NOT NULL,
	[CreateBy] [bigint] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdateBy] [bigint] NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_Rank] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RefreshToken]    Script Date: 05/06/2024 15:35:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RefreshToken](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Token] [varchar](max) NOT NULL,
	[Expires] [datetime] NULL,
	[CreatedTime] [datetime] NULL,
	[RevokedTime] [datetime] NULL,
	[ReplacedByToken] [varchar](max) NULL,
	[ReasonRevoked] [varchar](max) NULL,
	[AccountId] [bigint] NOT NULL,
 CONSTRAINT [PK_RefreshToken] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Region]    Script Date: 05/06/2024 15:35:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Region](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[AreaId] [bigint] NOT NULL,
	[Code] [nvarchar](10) NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Status] [bit] NOT NULL,
	[IsDelete] [bit] NOT NULL,
	[CreateBy] [bigint] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdateBy] [bigint] NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_Region] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 05/06/2024 15:35:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](10) NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Status] [bit] NOT NULL,
	[IsDelete] [bit] NOT NULL,
	[CreateBy] [bigint] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdateBy] [bigint] NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RoleFunction]    Script Date: 05/06/2024 15:35:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RoleFunction](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[RoleId] [bigint] NOT NULL,
	[FunctionId] [bigint] NOT NULL,
 CONSTRAINT [PK_RoleFunction] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Skill]    Script Date: 05/06/2024 15:35:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Skill](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](10) NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Level] [int] NULL,
	[ItemId] [bigint] NULL,
	[Status] [bit] NOT NULL,
	[IsDelete] [bit] NOT NULL,
	[CreateBy] [bigint] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdateBy] [bigint] NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_Skill] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SkillCondition]    Script Date: 05/06/2024 15:35:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SkillCondition](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[SkillId] [bigint] NOT NULL,
	[ConditionId] [bigint] NOT NULL,
 CONSTRAINT [PK_SkillCondition] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Stat]    Script Date: 05/06/2024 15:35:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Stat](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[PokemonId] [bigint] NOT NULL,
	[AreaId] [bigint] NULL,
	[RegionId] [bigint] NULL,
	[Attack] [int] NOT NULL,
	[Defence] [int] NOT NULL,
	[Speed] [int] NOT NULL,
	[SpeedAttack] [int] NOT NULL,
	[SpeedDefence] [int] NOT NULL,
 CONSTRAINT [PK_Stat] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Stone]    Script Date: 05/06/2024 15:35:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Stone](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](10) NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Status] [bit] NOT NULL,
	[IsDelete] [bit] NOT NULL,
	[CreateBy] [bigint] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdateBy] [bigint] NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_Stone] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Ability]  WITH CHECK ADD  CONSTRAINT [FK_Ability_Area] FOREIGN KEY([AreaId])
REFERENCES [dbo].[Area] ([Id])
GO
ALTER TABLE [dbo].[Ability] CHECK CONSTRAINT [FK_Ability_Area]
GO
ALTER TABLE [dbo].[Ability]  WITH CHECK ADD  CONSTRAINT [FK_Ability_Gender] FOREIGN KEY([GenderId])
REFERENCES [dbo].[Gender] ([Id])
GO
ALTER TABLE [dbo].[Ability] CHECK CONSTRAINT [FK_Ability_Gender]
GO
ALTER TABLE [dbo].[Ability]  WITH CHECK ADD  CONSTRAINT [FK_Ability_Pokemon] FOREIGN KEY([PokemonId])
REFERENCES [dbo].[Pokemon] ([Id])
GO
ALTER TABLE [dbo].[Ability] CHECK CONSTRAINT [FK_Ability_Pokemon]
GO
ALTER TABLE [dbo].[Ability]  WITH CHECK ADD  CONSTRAINT [FK_Ability_Region] FOREIGN KEY([RegionId])
REFERENCES [dbo].[Region] ([Id])
GO
ALTER TABLE [dbo].[Ability] CHECK CONSTRAINT [FK_Ability_Region]
GO
ALTER TABLE [dbo].[Account]  WITH CHECK ADD  CONSTRAINT [FK_Account_AccountType] FOREIGN KEY([AccountTypeId])
REFERENCES [dbo].[AccountType] ([Id])
GO
ALTER TABLE [dbo].[Account] CHECK CONSTRAINT [FK_Account_AccountType]
GO
ALTER TABLE [dbo].[AccountFunction]  WITH CHECK ADD  CONSTRAINT [FK_AccountFunction_Account] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Account] ([Id])
GO
ALTER TABLE [dbo].[AccountFunction] CHECK CONSTRAINT [FK_AccountFunction_Account]
GO
ALTER TABLE [dbo].[AccountFunction]  WITH CHECK ADD  CONSTRAINT [FK_AccountFunction_Function] FOREIGN KEY([FunctionId])
REFERENCES [dbo].[Function] ([Id])
GO
ALTER TABLE [dbo].[AccountFunction] CHECK CONSTRAINT [FK_AccountFunction_Function]
GO
ALTER TABLE [dbo].[AccountRole]  WITH CHECK ADD  CONSTRAINT [FK_AccountRole_Account] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Account] ([Id])
GO
ALTER TABLE [dbo].[AccountRole] CHECK CONSTRAINT [FK_AccountRole_Account]
GO
ALTER TABLE [dbo].[AccountRole]  WITH CHECK ADD  CONSTRAINT [FK_AccountRole_Role] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Role] ([Id])
GO
ALTER TABLE [dbo].[AccountRole] CHECK CONSTRAINT [FK_AccountRole_Role]
GO
ALTER TABLE [dbo].[Against]  WITH CHECK ADD  CONSTRAINT [FK_Against_Pokemon] FOREIGN KEY([PokemonId])
REFERENCES [dbo].[Pokemon] ([Id])
GO
ALTER TABLE [dbo].[Against] CHECK CONSTRAINT [FK_Against_Pokemon]
GO
ALTER TABLE [dbo].[Pokemon]  WITH CHECK ADD  CONSTRAINT [FK_Pokemon_Rank] FOREIGN KEY([RankId])
REFERENCES [dbo].[Rank] ([Id])
GO
ALTER TABLE [dbo].[Pokemon] CHECK CONSTRAINT [FK_Pokemon_Rank]
GO
ALTER TABLE [dbo].[Pokemon]  WITH CHECK ADD  CONSTRAINT [FK_Pokemon_Stone] FOREIGN KEY([StoneId])
REFERENCES [dbo].[Stone] ([Id])
GO
ALTER TABLE [dbo].[Pokemon] CHECK CONSTRAINT [FK_Pokemon_Stone]
GO
ALTER TABLE [dbo].[PokemonClass]  WITH CHECK ADD  CONSTRAINT [FK_PokemonClass_Classification] FOREIGN KEY([ClassificationId])
REFERENCES [dbo].[Classification] ([Id])
GO
ALTER TABLE [dbo].[PokemonClass] CHECK CONSTRAINT [FK_PokemonClass_Classification]
GO
ALTER TABLE [dbo].[PokemonClass]  WITH CHECK ADD  CONSTRAINT [FK_PokemonClass_Pokemon] FOREIGN KEY([PokemonId])
REFERENCES [dbo].[Pokemon] ([Id])
GO
ALTER TABLE [dbo].[PokemonClass] CHECK CONSTRAINT [FK_PokemonClass_Pokemon]
GO
ALTER TABLE [dbo].[PokemonGen]  WITH CHECK ADD  CONSTRAINT [FK_PokemonGen_Generation] FOREIGN KEY([GenerationId])
REFERENCES [dbo].[Generation] ([Id])
GO
ALTER TABLE [dbo].[PokemonGen] CHECK CONSTRAINT [FK_PokemonGen_Generation]
GO
ALTER TABLE [dbo].[PokemonGen]  WITH CHECK ADD  CONSTRAINT [FK_PokemonGen_Pokemon] FOREIGN KEY([PokemonId])
REFERENCES [dbo].[Pokemon] ([Id])
GO
ALTER TABLE [dbo].[PokemonGen] CHECK CONSTRAINT [FK_PokemonGen_Pokemon]
GO
ALTER TABLE [dbo].[PokemonSkill]  WITH CHECK ADD  CONSTRAINT [FK_PokemonSkill_Pokemon] FOREIGN KEY([PokemonId])
REFERENCES [dbo].[Pokemon] ([Id])
GO
ALTER TABLE [dbo].[PokemonSkill] CHECK CONSTRAINT [FK_PokemonSkill_Pokemon]
GO
ALTER TABLE [dbo].[PokemonSkill]  WITH CHECK ADD  CONSTRAINT [FK_PokemonSkill_Skill] FOREIGN KEY([SkillId])
REFERENCES [dbo].[Skill] ([Id])
GO
ALTER TABLE [dbo].[PokemonSkill] CHECK CONSTRAINT [FK_PokemonSkill_Skill]
GO
ALTER TABLE [dbo].[Region]  WITH CHECK ADD  CONSTRAINT [FK_Region_Region] FOREIGN KEY([AreaId])
REFERENCES [dbo].[Area] ([Id])
GO
ALTER TABLE [dbo].[Region] CHECK CONSTRAINT [FK_Region_Region]
GO
ALTER TABLE [dbo].[RoleFunction]  WITH CHECK ADD  CONSTRAINT [FK_RoleFunction_Function] FOREIGN KEY([FunctionId])
REFERENCES [dbo].[Function] ([Id])
GO
ALTER TABLE [dbo].[RoleFunction] CHECK CONSTRAINT [FK_RoleFunction_Function]
GO
ALTER TABLE [dbo].[RoleFunction]  WITH CHECK ADD  CONSTRAINT [FK_RoleFunction_Role] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Role] ([Id])
GO
ALTER TABLE [dbo].[RoleFunction] CHECK CONSTRAINT [FK_RoleFunction_Role]
GO
ALTER TABLE [dbo].[Skill]  WITH CHECK ADD  CONSTRAINT [FK_Skill_Item] FOREIGN KEY([ItemId])
REFERENCES [dbo].[Item] ([Id])
GO
ALTER TABLE [dbo].[Skill] CHECK CONSTRAINT [FK_Skill_Item]
GO
ALTER TABLE [dbo].[SkillCondition]  WITH CHECK ADD  CONSTRAINT [FK_SkillCondition_Condition] FOREIGN KEY([ConditionId])
REFERENCES [dbo].[Condition] ([Id])
GO
ALTER TABLE [dbo].[SkillCondition] CHECK CONSTRAINT [FK_SkillCondition_Condition]
GO
ALTER TABLE [dbo].[SkillCondition]  WITH CHECK ADD  CONSTRAINT [FK_SkillCondition_Skill] FOREIGN KEY([SkillId])
REFERENCES [dbo].[Skill] ([Id])
GO
ALTER TABLE [dbo].[SkillCondition] CHECK CONSTRAINT [FK_SkillCondition_Skill]
GO
ALTER TABLE [dbo].[Stat]  WITH CHECK ADD  CONSTRAINT [FK_Stat_Area] FOREIGN KEY([AreaId])
REFERENCES [dbo].[Area] ([Id])
GO
ALTER TABLE [dbo].[Stat] CHECK CONSTRAINT [FK_Stat_Area]
GO
ALTER TABLE [dbo].[Stat]  WITH CHECK ADD  CONSTRAINT [FK_Stat_Pokemon] FOREIGN KEY([PokemonId])
REFERENCES [dbo].[Pokemon] ([Id])
GO
ALTER TABLE [dbo].[Stat] CHECK CONSTRAINT [FK_Stat_Pokemon]
GO
ALTER TABLE [dbo].[Stat]  WITH CHECK ADD  CONSTRAINT [FK_Stat_Region] FOREIGN KEY([RegionId])
REFERENCES [dbo].[Region] ([Id])
GO
ALTER TABLE [dbo].[Stat] CHECK CONSTRAINT [FK_Stat_Region]
GO
