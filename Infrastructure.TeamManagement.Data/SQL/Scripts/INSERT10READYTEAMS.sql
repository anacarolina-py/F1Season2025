INSERT INTO Teams([Name]) 
VALUES ('McLaren Formula 1 Team'), -- 1
	   ('Scuderia Ferrari HP'), -- 2
	   ('Oracle Red Bull Racing'), -- 3
	   ('Mercedes-AMG Petronas F1 Team'), -- 4
	   ('Aston Martin Aramco F1 Team'), -- 5 
	   ('Atlassian Williams Racing'), -- 6
	   ('BWT Alpine F1 Team'), -- 7
	   ('Stake F1 Team Kick Sauber'), -- 8
	   ('Visa Cash App Racing Bulls'), -- 9
	   ('MoneyGram Haas F1 Team'); -- 10
GO

INSERT INTO Staffs([FirstName],[LastName],[Age],[Experience]) 
VALUES ('Leandro', 'Norris',26,1.023),
       ('Oscar','Piastri',24,1.157),

	   ('Charles', 'Leclerc',28,1.489),
       ('Lewis','Hamilton',40,1.732),

	   ('Max', 'Verstappen',28,2.064),
       ('Liam','Lawson',23,2.318),

	   ('George', 'Russell',27,2.759),
       ('Andrea','Antonelli',19,3.041),

	   ('Fernando', 'Alonso',44,3.286),
       ('Lance','Stroll',27,3.694),

	   ('Alexander', 'Albon',29,4.018),
       ('Carlos','Sainz',31,4.275),

	   ('Pierre', 'Gasly',29,4.603),
       ('Jack','Doohan',22,4.829),

	   ('Gabriel', 'Bortoleto',21,2.467),
       ('Nico','Hulkenberg',38,2.814),

	   ('Yuki', 'Tsunoda',25,3.092),
       ('Isack','Hadjar',21,3.358),

	   ('Esteban', 'Ocon',29,4.219),
       ('Oliver','Bearman',20,4.552);
GO

INSERT INTO Drivers([DriverId],[StaffId],[PerformancePoints],[Handicap])
VALUES (1,1,0,78),
       (99,2,0,56),

	   (2,3,0,67),
	   (98,4,0,87),

	   (3,5,0,98),
	   (97,6,0,66),

	   (4,7,0,65),
	   (96,8,0,89),

	   (5,9,0,99),
	   (95,10,0,67),

	   (6,11,0,67),
	   (94,12,0,87),

	   (7,13,0,96),
	   (93,14,0,95),

	   (8,15,0,94),
	   (92,16,0,90),

	   (9,17,0,79),
	   (91,18,0,78),

	   (10,19,0,91),
	   (90,20,0,92);
GO

INSERT INTO TeamsDrivers([TeamId],[DriverId])
VALUES (1,1),
	   (1,99),

	   (2,2),
	   (2,98),

	   (3,3),
	   (3,97),

	   (4,4),
	   (4,96),

	   (5,5),
	   (5,95),

	   (6,6),
	   (6,94),

	   (7,7),
	   (7,93),

	   (8,8),
	   (8,92),

	   (9,9),
	   (9,91),

	   (10,10),
	   (10,90);
GO

INSERT INTO Cars([Model],[Weight],[AerodynamicCoefficient],[PowerCoefficient])
VALUES ('MCL39',800,2.374,7.918),
       ('MCL39',850,0.546,9.203),

	   ('SFE25',800,4.881,1.067),
       ('SFE25',850,6.492,3.758),

	   ('RBL21',800,8.104,0.935),
       ('RBL21',850,5.667,6.221),

	   ('FIW16',800,1.809,4.390),
       ('FIW16',850,9.552,2.146),

	   ('AMR25',800,3.028,8.764),
       ('AMR25',850,7.315,5.009),

	   ('FWR47',800,0.194,6.873),
       ('FWR47',850,4.607,9.118),

	   ('ASF25',800,2.955,1.482),
       ('ASF25',850,8.631,7.240),

	   ('CFI45',800,5.083,3.699),
       ('CFI45',850,9.901,0.558),

	   ('VCB02',800,6.274,4.016),
       ('VCB02',850,1.392,8.507),

	   ('VFI25',800,7.846,2.933),
       ('VFI25',850,3.560,5.128);
GO

INSERT INTO TeamsCars([TeamId],[CarId])
VALUES (1,1),
	   (1,2),

	   (2,3),
	   (2,4),

	   (3,5),
	   (3,6),

	   (4,7),
	   (4,8),

	   (5,9),
	   (5,10),

	   (6,11),
	   (6,12),

	   (7,13),
	   (7,14),

	   (8,15),
	   (8,16),

	   (9,17),
	   (9,18),

	   (10,19),
	   (10,20);
