USE [db_accountingCartridges]
GO
/****** Object:  Table [dbo].[Device]    Script Date: 20.05.2023 0:31:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Device](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [varchar](100) NOT NULL,
	[TypeId] [int] NOT NULL,
	[Count] [int] NOT NULL,
	[Сabinet] [varchar](20) NOT NULL,
	[DateAcceptance] [date] NOT NULL,
	[SerialNumber] [varchar](100) NULL,
	[StatusId] [int] NOT NULL,
	[FIO] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Device] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Status]    Script Date: 20.05.2023 0:31:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Status](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Status] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Type]    Script Date: 20.05.2023 0:31:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Type](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [varchar](100) NOT NULL,
 CONSTRAINT [PK_Type] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Device]  WITH CHECK ADD  CONSTRAINT [FK_Device_Status] FOREIGN KEY([StatusId])
REFERENCES [dbo].[Status] ([id])
GO
ALTER TABLE [dbo].[Device] CHECK CONSTRAINT [FK_Device_Status]
GO
ALTER TABLE [dbo].[Device]  WITH CHECK ADD  CONSTRAINT [FK_Device_Type] FOREIGN KEY([TypeId])
REFERENCES [dbo].[Type] ([id])
GO
ALTER TABLE [dbo].[Device] CHECK CONSTRAINT [FK_Device_Type]
GO
