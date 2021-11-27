Use [AnReshProbation]
CREATE TABLE Employees
(
Id INT IDENTITY (1, 1) PRIMARY KEY,
FirstName NVARCHAR(20) NOT NULL,
LastName NVARCHAR(20) NOT NULL,
MiddleName NVARCHAR(20),
DepartmentID INT NOT NULL,
Salary DECIMAL(18,2) NOT NULL,
FOREIGN KEY (DepartmentID) REFERENCES Departments (Id)
)

INSERT Employees(FirstName, LastName, MiddleName, DepartmentID, Salary) VALUES
('Walter', 'Horton', null, 1, 1000),
('Dan', 'Coleman', null, 2, 50000),
('Darrell', 'Jones', 'Miller', 1, 80500),
('Jerry', 'Gray', 'Jackson', 1, 100500),
('Bobby', 'McGee', 'Newman', 3, 7500);