USE F1Season2025DB;
GO

CREATE OR ALTER TRIGGER TR_ValidateTeam
ON Teams
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 
               FROM inserted 
               WHERE LEN([Name]) < 3 OR 
                     LEN([Name]) > 50)
    BEGIN
        RAISERROR ('Failure: Team name must be between 3 and 50 characters.', 16, 1);
        ROLLBACK TRANSACTION; 
        RETURN;
    END

    IF EXISTS (SELECT 1 
               FROM inserted 
               WHERE [Status] = 'Ativo')
    BEGIN
        
        IF (SELECT COUNT(*)
            FROM Teams
            WHERE [Status] = 'Ativo') > 11
        BEGIN
            RAISERROR ('Failure: Limit of 11 active teams exceeded.', 16, 1);
            ROLLBACK TRANSACTION; 
            RETURN;
        END

        IF EXISTS (SELECT 1 
                   FROM inserted t
                   WHERE ((SELECT COUNT(*) 
                           FROM TeamsCars tc 
                           JOIN Cars c ON tc.CarId = c.CarId 
                           WHERE tc.TeamId = t.TeamId AND 
                                 tc.[Status] = 'Ativo' AND 
                                 c.[Status] = 'Ativo') != 2 OR

                  (SELECT COUNT(*) 
                   FROM TeamBosses tb 
                   JOIN Staffs s ON (SELECT StaffId 
                                     FROM Bosses 
                                     WHERE BossId = tb.BossId) = s.StaffId
                   WHERE tb.TeamId = t.TeamId AND 
                         tb.[Status] = 'Ativo' AND
                         s.[Status] = 'Ativo') != 2 OR

                  (SELECT COUNT(*) 
                   FROM TeamsDrivers td 
                   JOIN Staffs s ON (SELECT StaffId 
                                     FROM Drivers 
                                     WHERE DriverId = td.DriverId) = s.StaffId
                   WHERE td.TeamId = t.TeamId AND 
                         td.[Status] = 'Ativo' AND 
                         s.[Status] = 'Ativo') != 2 OR

                  (SELECT COUNT(*) 
                   FROM TeamsAerodynamic ta 
                   JOIN Staffs s ON (SELECT StaffId 
                                     FROM Engineers
                                     WHERE EngineerId = (SELECT EngineerId 
                                                         FROM AerodynamicEngineers
                                                         WHERE AerodynamicEngineerId = ta.AerodynamicEngineerId)) = s.StaffId
                   WHERE ta.TeamId = t.TeamId AND 
                         ta.[Status] = 'Ativo' AND 
                         s.[Status] = 'Ativo') != 2 OR

                  (SELECT COUNT(*) 
                   FROM TeamsPower tp 
                   JOIN Staffs s ON (SELECT StaffId 
                                     FROM Engineers 
                                     WHERE EngineerId = (SELECT EngineerId 
                                                         FROM PowerEngineers
                                                         WHERE PowerEngineerId = tp.PowerEngineerId)) = s.StaffId
                   WHERE tp.TeamId = t.TeamId AND 
                         tp.[Status] = 'Ativo' AND 
                         s.[Status] = 'Ativo') != 2
            )
        )
        BEGIN
            RAISERROR ('Failure: Team can only be Active if it has 10 active records (2 per category) in both relational and main tables.', 16, 1);
            ROLLBACK TRANSACTION; 
            RETURN;
        END
    END
END;
GO

CREATE OR ALTER TRIGGER TR_ValidateCars
ON Cars
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT 1 FROM inserted 
        WHERE [Model] NOT LIKE '[A-Z][A-Z][A-Z][0-9][0-9]' COLLATE Latin1_General_BIN
    )
    BEGIN
        RAISERROR ('Failure: Model must follow the pattern ABC12 (3 uppercase letters and 2 digits).', 16, 1);
        ROLLBACK TRANSACTION; 
        RETURN;
    END

    IF EXISTS (
        SELECT [Model] 
        FROM Cars 
        WHERE [Model] IN (SELECT [Model] 
                          FROM inserted)
        GROUP BY [Model] 
        HAVING COUNT(*) > 2
    )
    BEGIN
        RAISERROR ('Failure: Each model can have a maximum of 2 records.', 16, 1);
        ROLLBACK TRANSACTION; 
        RETURN;
    END

    IF EXISTS (
        SELECT 1
        FROM inserted 
        WHERE [Weight] < 700.00 OR 
              [Weight] > 1000.00
    )
    BEGIN
        RAISERROR ('Failure: Weight must be between 700.00 and 1000.00.', 16, 1);
        ROLLBACK TRANSACTION; RETURN;
    END

    IF EXISTS (
        SELECT 1 
        FROM inserted 
        WHERE [AerodynamicCoefficient] < 0.000 OR 
              [AerodynamicCoefficient] > 10.000 OR 
              [PowerCoefficient] < 0.000 OR 
              [PowerCoefficient] > 10.000
    )
    BEGIN
        RAISERROR ('Failure: Coefficients must be between 0.000 and 10.000.', 16, 1);
        ROLLBACK TRANSACTION; 
        RETURN;
    END
END;
GO

CREATE OR ALTER TRIGGER TR_ValidateStaffs
ON Staffs
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT 1 
        FROM inserted 
        WHERE LEN(FirstName) < 3 OR 
              LEN(FirstName) > 255 OR 
              LEN(LastName) < 3 OR 
              LEN(LastName) > 255
    )
    BEGIN
        RAISERROR ('Failure: First name and last name must be between 3 and 255 characters.', 16, 1);
        ROLLBACK TRANSACTION; 
        RETURN;
    END

    IF EXISTS (
        SELECT 1 
        FROM inserted 
        WHERE FirstName LIKE '%[0-9]%' OR 
              LastName LIKE '%[0-9]%'
    )
    BEGIN
        RAISERROR ('Failure: Names cannot contain digits.', 16, 1);
        ROLLBACK TRANSACTION; 
        RETURN;
    END

    IF EXISTS (
        SELECT 1 
        FROM inserted 
        WHERE Age < 17 OR 
              Age > 120
    )
    BEGIN
        RAISERROR ('Failure: Age must be between 17 and 120.', 16, 1);
        ROLLBACK TRANSACTION; RETURN;
    END

    IF EXISTS (
        SELECT 1 
        FROM inserted 
        WHERE Experience < 1.000 OR 
              Experience > 5.000
    )
    BEGIN
        RAISERROR ('Failure: Experience must be between 1.000 and 5.000.', 16, 1);
        ROLLBACK TRANSACTION; 
        RETURN;
    END
END;
GO

CREATE OR ALTER TRIGGER TR_ValidateSpecialization
ON Staffs
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    IF EXISTS (SELECT 1 
               FROM inserted 
               WHERE [Status] = 'Ativo')
    BEGIN
        IF EXISTS (
            SELECT 1 
            FROM inserted i
            WHERE NOT EXISTS (SELECT 1 
                              FROM Bosses 
                              WHERE StaffId = i.StaffId) AND 
                  NOT EXISTS (SELECT 1 
                              FROM Drivers 
                              WHERE StaffId = i.StaffId) AND 
                  NOT EXISTS (SELECT 1 
                              FROM Engineers 
                              WHERE StaffId = i.StaffId)
        )
        BEGIN
            RAISERROR ('Failure: Staff must be assigned to a specialty (Boss, Driver, or Engineer) before being Active.', 16, 1);
            ROLLBACK TRANSACTION; 
            RETURN;
        END
    END
END;
GO

CREATE OR ALTER TRIGGER TR_ValidateBossUniqueness
ON Bosses
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT 1
        FROM inserted i
        WHERE EXISTS (SELECT 1 
                      FROM Drivers 
                      WHERE StaffId = i.StaffId) OR 
              EXISTS (SELECT 1 
                      FROM Engineers 
                      WHERE StaffId = i.StaffId)
    )
    BEGIN
        RAISERROR ('Failure: Staff member is already assigned to another specialty.', 16, 1);
        ROLLBACK TRANSACTION; 
        RETURN;
    END
END;
GO

CREATE OR ALTER TRIGGER TR_ValidateDriverUniqueness
ON Drivers
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT 1 
        FROM inserted i
        WHERE EXISTS (SELECT 1 
                      FROM Bosses 
                      WHERE StaffId = i.StaffId) OR 
              EXISTS (SELECT 1 
                      FROM Engineers 
                      WHERE StaffId = i.StaffId)
    )
    BEGIN
        RAISERROR ('Failure: Staff member is already assigned as a Boss or Engineer.', 16, 1);
        ROLLBACK TRANSACTION; 
        RETURN;
    END
END;
GO

CREATE OR ALTER TRIGGER TR_ValidateEngineerUniqueness
ON Engineers
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT 1 
        FROM inserted i
        WHERE EXISTS (SELECT 1 
                      FROM Bosses 
                      WHERE StaffId = i.StaffId) OR 
              EXISTS (SELECT 1 
                      FROM Drivers 
                      WHERE StaffId = i.StaffId)
    )
    BEGIN
        RAISERROR ('Failure: Staff member is already assigned as a Boss or Driver.', 16, 1);
        ROLLBACK TRANSACTION; 
        RETURN;
    END
END;
GO

CREATE OR ALTER TRIGGER TR_ValidateDrivers
ON Drivers
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT 1 
        FROM inserted 
        WHERE [PerformancePoints] < 0.000 OR
              [PerformancePoints] > 100.000 OR 
              [Handicap] < 0.00 OR 
              [Handicap] > 100.00
    )
    BEGIN
        RAISERROR ('Failure: Performance Points and Handicap must be between 0 and 100.', 16, 1);
        ROLLBACK TRANSACTION; 
        RETURN;
    END
END;
GO

CREATE OR ALTER TRIGGER TR_ValidateEngineerSpecialty
ON Engineers
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT 1 
        FROM inserted i
        JOIN AerodynamicEngineers ae ON i.EngineerId = ae.EngineerId
        JOIN PowerEngineers pe ON i.EngineerId = pe.EngineerId
    )
    BEGIN
        RAISERROR ('Failure: An Engineer cannot be both Aerodynamic and Power specialist simultaneously.', 16, 1);
        ROLLBACK TRANSACTION; 
        RETURN;
    END
END;
GO

CREATE OR ALTER TRIGGER TR_LimitTeamAssets
ON TeamsCars
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 
               FROM inserted i
               WHERE (SELECT COUNT(*) 
                      FROM TeamsCars 
                      WHERE TeamId = i.TeamId AND 
                            [Status] = 'Ativo') > 2)
    BEGIN
        RAISERROR ('Failure: Each team can have a maximum of 2 active cars.', 16, 1);
        ROLLBACK TRANSACTION; 
        RETURN;
    END
END;
GO

CREATE OR ALTER TRIGGER TR_LimitTeamBosses
ON TeamBosses
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS ( SELECT 1 
                FROM inserted i
                WHERE (SELECT COUNT(*) 
                       FROM TeamBosses 
                       WHERE TeamId = i.TeamId AND 
                             [Status] = 'Ativo') > 2)
    BEGIN
        RAISERROR ('Failure: Each team can have a maximum of 2 active bosses.', 16, 1);
        ROLLBACK TRANSACTION; 
        RETURN;
    END
END;
GO

CREATE OR ALTER TRIGGER TR_LimitTeamDrivers
ON TeamsDrivers
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 
               FROM inserted i
               WHERE (SELECT COUNT(*) 
                      FROM TeamsDrivers 
                      WHERE TeamId = i.TeamId AND
                            [Status] = 'Ativo') > 2)
    BEGIN
        RAISERROR ('Failure: Each team can have a maximum of 2 active drivers.', 16, 1);
        ROLLBACK TRANSACTION; 
        RETURN;
    END
END;
GO

CREATE OR ALTER TRIGGER TR_LimitTeamAerodynamic
ON TeamsAerodynamic
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 
               FROM inserted i
               WHERE (SELECT COUNT(*) 
                      FROM TeamsAerodynamic 
                      WHERE TeamId = i.TeamId AND 
                            [Status] = 'ATIVO') > 2)
    BEGIN
        RAISERROR ('Failure: Each team can have a maximum of 2 active aerodynamic engineers.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END
END;
GO

CREATE OR ALTER TRIGGER TR_LimitTeamPower
ON TeamsPower
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 
               FROM inserted i
               WHERE (SELECT COUNT(*) 
                      FROM TeamsPower 
                      WHERE TeamId = i.TeamId AND 
                            [Status] = 'Ativo') > 2)
    BEGIN
        RAISERROR ('Failure: Each team can have a maximum of 2 active power engineers.', 16, 1);
        ROLLBACK TRANSACTION; 
        RETURN;
    END
END;
GO

CREATE OR ALTER TRIGGER TR_ValidateCarDriverTeam
ON CarsDrivers
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 
               FROM inserted i
               JOIN TeamsDrivers td ON i.DriverId = td.DriverId
               JOIN TeamsCars tc ON i.CarId = tc.CarId
               WHERE td.TeamId <> tc.TeamId AND 
                     td.[Status] = 'Ativo' AND 
                     tc.[Status] = 'Ativo')
    BEGIN
        RAISERROR ('Failure: Driver and Car must belong to the same team.', 16, 1);
        ROLLBACK TRANSACTION; 
        RETURN;
    END
END;
GO