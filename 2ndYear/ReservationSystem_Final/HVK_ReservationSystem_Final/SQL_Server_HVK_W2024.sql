/*******************************************************************************
   HVK Database Winter 2024 - Updated Version

CD January 13, '22 - starting point was 2021_v1_2
Same as initial W2021 but modified to reflect fall 2021 case study changes
removed kennel log, vet, pet neutured; 
moved special notes to pet, reduced runs to 24  ---> just runs and mapping left to do
and remapped type and reservations;
changed owner to customer terminology.
Aged reservations, vaccination, and pets:
updated reservation and vaccination dates by 3 years; 
updated pet birthdates by 5 years;
Removed use of confirmed status for reservations (now 2-5)
and ensured that run status (1-3).

CD - March 16,'22 - V2 Updates requested for Web V class
- added password to the customer table, and set the data entries to null
- added the VaccinationChecked field to the PetVaccination table, made it 
BIT, no nulls, and defaulted it to false (0).  Set it to true / 1 for 
any pets who have been in during the last three months.
- Changed all boolean-like fields to BIT, no nulls:  Pet - Climber, Barker and Run - Covered.
- changed all data inserts for climber, barker, and covered to new content.
- Removed constraints on Covered, since BIT now takes care of that.  
- Changed the date aging at the end to keep converting / aging based on today's date, and 
determined a delta to apply based on original script date.
- changed pet, birthdate to birthyear and made it int and updated data inserts.  Removed pet aging
at the end and just updated the inserts.

CD - March 13, 2023 - Added changes to ease Web V in W2023 - customer -> Member table
Added null helper columns to ensure all link tables will be explicitly generated:
added to PetReservationDiscount, PetReservationService, and ReservationDiscount.
Aged by an extra year from last year.  Population of some data related to reservations
removed to ease testing setup for J20 Assignment 3.  They are simply commented out
but left in.  Renamed the base database to HVKV2_xy.

KB - Feb 19, 2024 - Modified this a3 script for a2 
- Removed House Trained and replaced with Run Sharing in Insert for Discount table
- Added Sterilized to Pet table
- Renamed base DB from HVKV2_xy to HVKW24_Team7
- Made Emergency Contact Name and Phone Mandatory fields
- Removed Member table and replaced with HVKUser, Employee, Customer
- Updated references to use HVKUser or Customer instead of Member
- Uncommented out data inserts
- Added Jim and Sally rows to Employee table
- Bump delta and age 5 years
Feb 21, 2024 
- Removed Employee and Customer and only use HVKUser
- Add [UserType] to HVKUser and add CHECK for Employee or Customer type
- Change Emergency Contact Name and Phone back to non-mandatory
- Changed data inserts to match new structure
 
********************************************************************************/
/*******************************************************************************
420-J20-HR Student Naming conventions

Ensure that before you create your Database, you replace the _xy in the DB name
with your initials.

For instance, my initials are kb, so the DB name throughout should be changed
from HVKW24_Team7 to HVKW24_kb.

Recommend that you save a copy of this file with these changes in your H drive.

********************************************************************************/

USE master;

/*******************************************************************************
   Drop database if it exists
********************************************************************************/
IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'HVKW24_Team7')
BEGIN
	ALTER DATABASE [HVKW24_Team7] SET OFFLINE WITH ROLLBACK IMMEDIATE;
	ALTER DATABASE [HVKW24_Team7] SET ONLINE;
	DROP DATABASE [HVKW24_Team7];
END

GO

/*******************************************************************************
   Create database
********************************************************************************/
CREATE DATABASE [HVKW24_Team7];
GO

USE [HVKW24_Team7];
GO

/*******************************************************************************
   Create Tables
********************************************************************************/

/* List of Tables ported from Oracle
HVK_DAILY_RATE - DailyRate
HVK_DISCOUNT - Discount
HVK_MEDICATION - Medication
HVK_CUSTOMER - HVKUser
HVK_PET - Pet
HVK_PET_RESERVATION - PetReservation
HVK_PET_RESERVATION_DISCOUNT - PetReservationDiscount
HVK_PET_RESERVATION_SERVICE - PetReservationService
HVK_PET_VACCINATION - PetVaccination
HVK_RESERVATION - Reservation
HVK_RESERVATION_DISCOUNT - ReservationDiscount
HVK_RUN - Run
HVK_SERVICE - Service
HVK_VACCINATION - Vaccination */


/* DDL for Table DailyRate */

CREATE TABLE 
[dbo].[DailyRate]
(
   [DailyRateId] INT IDENTITY (1, 1) PRIMARY KEY,
   [Rate] NUMERIC (5, 2)  NOT NULL,
   [DogSize] CHAR(1)  NULL,
   [ServiceId] INT NOT NULL
);

GO

/* DDL for Table Discount */

CREATE TABLE 
[dbo].[Discount]
(
   [DiscountId] INT IDENTITY (1, 1) PRIMARY KEY,
   [Desciption] VARCHAR(50)  NOT NULL,
   [Percentage] NUMERIC(3, 2)  NOT NULL,
   [Type] CHAR(1)  NOT NULL
);

GO


/* DDL for Table Medication */

CREATE TABLE 
[dbo].[Medication]
(
   [MedicationId] INT IDENTITY (1, 1) PRIMARY KEY,
   [Name] VARCHAR(50), 
   [Dosage] VARCHAR(50), 
   [SpecialInstruct] VARCHAR(50), 
   [EndDate] DATE, 
   [PetReservationId] INT NOT NULL
);
  
GO 



/* DDL for Table HVKUser */

CREATE TABLE 
[dbo].[HVKUser]
(
   [HVKUserId] INT IDENTITY (1, 1) PRIMARY KEY,
   [FirstName] VARCHAR(25)  NOT NULL,
   [LastName] VARCHAR(25)  NOT NULL,
   [Email] VARCHAR(50)  NULL,
   [Password] VARCHAR(50)  NULL,
   [Street] VARCHAR(40)  NULL,
   [City] VARCHAR(25)  NULL,
   [Province] CHAR(2)  NULL,
   [PostalCode] CHAR(6)  NULL,
   [Phone] CHAR(10)  NULL,
   [CellPhone] CHAR(10)  NULL,
   [EmergencyContactFirstName] VARCHAR(25)  NULL,
   [EmergencyContactLastName] VARCHAR(25)  NULL,
   [EmergencyContactPhone] CHAR(10)  NULL,
   [UserType] VARCHAR(10) NOT NULL DEFAULT 'Customer',
   CHECK (UserType in ('Customer','Employee'))
);

GO


/* DDL for Table Pet */


CREATE TABLE 
[dbo].[Pet]
(
   [PetId] INT IDENTITY (1, 1) PRIMARY KEY,
   [Name] VARCHAR(25)  NOT NULL,
   [Gender] CHAR(1)  NOT NULL,
   [Breed] VARCHAR(50)  NULL,
   [Birthyear] INT  NULL,
   [HVKUserId] INT  NOT NULL,
   [DogSize] CHAR(1)  NULL,
   [Climber] BIT  NOT NULL DEFAULT 0,
   [Barker] BIT  NOT NULL DEFAULT 0,
   [SpecialNotes] VARCHAR(200)  NULL,
   [Sterilized] BIT  NOT NULL DEFAULT 0
);

GO

/* DDL for Table PetReservation */

CREATE TABLE 
[dbo].[PetReservation]
(
   [PetReservationId] INT IDENTITY (1, 1) PRIMARY KEY,
   [PetId] INT NOT NULL,
   [ReservationId] INT NOT NULL,
   [RunId] INT NULL
);

GO

/* DDL for Table PetReservationDiscount */


