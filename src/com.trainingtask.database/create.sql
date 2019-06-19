CREATE LOGIN test WITH PASSWORD = 'test123!_';

CREATE DATABASE TrainingTask;

USE TrainingTask;

CREATE USER testUser FOR LOGIN test;

-- Table: employee

CREATE TABLE Employee
(
    id INTEGER IDENTITY NOT NULL PRIMARY KEY,
    lastname NVARCHAR(255) NOT NULL,
    firstname NVARCHAR(255) NOT NULL,
    middlename NVARCHAR(255),
    position NVARCHAR(255) NOT NULL
);

-- Table: project

CREATE TABLE Project
(
    id INTEGER IDENTITY NOT NULL PRIMARY KEY,
    name NVARCHAR(255) NOT NULL,
    shortname NVARCHAR(255) UNIQUE,
    description NVARCHAR(4000)
);

-- Table: task

CREATE TABLE Issue
(
    id INTEGER IDENTITY NOT NULL PRIMARY KEY,
    name NVARCHAR(255) NOT NULL,
    begindate DATE NOT NULL,
    enddate DATE NOT NULL,
    work INTEGER NOT NULL,
    projectid INTEGER NOT NULL,
    statusid INTEGER NOT NULL,
    employeeid INTEGER NOT NULL,
    CONSTRAINT fk_projectid FOREIGN KEY (projectid)
        REFERENCES project (id)
        ON UPDATE CASCADE
        ON DELETE CASCADE,
    CONSTRAINT fk_employeeid FOREIGN KEY (employeeid)
        REFERENCES employee (id)
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
);

grant select, update, insert, delete on Employee to testUser;

grant select, update, insert, delete on Project to testUser;

grant select, update, insert, delete on Issue to testUser;