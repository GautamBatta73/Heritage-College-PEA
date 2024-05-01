USE HVKW24_Team7;
GO
BEGIN TRANSACTION;
--BEFORE Inserting into Reservation table
SELECT * FROM dbo.Reservation ORDER BY ReservationID DESC;

SET IDENTITY_INSERT dbo.Reservation ON;
insert Reservation (ReservationID, StartDate, EndDate)
values (2005, '2024-04-28', '2024-04-29');
insert Reservation (ReservationID, StartDate, EndDate)
values (2006, '2024-04-29', '2024-04-30');
insert Reservation (ReservationID, StartDate, EndDate)
values (2007, '2024-04-30', '2024-05-01');
SET IDENTITY_INSERT dbo.Reservation OFF;

--AFTER Inserting into Reservation table
SELECT * FROM dbo.Reservation ORDER BY ReservationID DESC;

---------------------------------------------------------------------------------------------------------------------

--BEFORE Inserting into PetReservation table
SELECT * FROM dbo.PetReservation ORDER BY PetReservationID DESC;

SET IDENTITY_INSERT dbo.PetReservation ON;
insert PetReservation (PetReservationID, PetId, ReservationId)
values (1960, 1, 2005);
insert PetReservation (PetReservationID, PetId, ReservationId)
values (1961, 1, 2005);
insert PetReservation (PetReservationID, PetId, ReservationId)
values (1962, 2, 2006);
insert PetReservation (PetReservationID, PetId, ReservationId)
values (1963, 3, 2007);
SET IDENTITY_INSERT dbo.PetReservation OFF;

--AFTER Inserting into PetReservation table
SELECT * FROM dbo.PetReservation ORDER BY PetReservationID DESC;
ROLLBACK TRANSACTION;
GO