CREATE TABLE 
[dbo].[PetReservationDiscount]
(
   [DiscountId] INT NOT NULL,
   [PetReservationId] INT NOT NULL,
   [NullHelper] CHAR(1) NULL
);

GO

/* DDL for Table PetReservationService */

CREATE TABLE 
[dbo].[PetReservationService]
(
   [PetReservationId] INT NOT NULL,
   [ServiceId] INT NOT NULL,
   [NullHelper] CHAR(1) NULL
);

GO

/* DDL for Table PetVaccination */

CREATE TABLE 
[dbo].[PetVaccination]
(
   [ExpiryDate] DATE  NOT NULL,
   [VaccinationId] INT NOT NULL,
   [PetId] INT NOT NULL,
   [VaccinationChecked] BIT NOT NULL DEFAULT 0
);

GO


/* DDL for Table Reservation */


CREATE TABLE 
[dbo].[Reservation]
(
   [ReservationId] INT IDENTITY (1, 1) PRIMARY KEY,
   [StartDate] DATE  NOT NULL,
   [EndDate] DATE  NOT NULL,
   [Status] NUMERIC(1, 0)  NOT NULL
);

GO

/* DDL for Table ReservationDiscount */

CREATE TABLE 
[dbo].[ReservationDiscount]
(
   [DiscountId] INT NOT NULL,
   [ReservationId] INT NOT NULL,
   [NullHelper] CHAR(1) NULL
);

GO

/* DDL for Table Run */

CREATE TABLE 
[dbo].[Run]
(
   [RunId] INT IDENTITY (1, 1) PRIMARY KEY,
   [Size] CHAR(1)  NOT NULL,
   [Covered] BIT NOT  NULL DEFAULT 0,
   [Location] CHAR(1)  NULL,
   [Status] NUMERIC(1, 0)  NULL
);

GO


/* DDL for Table Service */

CREATE TABLE 
[dbo].[Service]
(
   [ServiceId] INT IDENTITY (1, 1) PRIMARY KEY,
   [ServiceDescription] VARCHAR(50)  NOT NULL
);

GO


/* DDL for Table Vaccination */


CREATE TABLE 
[dbo].[Vaccination]
(
   [VaccinationId] INT IDENTITY (1, 1) PRIMARY KEY,
   [Name] VARCHAR(50)  NOT NULL
);

GO


/*--------------------------------------------------------
  MIGRATING DATA FROM ORACLE COPY
--------------------------------------------------------*/
/* To ensure the data from Oracle is migrated with same primary key 
values for the tables, autogeneration of the the IDs for each table 
is turned off and then back on for each table.  */

SET IDENTITY_INSERT [dbo].[DailyRate] ON 
Insert into DailyRate (DailyRateId,Rate,DogSize,ServiceId) values (1,20,'S',1);
Insert into DailyRate (DailyRateId,Rate,DogSize,ServiceId) values (2,22,'M',1);
Insert into DailyRate (DailyRateId,Rate,DogSize,ServiceId) values (3,25,'L',1);
Insert into DailyRate (DailyRateId,Rate,DogSize,ServiceId) values (4,2,'S',2);
Insert into DailyRate (DailyRateId,Rate,DogSize,ServiceId) values (5,3,'M',2);
Insert into DailyRate (DailyRateId,Rate,DogSize,ServiceId) values (6,4,'L',2);
Insert into DailyRate (DailyRateId,Rate,DogSize,ServiceId) values (7,2,null,4);
Insert into DailyRate (DailyRateId,Rate,DogSize,ServiceId) values (11,1,null,5);
SET IDENTITY_INSERT [dbo].[DailyRate] OFF


SET IDENTITY_INSERT [dbo].[Discount] ON
Insert into Discount (DiscountId,Desciption,Percentage,Type) values (1,'Run Sharing',0.1,'D');
Insert into Discount (DiscountId,Desciption,Percentage,Type) values (2,'Multiple Pets',0.07,'T');
Insert into Discount (DiscountId,Desciption,Percentage,Type) values (3,'Own Food',0.1,'D');
SET IDENTITY_INSERT [dbo].[Discount] OFF



SET IDENTITY_INSERT [dbo].[HVKUser] ON
Insert into HVKUser (HVKUserId,FirstName,LastName,Email,Password, Street,City,Province,PostalCode,Phone,CellPhone,EmergencyContactFirstName,EmergencyContactLastName,EmergencyContactPhone)
values (18,'Barb B.','Que','bque@gmail.com','123','18 Sequel Row','Gatineau','QC','J8A1V3','8195554215','8195559999','Fox','Mulder','7575551234');
Insert into HVKUser (HVKUserId,FirstName,LastName,Email,Password, Street,City,Province,PostalCode,Phone,CellPhone,EmergencyContactFirstName,EmergencyContactLastName,EmergencyContactPhone)
values (19,'Chester','Drawers','cdrawers@gmail.com','123',null,null,null,null,'8191234876','8195559998','Fox','Mulder','7575551234');

Insert into HVKUser (HVKUserId,FirstName,LastName,Email,Password, Street,City,Province,PostalCode,Phone,CellPhone,EmergencyContactFirstName,EmergencyContactLastName,EmergencyContactPhone)
values (20,'Anita','Alibi','aalibi@gmail.com','123',null,null,null,null,'8191224876','8195559997','Alex','Krycek','7575551239');

Insert into HVKUser (HVKUserId,FirstName,LastName,Email,Password, Street,City,Province,PostalCode,Phone,CellPhone,EmergencyContactFirstName,EmergencyContactLastName,EmergencyContactPhone)
values (8,'Amanda','Linn','alinn@gmail.com','123','23 Java Road','Gatineau','QC','J8Y6T5','8195552233','8195559996','Fox','Mulder','7575551234');

Insert into HVKUser (HVKUserId,FirstName,LastName,Email,Password, Street,City,Province,PostalCode,Phone,CellPhone,EmergencyContactFirstName,EmergencyContactLastName,EmergencyContactPhone)
values (6,'Jeff','Summers','jsummers@gmail.com','123','62 Adams','Maniwaki','QC','J8Y8T3','8195551843','8195559995','Fox','Mulder','7575551234');

Insert into HVKUser (HVKUserId,FirstName,LastName,Email,Password, Street,City,Province,PostalCode,Phone,CellPhone,EmergencyContactFirstName,EmergencyContactLastName,EmergencyContactPhone)
values (7,'Peter','Piper','ppiper@gmail.com','123','107 Main','Gatineau','QC','J8Y6T3','8195554543','8195559994','Alex','Krycek','7575551239');

Insert into HVKUser (HVKUserId,FirstName,LastName,Email,Password, Street,City,Province,PostalCode,Phone,CellPhone,EmergencyContactFirstName,EmergencyContactLastName,EmergencyContactPhone)
values (1,'Jane','Smith','jsmith@gmail.com','123','202 Poodle Path','Gatineau','QC','J8A1R2','8195551111','8195559993','Dana','Scully','7575551235');

Insert into HVKUser (HVKUserId,FirstName,LastName,Email,Password, Street,City,Province,PostalCode,Phone,CellPhone,EmergencyContactFirstName,EmergencyContactLastName,EmergencyContactPhone)
values (2,'Mike','O''Phone','mophone@gmail.com','123','74 Benton','Gatineau','QC','J8Y6T3','8195552222','8195559992','Dana','Scully','7575551235');

Insert into HVKUser (HVKUserId,FirstName,LastName,Email,Password, Street,City,Province,PostalCode,Phone,CellPhone,EmergencyContactFirstName,EmergencyContactLastName,EmergencyContactPhone)
values (3,'Dwight','Wong','dwong@gmail.com','123','384 Garten','Ottawa','ON','K8Y6T3','8195555222','8195559991','Dana','Scully','7575551235');

