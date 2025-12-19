USE F1Season2025DB;
GO

CREATE PROCEDURE sp_InsertBoss
    @FirstName VARCHAR(255),
    @LastName VARCHAR(255),
    @Age INT,
    @Experience DECIMAL(4,3),
    @Status VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;

    BEGIN TRY
        INSERT INTO Staffs (FirstName, LastName, Age, Experience, [Status])
        VALUES (@FirstName, @LastName, @Age, @Experience, @Status);

        DECLARE @NewStaffId INT = SCOPE_IDENTITY();

        INSERT INTO Bosses (StaffId)
        VALUES (@NewStaffId);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

CREATE PROCEDURE sp_InsertDriver
    @FirstName VARCHAR(255),
    @LastName VARCHAR(255),
    @Age INT,
    @Experience DECIMAL(4,3),
    @Status VARCHAR(50),
    @DriverId INT,
    @PerformancePoints DECIMAL(5,2),
    @Handicap DECIMAL(5,2)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;

    BEGIN TRY
        INSERT INTO Staffs (FirstName, LastName, Age, Experience, [Status])
        VALUES (@FirstName, @LastName, @Age, @Experience, @Status);

        DECLARE @NewStaffId INT = SCOPE_IDENTITY();

        INSERT INTO Drivers (StaffId, DriverId, PerformancePoints, Handicap)
        VALUES (@NewStaffId, @DriverId, @PerformancePoints, @Handicap);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

CREATE PROCEDURE sp_InsertAerodynamicEngineer
    @FirstName VARCHAR(255),
    @LastName VARCHAR(255),
    @Age INT,
    @Experience DECIMAL(4,3),
    @Status VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;

    BEGIN TRY
        INSERT INTO Staffs (FirstName, LastName, Age, Experience, [Status])
        VALUES (@FirstName, @LastName, @Age, @Experience, @Status);

        DECLARE @NewStaffId INT = SCOPE_IDENTITY();

        INSERT INTO Engineers (StaffId)
        VALUES (@NewStaffId);

        DECLARE @NewEngineerId INT = SCOPE_IDENTITY();

        INSERT INTO AerodynamicEngineers (EngineerId)
        VALUES (@NewEngineerId);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

CREATE PROCEDURE sp_InsertPowerEngineer
    @FirstName VARCHAR(255),
    @LastName VARCHAR(255),
    @Age INT,
    @Experience DECIMAL(4,3),
    @Status VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;

    BEGIN TRY
        INSERT INTO Staffs (FirstName, LastName, Age, Experience, [Status])
        VALUES (@FirstName, @LastName, @Age, @Experience, @Status);

        DECLARE @NewStaffId INT = SCOPE_IDENTITY();

        INSERT INTO Engineers (StaffId)
        VALUES (@NewStaffId);

        DECLARE @NewEngineerId INT = SCOPE_IDENTITY();

        INSERT INTO PowerEngineers (EngineerId)
        VALUES (@NewEngineerId);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO