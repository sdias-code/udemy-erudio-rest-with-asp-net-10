ALTER TABLE dbo.person
ADD enabled BIT NOT NULL CONSTRAINT DF_person_enabled DEFAULT 1;