GO

INSERT INTO CarsDrivers([CarId],[DriverId])
VALUES (1,1),
       (2,99),

	   (3,2),
	   (4,98),

	   (5,3),
	   (6,97),

	   (7,4),
	   (8,96),

	   (9,5),
	   (10,95),

	   (11,6),
	   (12,94),

	   (13,7),
	   (14,93),

	   (15,8),
	   (16,92),

	   (17,9),
	   (18,91),

	   (19,10),
	   (20,90);
GO

INSERT INTO Staffs([FirstName],[LastName],[Age],[Experience]) 
VALUES ('Andrea', 'Stella',54,1.037),
       ('Rob','Marshall',57,4.901),

	   ('Frédéric', 'Vasseur',57,1.284),
       ('Lewis','Hamilton',53,4.587),

	   ('Christian', 'Horner',52,1.519),
       ('Pierre','Waché',50,4.329),

	   ('Toto', 'Wolff',53,1.863),
       ('James','Allison',57,4.074),

	   ('Mike', 'Krack',53,2.104),
       ('Dan','Fallows',52,3.812),

	   ('James', 'Vowles',46,2.347),
       ('Pat','Fry',61,3.549),

	   ('Oliver', 'Oakes',37,2.698),
       ('David','Sanchez',45,3.286),

	   ('Jonathan', 'Wheatley',58,2.467),
       ('Stefan','Strahnz',46,3.691),

	   ('Laurent', 'Mekies',48,3.691),
       ('Jody','Egginton',51,3.958),

	   ('Ayao', 'Komatsu',49,1.607),
       ('Andrea','Zordo',43,2.756);
GO

INSERT INTO Bosses([StaffId]) 
VALUES (21),
       (22),
	   (23),
	   (24),
	   (25),
	   (26),
	   (27),
	   (28),
	   (29),
	   (30),
	   (31),
	   (32),
	   (33),
	   (34),
	   (35),
	   (36),
	   (37),
	   (38),
	   (39),
	   (40);
GO

INSERT INTO TeamsBosses([TeamId],[BossId])
VALUES (1,1),
	   (1,2),

	   (2,3),
	   (2,4),

	   (3,5),
	   (3,6),

	   (4,7),
	   (4,8),

	   (5,9),
	   (5,10),

	   (6,11),
	   (6,12),

	   (7,13),
	   (7,14),

	   (8,15),
	   (8,16),

	   (9,17),
	   (9,18),

	   (10,19),
	   (10,20);
GO

INSERT INTO Staffs([FirstName],[LastName],[Age],[Experience]) 
VALUES ('Will', 'Joseph',40,3.537),
       ('Tom','Stallard',46,2.971),

	   ('Bryan', 'Bozzi',35,3.384),
       ('Riccardo','Adami',51,2.547),

	   ('Gianpiero', 'Lambiase',44,4.519),
       ('Richard','Wood',38,2.389),

	   ('Marcus', 'Dudley',40,2.463),
       ('Peter','Bonnington',49,3.044),

	   ('Andrew', 'Vizard',40,4.092),
       ('Gary','Gannon',45,1.218),

	   ('James', 'Urwin',42,3.437),
       ('Gaetan','Jago',39,4.543),

	   ('John', 'Howard',40,2.968),
       ('Josh','Peckett',35,2.368),

	   ('Steven', 'Petrik',38,4.627),
       ('Jose','Lopez',40,1.693),

	   ('Ernesto', 'Desiderio',36,1.693),
       ('Pierre','Hamelin',41,1.598),

	   ('Laura', 'Mueller',34,1.076),
       ('Mark','Slade',57,2.675);
GO

INSERT INTO Engineers([StaffId])
VALUES (41),
       (42),
	   (43),
	   (44),
	   (45),
	   (46),
	   (47),
	   (48),
	   (49),
	   (50),
	   (51),
	   (52),
	   (53),
	   (54),
	   (55),
	   (56),
	   (57),
	   (58),
	   (59),
	   (60);
GO

INSERT INTO AerodynamicEngineers([EngineerId])
VALUES (1),
       (2),
	   (3),
	   (4),
	   (5),
	   (6),
	   (7),
	   (8),
	   (9),
	   (10),
	   (11),
	   (12),
	   (13),
	   (14),
	   (15),
	   (16),
	   (17),
	   (18),
	   (19),
	   (20);
GO

INSERT INTO TeamsAerodynamic([TeamId],[AerodynamicEngineerId])
VALUES (1,1),
       (1,2),

	   (2,3),
	   (2,4),

	   (3,5),
	   (3,6),

	   (4,7),
	   (4,8),

	   (5,9),
	   (5,10),

	   (6,11),
	   (6,12),

	   (7,13),
	   (7,14),

	   (8,15),
	   (8,16),

	   (9,17),
	   (9,18),

	   (10,19),
	   (10,20);
GO

INSERT INTO CarsAerodynamic([CarId],[AerodynamicEngineerId])
VALUES (1,1),
       (2,2),

	   (3,3),
	   (4,4),

	   (5,5),
	   (6,6),

	   (7,7),
	   (8,8),

	   (9,9),
	   (10,10),

	   (11,11),
	   (12,12),

	   (13,13),
	   (14,14),

	   (15,15),
	   (16,16),

	   (17,17),
	   (18,18),

	   (19,19),
	   (20,20);
GO

INSERT INTO Staffs([FirstName],[LastName],[Age],[Experience])
VALUES ('Andrew', 'Jarvis',38,1.026),
       ('Barnaby','Barnaby',35,1.587),

	   ('Jock', 'Clear',61,2.943),
       ('Ben','Marchant',36,4.118),

	   ('Tom', 'Hart',37,3.204),
       ('Stephen','Knowles',40,1.879),

	   ('Riccardo', 'Musconi',44,4.652),
       ('Joseph','Leberrer',33,2.371),

	   ('Chris', 'Cronin',41,3.756),
       ('Ben','Michell',39,4.981),

	   ('Edoardo', 'Brosco',40,1.442),
       ('Alexis','Potter',37,2.806),

	   ('Karel', 'Loos',42,3.119),
       ('Barney','Hassell',36,4.337),

	   ('Jorn', 'Becker',40,1.694),
       ('Lucia','Conconi',38,3.582),

	   ('Ben', 'Shovlin',35,2.058),
       ('Mattia','Spini',40,4.764),

	   ('Francesco', 'Nenci',52,1.913),
       ('Dominic','Haines',39,3.298);
GO

INSERT INTO Engineers([StaffId])
VALUES (61),
       (62),
	   (63),
	   (64),
	   (65),
	   (66),
	   (67),
	   (68),
	   (69),
	   (70),
	   (71),
	   (72),
	   (73),
	   (74),
	   (75),
	   (76),
	   (77),
	   (78),
	   (79),
	   (80);
GO

INSERT INTO PowerEngineers([EngineerId])
VALUES (21),
       (22),
       (23),
	   (24),
	   (25),
	   (26),
	   (27),
	   (28),
	   (29),
	   (30),
	   (31),
	   (32),
	   (33),
	   (34),
	   (35),
	   (36),
	   (37),
	   (38),
	   (39),
	   (40);
GO

INSERT INTO TeamsPower([TeamId],[PowerEngineerId])
VALUES (1,1),
       (1,2),
	   (2,3),
	   (2,4),
	   (3,5),
	   (3,6),
	   (4,7),
	   (4,8),
	   (5,9),
	   (5,10),
	   (6,11),
	   (6,12),
	   (7,13),
	   (7,14),
	   (8,15),
	   (8,16),
	   (9,17),
	   (9,18),
	   (10,19),
	   (10,20);
GO

INSERT INTO CarsPower([CarId],[PowerEngineerId])
VALUES (1,1),
       (2,2),
	   (3,3),
	   (4,4),
	   (5,5),
	   (6,6),
	   (7,7),
	   (8,8),
	   (9,9),
	   (10,10),
	   (11,11),
	   (12,12),
	   (13,13),
	   (14,14),
	   (15,15),
	   (16,16),
	   (17,17),
	   (18,18),
	   (19,19),
	   (20,20);
GO

UPDATE Teams 
SET Status = 'Ativo'
GO

--SELECT * FROM PowerEngineers
--SELECT * FROM TeamsAerodynamic
--SELECT * FROM Cars
--select * from Staffs
--SELECT * FROM AerodynamicEngineers;
--SELECT * FROM Engineers
--SELECT * FROM Teams
--SELECT * FROM Drivers
--SELECT * FROM TeamsAerodynamic
--SELECT * FROM TeamsBosses
--SELECT * FROM TeamsCars	
--SELECT * FROM TeamsDrivers	
--SELECT * FROM TeamsPower	
--SELECT * FROM CarsAerodynamic	
--SELECT * FROM CarsPower
--SELECT * FROM CarsDrivers	