Insert into HVKUser (HVKUserId,FirstName,LastName,Email,Password, Street,City,Province,PostalCode,Phone,CellPhone,EmergencyContactFirstName,EmergencyContactLastName,EmergencyContactPhone)
values (4,'Mahatma','Coate','mcoate@gmail.com','123','1 Golfview','Ottawa','ON','K8Y6T3','8195559843','8195559990','Dana','Scully','7575551235');

Insert into HVKUser (HVKUserId,FirstName,LastName,Email,Password, Street,City,Province,PostalCode,Phone,CellPhone,EmergencyContactFirstName,EmergencyContactLastName,EmergencyContactPhone)
values (5,'Sue','Metu','smetu@gmail.com','123','3 Riverview','Ottawa','ON','K8Y6T3','8195556699','8195559989','Alex','Krycek','7575551239');

Insert into HVKUser (HVKUserId,FirstName,LastName,Email,Password, Street,City,Province,PostalCode,Phone,CellPhone,EmergencyContactFirstName,EmergencyContactLastName,EmergencyContactPhone)
values (10,'April','Showers','ashowers@gmail.com','123',null,null,null,null,'8195558765','8195559988','Walter','Skinner','7575551236');

Insert into HVKUser (HVKUserId,FirstName,LastName,Email,Password, Street,City,Province,PostalCode,Phone,CellPhone,EmergencyContactFirstName,EmergencyContactLastName,EmergencyContactPhone)
values (11,'Salton','Pepper','spepper@gmail.com','123',null,null,null,null,'8195555571','8195559987','Walter','Skinner','7575551236');

Insert into HVKUser (HVKUserId,FirstName,LastName,Email,Password, Street,City,Province,PostalCode,Phone,CellPhone,EmergencyContactFirstName,EmergencyContactLastName,EmergencyContactPhone)
values (12,'Ella','Mentary','ementary@gmail.com','123',null,null,null,null,'8195551839','8195559986','Walter','Skinner','7575551236');

Insert into HVKUser (HVKUserId,FirstName,LastName,Email,Password, Street,City,Province,PostalCode,Phone,CellPhone,EmergencyContactFirstName,EmergencyContactLastName,EmergencyContactPhone)
values (13,'Sam','Ovar','sovar@gmail.com','123',null,null,null,'W2W2W2','8195551652','8195559985','Walter','Skinner','7575551236');

Insert into HVKUser (HVKUserId,FirstName,LastName,Email,Password, Street,City,Province,PostalCode,Phone,CellPhone,EmergencyContactFirstName,EmergencyContactLastName,EmergencyContactPhone)
values (14,'Carrie','Mehome','cmehome@gmail.com','123',null,null,null,null,'8195551232','8195559984','Walter','Skinner','7575551236');

Insert into HVKUser (HVKUserId,FirstName,LastName,Email,Password, Street,City,Province,PostalCode,Phone,CellPhone,EmergencyContactFirstName,EmergencyContactLastName,EmergencyContactPhone)
values (15,'Bayo','Wolfe','bwolfe@gmail.com','123',null,null,null,null,'8195565832','8195559983','Walter','Skinner','7575551236');

Insert into HVKUser (HVKUserId,FirstName,LastName,Email,Password, Street,City,Province,PostalCode,Phone,CellPhone,EmergencyContactFirstName,EmergencyContactLastName,EmergencyContactPhone)
values (16,'Moe','Bilhome','mbilhome@gmail.com','123',null,null,null,null,'8195575332','8195559982','Monica','Reyes','7575551238');

Insert into HVKUser (HVKUserId,FirstName,LastName,Email,Password, Street,City,Province,PostalCode,Phone,CellPhone,EmergencyContactFirstName,EmergencyContactLastName,EmergencyContactPhone)
values (17,'Polly','Morfek','pmorfek@gmail.com','123',null,null,null,null,'8195575332','8195559981','Alex','Krycek','7575551239');

Insert into HVKUser (HVKUserId,FirstName,LastName,Email,Password, Street,City,Province,PostalCode,Phone,CellPhone,EmergencyContactFirstName,EmergencyContactLastName,EmergencyContactPhone,UserType)
values (21,'Sally','Read','sread@hvk.com','123',null,null,null,null,null,null,null,null,null,'Employee');

Insert into HVKUser (HVKUserId,FirstName,LastName,Email,Password, Street,City,Province,PostalCode,Phone,CellPhone,EmergencyContactFirstName,EmergencyContactLastName,EmergencyContactPhone,UserType)
values (22,'Jim','Read','jread@hvk.com','123',null,null,null,null,null,null,null,null,null,'Employee');


SET IDENTITY_INSERT [dbo].[HVKUser] OFF


SET IDENTITY_INSERT [dbo].[Pet] ON
Insert into Pet (PetId,Name,Gender,Breed,Birthyear,HVKUserId,DogSize,Climber,Barker,SpecialNotes) values (33,'Willie','M','Great Pyrenees',2015,18,'L',0,0,null);
Insert into Pet (PetId,Name,Gender,Breed,Birthyear,HVKUserId,DogSize,Climber,Barker,SpecialNotes) values (34,'Aurora','F','Beagle',2020,19,'M',0,0,null);
Insert into Pet (PetId,Name,Gender,Breed,Birthyear,HVKUserId,DogSize,Climber,Barker,SpecialNotes) values (35,'Bella','F','Chihuahua',2021,20,'S',0,0,null);
Insert into Pet (PetId,Name,Gender,Breed,Birthyear,HVKUserId,DogSize,Climber,Barker,SpecialNotes) values (14,'Grizzlie','F','Shi Tzu',2014,8,'S',0,1,null);
Insert into Pet (PetId,Name,Gender,Breed,Birthyear,HVKUserId,DogSize,Climber,Barker,SpecialNotes) values (9,'Parker','M','Shepherd Mix',null,6,'M',1,0,null);
Insert into Pet (PetId,Name,Gender,Breed,Birthyear,HVKUserId,DogSize,Climber,Barker,SpecialNotes) values (10,'Pete','M','Tibetan Spanial - Sheltie',null,7,'S',0,0,null);
Insert into Pet (PetId,Name,Gender,Breed,Birthyear,HVKUserId,DogSize,Climber,Barker,SpecialNotes) values (11,'Max','M','Samoyed',null,7,'L',0,0,null);
Insert into Pet (PetId,Name,Gender,Breed,Birthyear,HVKUserId,DogSize,Climber,Barker,SpecialNotes) values (12,'Kitoo','F','Samoyed',null,7,'L',0,0,null);
Insert into Pet (PetId,Name,Gender,Breed,Birthyear,HVKUserId,DogSize,Climber,Barker,SpecialNotes) values (1,'Scrabble','F','Llassapoo',null,1,'S',0,0,'Scrabble is terrified of hot air balloons');
Insert into Pet (PetId,Name,Gender,Breed,Birthyear,HVKUserId,DogSize,Climber,Barker,SpecialNotes) values (2,'Archie','F','Standard Poodle',null,1,'M',0,0,'Archie is extremely shy and very timid around other dogs - she does not do well in an open playtime.');
Insert into Pet (PetId,Name,Gender,Breed,Birthyear,HVKUserId,DogSize,Climber,Barker,SpecialNotes) values (3,'Busker','M','Rhodesian Ridgeback',2011,2,'L',1,0,null);
Insert into Pet (PetId,Name,Gender,Breed,Birthyear,HVKUserId,DogSize,Climber,Barker,SpecialNotes) values (6,'Huxley','M','Standard Poodle',2020,2,'M',0,0,'Huxley has an uncontrollable tendency to quote Shakespeare');
Insert into Pet (PetId,Name,Gender,Breed,Birthyear,HVKUserId,DogSize,Climber,Barker,SpecialNotes) values (7,'Charlie','M','Jack Russell Terrier',null,4,'S',0,0,null);
Insert into Pet (PetId,Name,Gender,Breed,Birthyear,HVKUserId,DogSize,Climber,Barker,SpecialNotes) values (16,'Maggie','F','Golden Retriever',2012,10,'L',0,0,null);
Insert into Pet (PetId,Name,Gender,Breed,Birthyear,HVKUserId,DogSize,Climber,Barker,SpecialNotes) values (26,'Skarpa','F','Spaniel mix',null,15,'S',0,0,null);
Insert into Pet (PetId,Name,Gender,Breed,Birthyear,HVKUserId,DogSize,Climber,Barker,SpecialNotes) values (27,'Bothvar','M','Schnauzer',null,15,'S',0,0,null);
Insert into Pet (PetId,Name,Gender,Breed,Birthyear,HVKUserId,DogSize,Climber,Barker,SpecialNotes) values (28,'Foxfire','F','Beagle mix',null,15,'S',0,0,null);
Insert into Pet (PetId,Name,Gender,Breed,Birthyear,HVKUserId,DogSize,Climber,Barker,SpecialNotes) values (29,'Shaboo','F','Rat Terrier',null,16,'L',0,0,null);
Insert into Pet (PetId,Name,Gender,Breed,Birthyear,HVKUserId,DogSize,Climber,Barker,SpecialNotes) values (30,'Suki','F','St Bernard',null,5,'L',0,0,null);
Insert into Pet (PetId,Name,Gender,Breed,Birthyear,HVKUserId,DogSize,Climber,Barker,SpecialNotes) values (31,'Sam','M','Border Collie',null,5,'L',0,0,null);
Insert into Pet (PetId,Name,Gender,Breed,Birthyear,HVKUserId,DogSize,Climber,Barker,SpecialNotes) values (32,'Snoop Dogg','M','Black Lab',null,5,'M',0,0,null);
Insert into Pet (PetId,Name,Gender,Breed,Birthyear,HVKUserId,DogSize,Climber,Barker,SpecialNotes) values (13,'Logan','M','Bernese Mountain Dog',2020,3,'L',0,0,null);
Insert into Pet (PetId,Name,Gender,Breed,Birthyear,HVKUserId,DogSize,Climber,Barker,SpecialNotes) values (20,'Poppy','F','Jack Russell Terrier',2021,12,'S',0,1,null);
SET IDENTITY_INSERT [dbo].[Pet] OFF


SET IDENTITY_INSERT [dbo].[PetReservation] ON
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (200,7,631,null);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (201,20,632,null);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (202,32,633,null);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (204,20,625,null);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (205,33,630,null);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (206,10,635,null);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (207,9,696,null);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (208,26,707,null);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (209,9,708,null);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (210,11,709,null);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (211,13,711,null);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (212,14,712,null);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (213,26,713,null);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (214,29,716,null);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (215,30,717,17);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (216,3,720,17);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (217,3,721,17);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (219,3,595,null);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (223,13,102,16);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (224,6,103,24);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (225,9,104,20);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (226,6,105,17);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (227,3,108,24);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (228,10,109,23);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (229,14,114,1);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (230,3,115,17);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (231,9,120,19);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (232,9,123,23);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (233,20,131,20);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (234,10,136,16);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (235,12,137,null);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (236,13,138,14);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (237,14,139,19);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (239,27,140,1);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (240,29,143,16);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (241,29,144,7);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (242,30,145,17);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (243,31,146,17);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (249,6,122,24);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (250,6,615,null);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (251,16,700,null);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (252,7,620,null);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (253,30,701,null);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (254,33,702,null);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (255,2,703,null);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (256,32,704,null);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (257,34,705,null);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (261,1,703,null);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (264,6,115,17);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (268,12,709,null);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (270,27,713,null);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (271,28,713,null);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (272,31,145,6);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (274,32,717,null);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (298,11,148,17);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (404,10,802,null);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (405,11,802,null);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (406,3,803,null);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (409,6,803,null);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (410,10,804,null);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (411,11,804,null);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (412,12,804,null);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (284,6,636,null);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (286,1,594,24);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (288,26,601,null);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (292,31,603,null);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (294,11,605,null);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (296,26,140,2);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (300,1,112,20);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (302,1,100,20);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (304,3,106,17);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (306,11,110,24);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (400,31,800,null);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (402,26,801,null);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (413,3,805,null);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (417,3,806,null);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (423,13,808,null);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (424,11,809,null);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (426,33,810,null);
Insert into PetReservation (PetReservationId,PetId,ReservationId,RunId) values (427,13,811,null);
SET IDENTITY_INSERT [dbo].[PetReservation] OFF


SET IDENTITY_INSERT [dbo].[Medication] ON
Insert into Medication (MedicationId,Name,Dosage,SpecialInstruct,EndDate,PetReservationId) values (2,'Tapzole','1/2 tablet once daily',null,null,264);
Insert into Medication (MedicationId,Name,Dosage,SpecialInstruct,EndDate,PetReservationId) values (4,'Medicam','36 kg',null,null,240);
Insert into Medication (MedicationId,Name,Dosage,SpecialInstruct,EndDate,PetReservationId) values (1,'Prednisone','1 tablet twice daily',null,null,300);
Insert into Medication (MedicationId,Name,Dosage,SpecialInstruct,EndDate,PetReservationId) values (5,'Tapzole','1/2 tablet once daily',null,null,300);
SET IDENTITY_INSERT [dbo].[Medication] OFF


Insert into PetReservationDiscount (DiscountId,PetReservationId) values (1,284);
Insert into PetReservationDiscount (DiscountId,PetReservationId) values (1,286);
Insert into PetReservationDiscount (DiscountId,PetReservationId) values (1,288);
Insert into PetReservationDiscount (DiscountId,PetReservationId) values (1,292);
Insert into PetReservationDiscount (DiscountId,PetReservationId) values (1,294);
Insert into PetReservationDiscount (DiscountId,PetReservationId) values (1,296);
Insert into PetReservationDiscount (DiscountId,PetReservationId) values (1,300);
Insert into PetReservationDiscount (DiscountId,PetReservationId) values (1,302);
Insert into PetReservationDiscount (DiscountId,PetReservationId) values (1,304);
Insert into PetReservationDiscount (DiscountId,PetReservationId) values (1,306);
Insert into PetReservationDiscount (DiscountId,PetReservationId) values (3,306);


