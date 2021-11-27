Use [AnReshProbation]
CREATE TABLE Skills
(
Id INT IDENTITY (1, 1) PRIMARY KEY,
Name NVARCHAR(20) NOT NULL UNIQUE,
)

INSERT Skills(Name) VALUES
('C#'),
('Java'),
('MVC'),
('VueJS'),
('Anhular');