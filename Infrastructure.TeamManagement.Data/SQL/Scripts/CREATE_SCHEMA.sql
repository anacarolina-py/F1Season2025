CREATE DATABASE F1Season2025DB;
GO

USE F1Season2025DB;
GO

CREATE TABLE [Teams] (
	[TeamId] INTEGER NOT NULL PRIMARY KEY IDENTITY(1,1),
	[Name] NVARCHAR(50) NOT NULL UNIQUE,
	[Status] NVARCHAR(7) NOT NULL DEFAULT 'Inativo'
);
GO

CREATE TABLE [Cars] (
	[CarId] INTEGER NOT NULL PRIMARY KEY IDENTITY(1,1),
	[Model] NVARCHAR(5) NOT NULL,
	[AerodynamicCoefficient] DECIMAL(5,3) NOT NULL DEFAULT 5.0,
	[PowerCoefficient] DECIMAL(5,3) NOT NULL DEFAULT 5.0,
	[Weight] DECIMAL(5,2) NOT NULL DEFAULT 700.00,
	[Status] NVARCHAR(7) NOT NULL DEFAULT 'Inativo'
);
GO

CREATE TABLE [Staffs] (
	[StaffId] INTEGER NOT NULL PRIMARY KEY IDENTITY(1,1) ,
	[FirstName] NVARCHAR(255) NOT NULL,
	[LastName] NVARCHAR(255) NOT NULL,
	[Age] INTEGER NOT NULL,
	[Experience] DECIMAL(4,3) NOT NULL DEFAULT 1.0,
	[Status] NVARCHAR(7) NOT NULL DEFAULT 'Inativo'
);
GO

CREATE TABLE [Bosses] (
	[BossId] INTEGER NOT NULL PRIMARY KEY IDENTITY(1,1),
	[StaffId] INTEGER NOT NULL UNIQUE,
	FOREIGN KEY([StaffId]) REFERENCES [Staffs]([StaffId])
);
GO

CREATE TABLE [Drivers] (
	[StaffId] INTEGER NOT NULL,
	[DriverId] INTEGER NOT NULL UNIQUE,
	[PerformancePoints] DECIMAL(6,3) NOT NULL DEFAULT 000.000,
	[Handicap] DECIMAL(5,2) NOT NULL DEFAULT 100.00,
	FOREIGN KEY([StaffId]) REFERENCES [Staffs]([StaffId]),
	PRIMARY KEY([StaffId])
);
GO

CREATE TABLE [Engineers] (
	[EngineerId] INTEGER NOT NULL PRIMARY KEY IDENTITY(1,1) ,
	[StaffId] INTEGER NOT NULL UNIQUE,
	FOREIGN KEY([StaffId]) REFERENCES [Staffs]([StaffId])
);
GO

CREATE TABLE [AerodynamicEngineers] (
	[AerodynamicEngineerId ] INTEGER NOT NULL PRIMARY KEY IDENTITY(1,1) ,
	[EngineerId] INTEGER NOT NULL UNIQUE,
	FOREIGN KEY([EngineerId]) REFERENCES [Engineers]([EngineerId])
);
GO

CREATE TABLE [PowerEngineers] (
	[PowerEngineerId] INTEGER NOT NULL PRIMARY KEY IDENTITY(1,1) ,
	[EngineerId ] INTEGER NOT NULL UNIQUE,
	FOREIGN KEY([EngineerId]) REFERENCES [Engineers]([EngineerId])
);
GO

CREATE TABLE [TeamsCars] (
	[TeamId] INTEGER NOT NULL,
	[CarId] INTEGER NOT NULL,
	[Status] NVARCHAR(7) NOT NULL DEFAULT 'Ativo',
	FOREIGN KEY([TeamId]) REFERENCES [Teams]([TeamId]),
	FOREIGN KEY([CarId]) REFERENCES [Cars]([CarId]),
	PRIMARY KEY([TeamId], [CarId])
);
GO

CREATE TABLE [CarsAerodynamic] (
	[AerodynamicEngineerId] INTEGER NOT NULL,
	[CarId] INTEGER NOT NULL,
	[Status] NVARCHAR(7) NOT NULL DEFAULT 'Ativo',
	FOREIGN KEY([AerodynamicEngineerId]) REFERENCES [AerodynamicEngineers]([AerodynamicEngineerId]),
	FOREIGN KEY([CarId]) REFERENCES [Cars]([CarId]),
	PRIMARY KEY([AerodynamicEngineerId], [CarId])
);
GO

CREATE TABLE [CarsPower] (
	[PowerEngineerId] INTEGER NOT NULL,
	[CarId] INTEGER NOT NULL,
	[Status] NVARCHAR(7) NOT NULL DEFAULT 'Ativo',
	FOREIGN KEY([PowerEngineerId]) REFERENCES [PowerEngineers]([PowerEngineerId]),
	FOREIGN KEY([CarId]) REFERENCES [Cars]([CarId]),
	PRIMARY KEY([PowerEngineerId], [CarId])
);
GO

CREATE TABLE [CarsDrivers] (
	[DriverId] INTEGER NOT NULL,
	[CarId] INTEGER NOT NULL,
	[Status] NVARCHAR(7) NOT NULL DEFAULT 'Ativo',
	FOREIGN KEY([DriverId]) REFERENCES [Drivers]([DriverId]),
	FOREIGN KEY([CarId]) REFERENCES [Cars]([CarId]),
	PRIMARY KEY([DriverId], [CarId])
);
GO

CREATE TABLE [TeamBosses] (
	[TeamId] INTEGER NOT NULL,
	[BossId] INTEGER NOT NULL,
	[Status] NVARCHAR(7) NOT NULL DEFAULT 'Ativo',
	FOREIGN KEY([TeamId]) REFERENCES [Teams]([TeamId]),
	FOREIGN KEY([BossId]) REFERENCES [Bosses]([BossId]),
	PRIMARY KEY([TeamId], [BossId])
);
GO

CREATE TABLE [TeamsDrivers] (
	[TeamId] INTEGER NOT NULL,
	[DriverId] INTEGER NOT NULL,
	[Status] NVARCHAR(7) NOT NULL DEFAULT 'Ativo',
	FOREIGN KEY([TeamId]) REFERENCES [Teams]([TeamId]),
	FOREIGN KEY([DriverId]) REFERENCES [Drivers]([DriverId]),
	PRIMARY KEY([TeamId], [DriverId])
);
GO

CREATE TABLE [TeamsAerodynamic] (
	[TeamId] INTEGER NOT NULL,
	[AerodynamicEngineerId] INTEGER NOT NULL,
	[Status] NVARCHAR(7) NOT NULL DEFAULT 'Ativo',
	FOREIGN KEY([TeamId]) REFERENCES [Teams]([TeamId]),
	FOREIGN KEY([AerodynamicEngineerId]) REFERENCES [AerodynamicEngineers]([AerodynamicEngineerId]),
	PRIMARY KEY([TeamId], [AerodynamicEngineerId])
);
GO

CREATE TABLE [TeamsPower] (
	[TeamId] INTEGER NOT NULL,
	[PowerEngineerId] INTEGER NOT NULL,
	[Status] NVARCHAR(7) NOT NULL DEFAULT 'Ativo',
	FOREIGN KEY([TeamId]) REFERENCES [Teams]([TeamId]),
	FOREIGN KEY([PowerEngineerId]) REFERENCES [PowerEngineers]([PowerEngineerId]),
	PRIMARY KEY([TeamId], [PowerEngineerId])
);
GO
