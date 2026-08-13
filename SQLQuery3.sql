SELECT Id, Email, UserName
FROM AspNetUsers
WHERE Email = 'patient@hospital.com';

SELECT *
FROM Patients
WHERE UserId = '31859f77-b2f2-49c3-bdb2-116c64f9d5d2';

SELECT 
    COLUMN_NAME,
    DATA_TYPE,
    IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'Patients'
ORDER BY ORDINAL_POSITION;

SELECT TOP 10 *
FROM Patients;

INSERT INTO Patients
(
    UserId,
    FullName,
    DateOfBirth,
    PhoneNumber,
    Address,
    Email
)
VALUES
(
    '31859f77-b2f2-49c3-bdb2-116c64f9d5d2',
    'Test Patient',
    '2002-01-15',
    '9800000000',
    'Kathmandu, Nepal',
    'patient@hospital.com'
);

SELECT *
FROM Patients
WHERE UserId = '31859f77-b2f2-49c3-bdb2-116c64f9d5d2';

SELECT
    u.Id,
    u.Email,
    p.PatientId,
    p.FullName
FROM AspNetUsers u
INNER JOIN Patients p
    ON u.Id = p.UserId
WHERE u.Email = 'patient@hospital.com';

SELECT *
FROM Appointments
ORDER BY AppointmentId DESC;

SELECT COLUMN_NAME
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'AspNetUsers';