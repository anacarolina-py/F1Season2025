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
        IF EXISTS (
            SELECT 1 
            FROM inserted t
            WHERE (
                (SELECT COUNT(*) 
                 FROM TeamsCars tc 
                 JOIN Cars c ON tc.CarId = c.CarId 
                 WHERE tc.TeamId = t.TeamId AND 
                       tc.[Status] = 'Ativo' AND 
                       c.[Status] = 'Ativo') != 2
                OR
                (SELECT COUNT(*) 
                 FROM TeamBosses tb 
                 JOIN Staffs s ON (SELECT StaffId 
                                   FROM Bosses 
                                   WHERE BossId = tb.BossId) = s.StaffId
                 WHERE tb.TeamId = t.TeamId AND 
                                   tb.[Status] = 'Ativo' AND 
                                   s.[Status] = 'Ativo') != 2
                OR
                (SELECT COUNT(*) 
                 FROM TeamsDrivers td 
                 JOIN Staffs s ON (SELECT StaffId 
                                   FROM Drivers 
                                   WHERE DriverId = td.DriverId) = s.StaffId
                 WHERE td.TeamId = t.TeamId AND 
                                   td.[Status] = 'Ativo' AND 
                                   s.[Status] = 'Ativo') != 2
                OR
                (SELECT COUNT(*) 
                 FROM TeamsAerodynamic ta 
                 JOIN Staffs s ON (SELECT StaffId 
                                   FROM Engineers 
                                   WHERE EngineerId = (SELECT EngineerId 
                                                       FROM AerodynamicEngineers 
                                                       WHERE AerodynamicEngineerId = ta.AerodynamicEngineerId)) = s.StaffId
                 WHERE ta.TeamId = t.TeamId AND
                                   ta.[Status] = 'Ativo' AND 
                                   s.[Status] = 'Ativo') != 2
                OR
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


