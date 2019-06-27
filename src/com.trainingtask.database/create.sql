CREATE LOGIN test WITH PASSWORD = 'test123!_';

CREATE DATABASE TrainingTask;

USE TrainingTask;

CREATE USER testUser FOR LOGIN test;

-- Table: Employee

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Employee') 
BEGIN 
	CREATE TABLE Employee
	(
    Id INTEGER IDENTITY(1,1) NOT NULL PRIMARY KEY,
    LastName NVARCHAR(255) NOT NULL,
    FirstName NVARCHAR(255) NOT NULL,
    MiddleName NVARCHAR(255),
    Position NVARCHAR(255) NOT NULL,
	IsDeleted INTEGER NOT NULL
	);
END

-- Table: Project

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Project') 
BEGIN 
	CREATE TABLE Project
	(
		Id INTEGER IDENTITY(1,1) NOT NULL PRIMARY KEY,
		Name NVARCHAR(255) NOT NULL,
		ShortName NVARCHAR(255) NOT NULL,
		Description NVARCHAR(4000),
		IsDeleted INTEGER NOT NULL
	);
END

-- Table: Task

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Issue') 
BEGIN 
	CREATE TABLE Issue
	(
		Id INTEGER IDENTITY(1,1) NOT NULL PRIMARY KEY,
		Name NVARCHAR(255) NOT NULL,
		BeginDate DATE NOT NULL,
		EndDate DATE NOT NULL,
		Work INTEGER NOT NULL,
		ProjectId INTEGER NOT NULL,
		StatusId INTEGER NOT NULL,
		EmployeeId INTEGER NOT NULL,
		IsDeleted INTEGER NOT NULL,
		CONSTRAINT fk_ProjectId FOREIGN KEY (ProjectId)
			REFERENCES Project (Id)
			ON UPDATE CASCADE
			ON DELETE CASCADE,
		CONSTRAINT fk_EmployeeId FOREIGN KEY (EmployeeId)
			REFERENCES Employee (Id)
			ON UPDATE NO ACTION
			ON DELETE NO ACTION
	);
END

-- user permissions

GRANT SELECT, UPDATE, INSERT, DELETE on Employee to testUser;

GRANT SELECT, UPDATE, INSERT, DELETE on Project to testUser;

GRANT SELECT, UPDATE, INSERT, DELETE on Issue to testUser;