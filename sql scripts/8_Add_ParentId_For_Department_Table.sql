use AnReshProbation
ALTER TABLE Departments ADD ParentId INT NULL,
FOREIGN KEY (ParentId) REFERENCES Departments (Id);