Use [AnReshProbation]
CREATE TABLE Departments
(
Id INT IDENTITY (1, 1) PRIMARY KEY,
Name NVARCHAR(20) NOT NULL UNIQUE,
)

INSERT Departments(Name) VALUES
('Development'),
('Testing'),
('HR');