Insert into PetReservationService (PetReservationId,ServiceId) values (200,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (201,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (202,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (204,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (205,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (206,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (207,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (208,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (209,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (210,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (211,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (212,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (213,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (214,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (215,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (216,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (217,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (219,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (223,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (223,2);
Insert into PetReservationService (PetReservationId,ServiceId) values (224,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (224,5);
Insert into PetReservationService (PetReservationId,ServiceId) values (225,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (225,5);
Insert into PetReservationService (PetReservationId,ServiceId) values (226,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (227,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (228,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (228,2);
Insert into PetReservationService (PetReservationId,ServiceId) values (229,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (229,2);
Insert into PetReservationService (PetReservationId,ServiceId) values (230,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (230,5);
Insert into PetReservationService (PetReservationId,ServiceId) values (231,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (231,5);
Insert into PetReservationService (PetReservationId,ServiceId) values (232,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (232,5);
Insert into PetReservationService (PetReservationId,ServiceId) values (233,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (233,2);
Insert into PetReservationService (PetReservationId,ServiceId) values (234,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (234,2);
Insert into PetReservationService (PetReservationId,ServiceId) values (234,5);
Insert into PetReservationService (PetReservationId,ServiceId) values (235,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (236,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (237,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (239,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (240,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (240,4);
Insert into PetReservationService (PetReservationId,ServiceId) values (241,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (242,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (243,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (249,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (249,5);
Insert into PetReservationService (PetReservationId,ServiceId) values (250,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (251,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (252,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (253,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (254,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (255,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (256,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (257,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (261,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (264,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (264,4);
Insert into PetReservationService (PetReservationId,ServiceId) values (264,5);
Insert into PetReservationService (PetReservationId,ServiceId) values (268,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (270,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (271,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (272,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (274,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (284,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (286,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (288,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (292,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (294,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (296,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (296,2);
Insert into PetReservationService (PetReservationId,ServiceId) values (296,5);
Insert into PetReservationService (PetReservationId,ServiceId) values (298,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (300,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (300,2);
Insert into PetReservationService (PetReservationId,ServiceId) values (300,4);
Insert into PetReservationService (PetReservationId,ServiceId) values (302,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (302,2);
Insert into PetReservationService (PetReservationId,ServiceId) values (304,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (304,5);
Insert into PetReservationService (PetReservationId,ServiceId) values (306,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (306,2);
Insert into PetReservationService (PetReservationId,ServiceId) values (400,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (402,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (404,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (405,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (405,2);
Insert into PetReservationService (PetReservationId,ServiceId) values (406,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (409,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (410,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (411,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (412,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (413,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (417,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (423,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (423,2);
Insert into PetReservationService (PetReservationId,ServiceId) values (424,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (426,1);
Insert into PetReservationService (PetReservationId,ServiceId) values (427,1);



Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-04-12',2),1,33);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-04-12',2),2,33);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-04-12',2),3,33);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-04-12',2),4,33);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'20-04-12',2),5,33);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'20-04-12',2),6,33);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'18-12-10',2),1,35);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'18-12-10',2),2,35);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'18-12-10',2),3,35);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'18-12-10',2),4,35);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'18-12-10',2),5,35);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'18-12-10',2),6,35);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-08-05',2),1,13);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-08-05',2),2,13);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-08-05',2),3,13);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-08-05',2),4,13);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-08-05',2),5,13);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-08-05',2),1,9);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-08-05',2),2,9);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-02-05',2),1,1);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-02-05',2),2,1);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-02-05',2),3,1);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-02-05',2),4,1);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-02-05',2),5,1);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'20-02-05',2),6,1);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-02-05',2),1,2);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-02-05',2),2,2);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-02-05',2),3,2);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-02-05',2),4,2);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-02-05',2),5,2);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'20-02-05',2),6,2);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-08-05',2),1,3);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-08-05',2),2,3);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-08-05',2),3,3);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-08-05',2),4,3);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-08-05',2),5,3);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'18-02-07',2),1,6);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'18-09-03',2),2,6);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'18-02-07',2),3,6);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'18-02-07',2),4,6);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'18-02-07',2),5,6);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-07-07',2),6,6);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-02-07',2),1,7);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-02-07',2),2,7);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'18-06-07',2),4,7);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-10-17',2),6,7);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-08-05',2),3,9);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-08-05',2),4,9);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-08-05',2),5,9);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-08-05',2),1,12);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-08-05',2),2,12);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-08-05',2),3,12);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-08-05',2),4,12);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-08-05',2),5,12);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-08-05',2),1,11);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-08-05',2),2,11);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-08-05',2),3,11);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-08-05',2),4,11);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-08-05',2),5,11);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-08-05',2),1,14);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-08-05',2),2,14);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-08-05',2),3,14);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-08-05',2),4,14);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-08-05',2),5,14);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-02-21',2),1,16);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-02-02',2),3,16);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'18-02-21',2),4,16);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'20-03-02',2),1,20);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'20-03-02',2),2,20);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'20-02-21',2),5,20);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-11-08',2),1,26);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-11-08',2),2,26);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-11-08',2),3,26);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-11-08',2),4,26);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-11-08',2),5,26);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-11-08',2),6,26);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-11-08',2),1,27);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-11-08',2),2,27);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-11-08',2),3,27);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-11-08',2),4,27);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-11-08',2),5,27);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-11-08',2),6,27);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-11-08',2),1,28);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-11-08',2),2,28);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-11-08',2),3,28);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-11-08',2),4,28);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-11-08',2),5,28);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-11-08',2),6,28);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-11-08',2),1,29);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-11-08',2),2,29);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-11-08',2),3,29);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-11-08',2),4,29);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-11-08',2),5,29);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-11-08',2),6,29);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-11-08',2),1,30);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-11-08',2),2,30);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-11-08',2),3,30);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-11-08',2),4,30);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-11-08',2),5,30);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-11-08',2),6,30);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-11-08',2),1,31);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-11-08',2),2,31);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-11-08',2),3,31);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-11-08',2),4,31);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-11-08',2),5,31);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-11-08',2),6,31);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-11-08',2),1,32);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-11-08',2),2,32);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-11-08',2),3,32);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-11-08',2),4,32);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-11-08',2),5,32);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'19-11-08',2),6,32);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'20-08-05',2),6,13);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'20-08-05',2),6,3);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'20-08-05',2),6,9);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'20-08-05',2),6,12);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'20-08-05',2),6,11);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'20-05-02',2),6,20);
Insert into PetVaccination (ExpiryDate,VaccinationId,PetId) values (CONVERT(DATE,'20-05-02',2),6,16);


SET IDENTITY_INSERT [dbo].[Reservation] ON
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (800,CONVERT(DATE,'19-07-20',2),CONVERT(DATE,'19-07-26',2),2);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (801,CONVERT(DATE,'19-07-20',2),CONVERT(DATE,'19-07-26',2),2);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (802,CONVERT(DATE,'19-07-20',2),CONVERT(DATE,'19-07-26',2),2);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (803,CONVERT(DATE,'19-07-20',2),CONVERT(DATE,'19-07-26',2),2);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (804,CONVERT(DATE,'19-09-20',2),CONVERT(DATE,'19-09-26',2),2);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (805,CONVERT(DATE,'19-09-20',2),CONVERT(DATE,'19-09-30',2),2);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (806,CONVERT(DATE,'20-03-23',2),CONVERT(DATE,'20-04-03',2),2);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (100,CONVERT(DATE,'17-10-12',2),CONVERT(DATE,'17-10-19',2),5);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (102,CONVERT(DATE,'17-10-16',2),CONVERT(DATE,'17-10-18',2),5);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (103,CONVERT(DATE,'17-11-01',2),CONVERT(DATE,'17-12-05',2),5);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (104,CONVERT(DATE,'17-11-15',2),CONVERT(DATE,'17-11-22',2),5);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (105,CONVERT(DATE,'18-02-01',2),CONVERT(DATE,'18-02-20',2),5);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (106,CONVERT(DATE,'18-05-10',2),CONVERT(DATE,'18-05-17',2),5);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (108,CONVERT(DATE,'18-05-31',2),CONVERT(DATE,'18-06-04',2),5);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (109,CONVERT(DATE,'18-08-01',2),CONVERT(DATE,'18-08-18',2),5);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (110,CONVERT(DATE,'18-08-01',2),CONVERT(DATE,'18-08-18',2),5);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (112,CONVERT(DATE,'18-08-12',2),CONVERT(DATE,'18-08-19',2),5);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (114,CONVERT(DATE,'18-08-15',2),CONVERT(DATE,'18-08-18',2),5);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (115,CONVERT(DATE,'18-08-15',2),CONVERT(DATE,'18-08-17',2),5);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (120,CONVERT(DATE,'18-08-16',2),CONVERT(DATE,'18-08-18',2),5);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (122,CONVERT(DATE,'19-02-01',2),CONVERT(DATE,'19-02-05',2),2);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (123,CONVERT(DATE,'18-05-20',2),CONVERT(DATE,'18-05-27',2),2);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (131,CONVERT(DATE,'19-06-04',2),CONVERT(DATE,'19-06-16',2),2);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (136,CONVERT(DATE,'19-06-04',2),CONVERT(DATE,'19-06-16',2),2);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (137,CONVERT(DATE,'19-05-07',2),CONVERT(DATE,'19-05-15',2),2);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (138,CONVERT(DATE,'19-05-26',2),CONVERT(DATE,'19-06-02',2),2);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (139,CONVERT(DATE,'19-06-04',2),CONVERT(DATE,'19-06-16',2),2);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (140,CONVERT(DATE,'19-06-04',2),CONVERT(DATE,'19-06-16',2),2);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (143,CONVERT(DATE,'19-01-03',2),CONVERT(DATE,'19-01-05',2),5);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (144,CONVERT(DATE,'19-05-26',2),CONVERT(DATE,'19-06-02',2),2);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (145,CONVERT(DATE,'19-06-04',2),CONVERT(DATE,'19-06-16',2),2);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (146,CONVERT(DATE,'19-01-28',2),CONVERT(DATE,'19-02-01',2),5);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (148,CONVERT(DATE,'19-05-26',2),CONVERT(DATE,'19-06-02',2),2);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (594,CONVERT(DATE,'19-01-01',2),CONVERT(DATE,'19-02-05',2),2);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (595,CONVERT(DATE,'19-03-28',2),CONVERT(DATE,'19-04-01',2),2);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (601,CONVERT(DATE,'19-04-01',2),CONVERT(DATE,'19-04-07',2),2);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (603,CONVERT(DATE,'19-04-01',2),CONVERT(DATE,'19-04-07',2),2);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (605,CONVERT(DATE,'19-04-01',2),CONVERT(DATE,'19-04-07',2),2);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (615,CONVERT(DATE,'19-03-07',2),CONVERT(DATE,'19-03-14',2),2);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (620,CONVERT(DATE,'18-05-08',2),CONVERT(DATE,'18-06-09',2),5);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (625,CONVERT(DATE,'19-04-15',2),CONVERT(DATE,'19-04-20',2),2);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (630,CONVERT(DATE,'19-04-05',2),CONVERT(DATE,'19-04-13',2),2);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (631,CONVERT(DATE,'18-02-01',2),CONVERT(DATE,'18-02-04',2),5);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (632,CONVERT(DATE,'19-06-04',2),CONVERT(DATE,'19-06-16',2),2);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (633,CONVERT(DATE,'19-06-04',2),CONVERT(DATE,'19-06-16',2),2);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (635,CONVERT(DATE,'19-04-20',2),CONVERT(DATE,'19-04-25',2),2);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (636,CONVERT(DATE,'19-02-09',2),CONVERT(DATE,'19-02-12',2),2);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (696,CONVERT(DATE,'18-12-07',2),CONVERT(DATE,'18-12-16',2),5);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (700,CONVERT(DATE,'19-02-10',2),CONVERT(DATE,'19-02-12',2),2);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (701,CONVERT(DATE,'19-05-26',2),CONVERT(DATE,'19-06-02',2),2);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (702,CONVERT(DATE,'19-05-26',2),CONVERT(DATE,'19-06-02',2),2);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (703,CONVERT(DATE,'19-02-10',2),CONVERT(DATE,'19-02-12',2),2);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (704,CONVERT(DATE,'19-02-10',2),CONVERT(DATE,'19-02-12',2),2);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (705,CONVERT(DATE,'19-02-10',2),CONVERT(DATE,'19-02-12',2),2);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (707,CONVERT(DATE,'19-04-15',2),CONVERT(DATE,'19-04-20',2),2);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (708,CONVERT(DATE,'19-05-15',2),CONVERT(DATE,'19-05-20',2),2);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (709,CONVERT(DATE,'19-05-15',2),CONVERT(DATE,'19-05-20',2),2);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (711,CONVERT(DATE,'19-05-15',2),CONVERT(DATE,'19-05-20',2),2);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (712,CONVERT(DATE,'19-05-15',2),CONVERT(DATE,'19-05-20',2),2);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (713,CONVERT(DATE,'19-05-10',2),CONVERT(DATE,'19-05-25',2),2);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (716,CONVERT(DATE,'19-05-10',2),CONVERT(DATE,'19-05-25',2),2);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (717,CONVERT(DATE,'19-05-10',2),CONVERT(DATE,'19-05-25',2),2);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (720,CONVERT(DATE,'19-05-25',2),CONVERT(DATE,'19-05-31',2),2);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (721,CONVERT(DATE,'19-05-05',2),CONVERT(DATE,'19-05-09',2),2);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (808,CONVERT(DATE,'20-01-31',2),CONVERT(DATE,'20-02-12',2),2);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (809,CONVERT(DATE,'19-08-02',2),CONVERT(DATE,'19-08-09',2),2);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (810,CONVERT(DATE,'19-04-12',2),CONVERT(DATE,'19-04-17',2),2);
Insert into Reservation (ReservationId,StartDate,EndDate,Status) values (811,CONVERT(DATE,'19-07-26',2),CONVERT(DATE,'19-08-05',2),2);
SET IDENTITY_INSERT [dbo].[Reservation] OFF


Insert into ReservationDiscount (DiscountId,ReservationId) values (2,713);
Insert into ReservationDiscount (DiscountId,ReservationId) values (2,717);


SET IDENTITY_INSERT [dbo].[Run] ON
Insert into Run (RunId,Size,Covered,Location,Status) values (1,'R',0,'F',1);
Insert into Run (RunId,Size,Covered,Location,Status) values (2,'R',0,'F',1);
Insert into Run (RunId,Size,Covered,Location,Status) values (3,'R',0,'F',1);
Insert into Run (RunId,Size,Covered,Location,Status) values (4,'R',0,'F',1);
Insert into Run (RunId,Size,Covered,Location,Status) values (5,'R',0,'F',1);
Insert into Run (RunId,Size,Covered,Location,Status) values (6,'L',0,'F',1);
Insert into Run (RunId,Size,Covered,Location,Status) values (7,'L',0,'F',1);
Insert into Run (RunId,Size,Covered,Location,Status) values (8,'L',0,'F',1);
Insert into Run (RunId,Size,Covered,Location,Status) values (9,'L',0,'F',1);
Insert into Run (RunId,Size,Covered,Location,Status) values (10,'L',0,'F',1);
Insert into Run (RunId,Size,Covered,Location,Status) values (11,'L',0,'F',1);
Insert into Run (RunId,Size,Covered,Location,Status) values (12,'L',0,'F',1);
Insert into Run (RunId,Size,Covered,Location,Status) values (13,'L',0,'B',1);
Insert into Run (RunId,Size,Covered,Location,Status) values (14,'L',0,'B',1);
Insert into Run (RunId,Size,Covered,Location,Status) values (15,'L',1,'B',1);
Insert into Run (RunId,Size,Covered,Location,Status) values (16,'L',1,'B',1);
Insert into Run (RunId,Size,Covered,Location,Status) values (17,'L',1,'B',1);
Insert into Run (RunId,Size,Covered,Location,Status) values (18,'R',1,'B',1);
Insert into Run (RunId,Size,Covered,Location,Status) values (19,'R',1,'B',1);
Insert into Run (RunId,Size,Covered,Location,Status) values (20,'R',1,'B',1);
Insert into Run (RunId,Size,Covered,Location,Status) values (21,'R',1,'B',1);
Insert into Run (RunId,Size,Covered,Location,Status) values (22,'R',1,'B',1);
Insert into Run (RunId,Size,Covered,Location,Status) values (23,'R',1,'B',1);
Insert into Run (RunId,Size,Covered,Location,Status) values (24,'R',1,'B',1);
SET IDENTITY_INSERT [dbo].[Run] OFF

SET IDENTITY_INSERT [dbo].[Service] ON
Insert into Service (ServiceId,ServiceDescription) values (1,'Boarding');
Insert into Service (ServiceId,ServiceDescription) values (2,'Walk');
Insert into Service (ServiceId,ServiceDescription) values (4,'Medication');
Insert into Service (ServiceId,ServiceDescription) values (5,'Playtime');
SET IDENTITY_INSERT [dbo].[Service] OFF


SET IDENTITY_INSERT [dbo].[Vaccination] ON
Insert into Vaccination (VaccinationId,Name) values (1,'Bordetella');
Insert into Vaccination (VaccinationId,Name) values (2,'Distemper');
Insert into Vaccination (VaccinationId,Name) values (3,'Hepatitis');
Insert into Vaccination (VaccinationId,Name) values (4,'Parainfluenza');
Insert into Vaccination (VaccinationId,Name) values (5,'Parovirus');
Insert into Vaccination (VaccinationId,Name) values (6,'Rabies');
SET IDENTITY_INSERT [dbo].[Vaccination] OFF
GO

--------------------------------------------------------
--  DDL for Index DAILY_RATE_PK
--------------------------------------------------------
CREATE UNIQUE INDEX "IX_DAILY_RATE_PK" ON "DailyRate" ("DailyRateId") 
GO

--------------------------------------------------------
--  DDL for Index DISCOUNT_PK
--------------------------------------------------------
CREATE UNIQUE INDEX "IX_DISCOUNT_PK" ON "Discount" ("DiscountId") 
GO

--------------------------------------------------------
--  DDL for Index PET_RES_DISC_PK
--------------------------------------------------------
CREATE UNIQUE INDEX "IX_PET_RES_DISC_PK" ON "PetReservationDiscount" ("DiscountId", "PetReservationId") 
GO


--------------------------------------------------------
--  DDL for Index RUN_PK
--------------------------------------------------------
CREATE UNIQUE INDEX "IX_RUN_PK" ON "Run" ("RunId") 
GO

--------------------------------------------------------
--  DDL for Index VACCINATION_PK
--------------------------------------------------------
CREATE UNIQUE INDEX "IX_VACCINATION_PK" ON "Vaccination" ("VaccinationId") 
GO
 
--------------------------------------------------------
--  DDL for Index PET_VACCINATION_PK
--------------------------------------------------------
CREATE UNIQUE INDEX "IX_PET_VACCINATION_PK" ON "PetVaccination" ("VaccinationId", "PetId") 
GO



--------------------------------------------------------
--  DDL for Index PET_RESERVATION_PK
--------------------------------------------------------
CREATE UNIQUE INDEX "IX_PET_RESERVATION_PK" ON "PetReservation" ("PetReservationId") 
GO

--------------------------------------------------------
--  DDL for Index HVKUser_PK
--------------------------------------------------------
CREATE UNIQUE INDEX "IX_HVKUser_PK" ON "HVKUser" ("HVKUserId") 
GO

--------------------------------------------------------
--  DDL for Index SERVICE_PK
--------------------------------------------------------
CREATE UNIQUE INDEX "IX_SERVICE_PK" ON "Service" ("ServiceId") 
GO

--------------------------------------------------------
--  DDL for Index PET_RESERVATION_SERVICE_PK
--------------------------------------------------------
CREATE UNIQUE INDEX "IX_PET_RESERVATION_SERVICE_PK" ON "PetReservationService" ("PetReservationId", "ServiceId") 
GO

--------------------------------------------------------
--  DDL for Index RES_DISC_PK
--------------------------------------------------------
CREATE UNIQUE INDEX "IX_RES_DISC_PK" ON "ReservationDiscount" ("DiscountId", "ReservationId") 
GO

--------------------------------------------------------
--  DDL for Index RESERVATION_PK
--------------------------------------------------------
CREATE UNIQUE INDEX "IX_RESERVATION_PK" ON "Reservation" ("ReservationId") 
GO

--------------------------------------------------------
--  DDL for Index PET_PK
--------------------------------------------------------
CREATE UNIQUE INDEX "IX_PET_PK" ON "Pet" ("PetId") 
GO

--------------------------------
-- CONSTRAINTS
--------------------------------



--------------------------------------------------------
--  Constraints for Table Run
--------------------------------------------------------

ALTER TABLE [dbo].[Run]
 ADD CONSTRAINT [Location_CHK]
 CHECK (Location IN ( 'B', 'F' ))

GO


ALTER TABLE [dbo].[Run]
 ADD CONSTRAINT [Size_CHK]
 CHECK (Size IN ( 'L', 'R' ))

GO

ALTER TABLE [dbo].[Run]
 ADD CONSTRAINT [Status_CHK]
 CHECK (Status IN ( 1, 2, 3))

GO

ALTER TABLE  [dbo].[Run]
 ADD DEFAULT 'R' FOR [Size]
GO

ALTER TABLE  [dbo].[Run]
 ADD DEFAULT 1 FOR [Status]
GO




--------------------------------------------------------
--  Constraints for Table PetReservationDiscount
--------------------------------------------------------

ALTER TABLE [dbo].[PetReservationDiscount]
 ADD CONSTRAINT [PET_RES_DISC_PK]
   PRIMARY KEY
   CLUSTERED ([DiscountId] ASC, [PetReservationId] ASC)

GO

ALTER TABLE [dbo].[PetReservationDiscount]
 ADD CONSTRAINT [PRD_PR_FK]
 FOREIGN KEY 
   ([PetReservationId])
 REFERENCES 
   [dbo].[PetReservation]     ([PetReservationId])
    ON DELETE NO ACTION
    ON UPDATE NO ACTION

GO


ALTER TABLE [dbo].[PetReservationDiscount]
 ADD CONSTRAINT [PRD_DISC_FK]
 FOREIGN KEY 
   ([DiscountId])
 REFERENCES 
   [dbo].[Discount]     ([DiscountId])
    ON DELETE NO ACTION
    ON UPDATE NO ACTION

GO

--------------------------------------------------------
--  Constraints for Table DailyRate
--------------------------------------------------------

ALTER TABLE [dbo].[DailyRate]
 ADD CONSTRAINT [SYS_C00131069]
 CHECK (DogSize IN ( 'L', 'M', 'S' ))

GO

--------------------------------------------------------
--  Constraints for Table Reservation
--------------------------------------------------------

ALTER TABLE [dbo].[Reservation]
 ADD CONSTRAINT [RES_STATUS_CHK]
 CHECK (Status IN ( 
2, 
3, 
4, 
5 ))

GO

ALTER TABLE  [dbo].[Reservation]
 ADD DEFAULT 2 FOR [Status]
GO

--------------------------------------------------------
--  Constraints for Table PetVaccination
--------------------------------------------------------

ALTER TABLE [dbo].[PetVaccination]
 ADD CONSTRAINT [PET_VACCINATION_PK]
   PRIMARY KEY
   CLUSTERED ([VaccinationId] ASC, [PetId] ASC)

GO

ALTER TABLE [dbo].[PetVaccination]
 ADD CONSTRAINT [PV_VACC_FK]
 FOREIGN KEY 
   ([VaccinationId])
 REFERENCES 
   [dbo].[Vaccination]     ([VaccinationId])
    ON DELETE NO ACTION
    ON UPDATE NO ACTION

GO

ALTER TABLE [dbo].[PetVaccination]
 ADD CONSTRAINT [PV_PET_FK]
 FOREIGN KEY 
   ([PetId])
 REFERENCES 
   [dbo].[Pet]     ([PetId])
    ON DELETE NO ACTION
    ON UPDATE NO ACTION

GO



--------------------------------------------------------
--  Constraints for Table ReservationDiscount
--------------------------------------------------------

ALTER TABLE [dbo].[ReservationDiscount]
 ADD CONSTRAINT [RES_DISC_PK]
   PRIMARY KEY
   CLUSTERED ([DiscountId] ASC, [ReservationId] ASC)

GO

--------------------------------------------------------
--  Constraints for Table Discount
--------------------------------------------------------


ALTER TABLE [dbo].[Discount]
 ADD CONSTRAINT [Type_CHK]
 CHECK (Type IN ( 'D', 'T' ))

GO

ALTER TABLE  [dbo].[Discount]
 ADD DEFAULT 'D' FOR [Type]
GO
  
  
--------------------------------------------------------
--  Constraints for Table PetReservationService
--------------------------------------------------------

ALTER TABLE [dbo].[PetReservationService]
 ADD CONSTRAINT [PET_RESERVATION_SERVICE_PK]
   PRIMARY KEY
   CLUSTERED ([PetReservationId] ASC, [ServiceId] ASC)

GO



--------------------------------------------------------
--  Ref Constraints for Table DailyRate
--------------------------------------------------------

ALTER TABLE [dbo].[DailyRate]
 ADD CONSTRAINT [DR_SERV_FK]
 FOREIGN KEY 
   ([ServiceId])
 REFERENCES 
   [dbo].[Service]     ([ServiceId])
    ON DELETE NO ACTION
    ON UPDATE NO ACTION

GO
 
 

--------------------------------------------------------
--  Ref Constraints for Table PetReservation
--------------------------------------------------------

ALTER TABLE [dbo].[PetReservation]
 ADD CONSTRAINT [PR_RES_FK]
 FOREIGN KEY 
   ([ReservationId])
 REFERENCES 
   [dbo].[Reservation]     ([ReservationId])
    ON DELETE NO ACTION
    ON UPDATE NO ACTION

GO


ALTER TABLE [dbo].[PetReservation]
 ADD CONSTRAINT [PR_RUN_FK]
 FOREIGN KEY 
   ([RunId])
 REFERENCES 
   [dbo].[Run]     ([RunId])
    ON DELETE NO ACTION
    ON UPDATE NO ACTION

GO

ALTER TABLE [dbo].[PetReservation]
 ADD CONSTRAINT [PR_PET_FK]
 FOREIGN KEY 
   ([PetId])
 REFERENCES 
   [dbo].[Pet]     ([PetId])
    ON DELETE NO ACTION
    ON UPDATE NO ACTION

GO

--------------------------------------------------------
--  Ref Constraints for Medication
--------------------------------------------------------

ALTER TABLE [dbo].[Medication]
 ADD CONSTRAINT [MED_PR_FK]
 FOREIGN KEY 
   ([PetReservationId])
 REFERENCES 
   [dbo].[PetReservation]     ([PetReservationId])
    ON DELETE NO ACTION
    ON UPDATE NO ACTION

GO

--------------------------------------------------------
--  Ref Constraints for Table PetReservationService
--------------------------------------------------------

ALTER TABLE [dbo].[PetReservationService]
 ADD CONSTRAINT [PRS_PR_FK]
 FOREIGN KEY 
   ([PetReservationId])
 REFERENCES 
   [dbo].[PetReservation]     ([PetReservationId])
    ON DELETE NO ACTION
    ON UPDATE NO ACTION

GO


ALTER TABLE [dbo].[PetReservationService]
 ADD CONSTRAINT [PRS_SERV_FK]
 FOREIGN KEY 
   ([ServiceId])
 REFERENCES 
   [dbo].[Service]     ([ServiceId])
    ON DELETE NO ACTION
    ON UPDATE NO ACTION

GO

 --------------------------------------------------------
--  Ref Constraints for Table ReservationDiscount
--------------------------------------------------------

ALTER TABLE [dbo].[ReservationDiscount]
 ADD CONSTRAINT [RD_DISC_FK]
 FOREIGN KEY 
   ([DiscountId])
 REFERENCES 
   [dbo].[Discount]     ([DiscountId])
    ON DELETE NO ACTION
    ON UPDATE NO ACTION

GO


ALTER TABLE [dbo].[ReservationDiscount]
 ADD CONSTRAINT [RD_RES_FK]
 FOREIGN KEY 
   ([ReservationId])
 REFERENCES 
   [dbo].[Reservation]     ([ReservationId])
    ON DELETE NO ACTION
    ON UPDATE NO ACTION

GO



--------------------------------------------------------
--  Ref Constraints for Table HVKUser
--------------------------------------------------------


ALTER TABLE [dbo].[HVKUser]
 ADD CONSTRAINT [SYS_C00134635]
 CHECK (Province IN ( 'ON', 'QC' ))

GO


ALTER TABLE  [dbo].[HVKUser]
 ADD DEFAULT 'QC' FOR [Province]
GO

--------------------------------------------------------
--  Ref Constraints for Table Pet
--------------------------------------------------------

ALTER TABLE [dbo].[Pet]
 ADD CONSTRAINT [SYS_C00262057]
 CHECK (Gender IN ( 'F', 'M' ))

GO


ALTER TABLE [dbo].[Pet]
 ADD CONSTRAINT [SYS_C00134656]
 CHECK (DogSize = 'L' OR DogSize = 'M' OR DogSize = 'S')

GO

ALTER TABLE [dbo].[Pet]
 ADD CONSTRAINT [PET_OWN_FK]
 FOREIGN KEY 
   ([HVKUserId])
 REFERENCES 
   [dbo].[HVKUser]     ([HVKUserId])
    ON DELETE CASCADE
    ON UPDATE NO ACTION

GO

-------------------------------------------------------
-- Updates to raw data previously entered
--
-- Original script had some conversion done to age reservations,
-- vaccinations based on hardcoded values 
-- and dates.  Those continue to be aged now based on today's date
-- minus those starting points, as off 13-JAN-22.
--------------------------------------------------------

DECLARE @Today DATE;
DECLARE @ThreeMonthsAgo DATE;
DECLARE @DeltaMonths INT;

DECLARE @StartingDate DATE = CONVERT(DATE,'13-JAN-22',6);
SET @Today = CONVERT (DATE, GETDATE());
SET @DeltaMonths = DATEDIFF(MONTH,@StartingDate,@Today);
SET @ThreeMonthsAgo = DATEADD(MONTH,-3,@Today);


--- Bump up reservations by 5 years +Delta
UPDATE [dbo].[Reservation]
SET StartDate = DATEADD(month, (60 + @DeltaMonths), StartDate),
EndDate = DATEADD(month, (60 + @DeltaMonths), EndDate)
WHERE StartDate IS NOT NULL AND EndDate IS NOT NULL;


--- Mark anything in the past as completed (status = 5)
--- anything in the future or today (status = 2) confirmed
UPDATE [dbo].[Reservation]
SET Status=5 where EndDate < @Today;
UPDATE Reservation
SET Status=2 where StartDate >= @Today;

-- Bump up vaccination expiry dates by 5 years + delta
UPDATE [dbo].[PetVaccination]
SET ExpiryDate = DATEADD(month, (60 + @DeltaMonths), ExpiryDate)
WHERE ExpiryDate IS NOT NULL;

-- Mark vaccination as checked if the pet has been in during the last 3 months
UPDATE [dbo].[PetVaccination]
SET VaccinationChecked = 1
WHERE
PetId IN (
SELECT DISTINCT (pr.PetID) FROM PetReservation pr, Reservation r
WHERE ((r.ReservationId = pr.ReservationId)
AND (r.EndDate >= @ThreeMonthsAgo)));