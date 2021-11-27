Use [AnReshProbation]
CREATE TABLE EmployeesSkills
(
Id INT IDENTITY (1, 1) PRIMARY KEY,
EmployeeId INT NOT NULL,
SkillId INT,
FOREIGN KEY (EmployeeId) REFERENCES Employees (Id)
ON DELETE CASCADE,
FOREIGN KEY (SkillId) REFERENCES Skills (Id)
ON DELETE CASCADE
)

INSERT EmployeesSkills(EmployeeId, SkillId) VALUES
(2,1),
(2,2),
(3,4);