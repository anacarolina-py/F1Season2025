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

CREATE PROCEDURE sp_ChangeCarStatus
    @CarId INT,
    @NewStatus VARCHAR(7)
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;

        IF @NewStatus = 'Inativo'
        BEGIN
            UPDATE CarsAerodynamic 
            SET [Status] = @NewStatus 
            WHERE CarId = @CarId;

            UPDATE CarsPower       
            SET [Status] = @NewStatus 
            WHERE CarId = @CarId;

            UPDATE CarsDrivers    
            SET [Status] = @NewStatus
            WHERE CarId = @CarId;
            
            UPDATE TeamsCars
            SET [Status] = @NewStatus
            WHERE CarId = @CarId;
        END

        UPDATE Cars
        SET [Status] = @NewStatus
        WHERE CarId = @CarId;
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

CREATE PROCEDURE sp_ChangeBossStatus
    @BossId INT,
    @NewStatus VARCHAR(7)
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @StaffId INT;
        SELECT @StaffId = StaffId FROM Bosses WHERE BossId = @BossId;

        IF @NewStatus = 'Inativo'
        BEGIN
            UPDATE TeamsBosses 
            SET [Status] = @NewStatus 
            WHERE BossId = @BossId;

            UPDATE Staffs
            SET [Status] = @NewStatus
            WHERE StaffId = @StaffId;
        END
        ELSE
        BEGIN
            UPDATE Staffs
            SET [Status] = @NewStatus
            WHERE StaffId = @StaffId;
        END

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO


CREATE PROCEDURE sp_ChangeDriverStatus
    @DriverId INT,
    @NewStatus VARCHAR(7) 
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @StaffId INT;
        SELECT @StaffId = StaffId FROM Drivers WHERE DriverId = @DriverId;

        IF @NewStatus = 'Inativo'
        BEGIN
            UPDATE CarsDrivers 
            SET [Status] = @NewStatus
            WHERE DriverId = @DriverId;

            UPDATE TeamsDrivers
            SET [Status] = @NewStatus
            WHERE DriverId = @DriverId;

            UPDATE Staffs
            SET [Status] = @NewStatus
            WHERE StaffId = @StaffId;
        END
        ELSE IF @NewStatus = 'Ativo'
        BEGIN
            UPDATE Staffs
            SET [Status] = @NewStatus
            WHERE StaffId = @StaffId;
        END

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

CREATE PROCEDURE sp_ChangeAerodynamicEngineerStatus
    @AerodynamicEngineerId INT,
    @NewStatus VARCHAR(7)
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;

       DECLARE @StaffId INT, @EngineerId INT;
        
        SELECT @StaffId = e.StaffId, 
               @EngineerId = e.EngineerId
        FROM Engineers e
        JOIN AerodynamicEngineers ae ON e.EngineerId = ae.EngineerId
        WHERE ae.AerodynamicEngineerId = @AerodynamicEngineerId;

        IF @NewStatus = 'Inativo'
        BEGIN
            UPDATE TeamsAerodynamic 
            SET [Status] = @NewStatus 
            WHERE AerodynamicEngineerId = @AerodynamicEngineerId;

            UPDATE CarsAerodynamic  
            SET [Status] = @NewStatus 
            WHERE AerodynamicEngineerId = @AerodynamicEngineerId;

            UPDATE Staffs 
            SET [Status] = @NewStatus 
            WHERE StaffId = @StaffId;
        END

        ELSE IF @NewStatus = 'Ativo'
        BEGIN
            UPDATE Staffs 
            SET [Status] = @NewStatus 
            WHERE StaffId = @StaffId;
        END

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

CREATE PROCEDURE sp_ChangePowerEngineerStatus
    @PowerEngineerId INT,
    @NewStatus VARCHAR(7)
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @StaffId INT, @EngineerId INT;
        
        SELECT @StaffId = e.StaffId, 
               @EngineerId = e.EngineerId
        FROM Engineers e
        JOIN PowerEngineers pe ON e.EngineerId = pe.EngineerId
        WHERE pe.PowerEngineerId = @PowerEngineerId;

        IF @NewStatus = 'Inativo'
        BEGIN
            UPDATE TeamsPower 
            SET [Status] = @NewStatus 
            WHERE PowerEngineerId = @PowerEngineerId;

            UPDATE CarsPower  
            SET [Status] = @NewStatus 
            WHERE PowerEngineerId = @PowerEngineerId;

            UPDATE Staffs 
            SET [Status] = @NewStatus 
            WHERE StaffId = @StaffId;
        END

        ELSE IF @NewStatus = 'Ativo'
        BEGIN
            UPDATE Staffs 
            SET [Status] = @NewStatus 
            WHERE StaffId = @StaffId;
        END

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

CREATE PROCEDURE sp_TurnOffTeam
    @TeamId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
            UPDATE TeamsBosses 
            SET [Status] = 'Inativo' 
            WHERE TeamId = @TeamId;

            UPDATE TeamsCars 
            SET [Status] = 'Inativo' 
            WHERE TeamId = @TeamId;

            UPDATE CarsAerodynamic 
            SET [Status] = 'Inativo' 
            WHERE CarId IN (SELECT CarId 
                            FROM TeamsCars 
                            WHERE TeamId = @TeamId);

            UPDATE CarsPower 
            SET [Status] = 'Inativo' 
            WHERE CarId IN (SELECT CarId 
                            FROM TeamsCars
                            WHERE TeamId = @TeamId);

            UPDATE CarsDrivers 
            SET [Status] = 'Inativo' 
            WHERE CarId IN (SELECT CarId 
                            FROM TeamsCars 
                            WHERE TeamId = @TeamId);

            UPDATE TeamsDrivers
            SET [Status] = 'Inativo' 
            WHERE TeamId = @TeamId;

            UPDATE TeamsAerodynamic
            SET [Status] = 'Inativo' 
            WHERE TeamId = @TeamId;

            UPDATE TeamsPower 
            SET [Status] = 'Inativo' 
            WHERE TeamId = @TeamId;

            UPDATE Teams
            SET [Status] = 'Inativo'
            WHERE TeamId = @TeamId;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO