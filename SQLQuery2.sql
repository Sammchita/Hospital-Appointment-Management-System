SELECT
    u.Email,
    r.Name AS RoleName
FROM AspNetUsers u
LEFT JOIN AspNetUserRoles ur
    ON u.Id = ur.UserId
LEFT JOIN AspNetRoles r
    ON ur.RoleId = r.Id
WHERE u.Email = 'admin@hospital.com';

SELECT Id, Name, NormalizedName
FROM AspNetRoles;

SELECT
    u.Email,
    u.UserName,
    r.Name AS RoleName
FROM AspNetUsers u
INNER JOIN AspNetUserRoles ur
    ON u.Id = ur.UserId
INNER JOIN AspNetRoles r
    ON ur.RoleId = r.Id
WHERE u.Email = 'admin@hospital.com';