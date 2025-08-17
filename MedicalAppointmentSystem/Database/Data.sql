--Create DB
CREATE DATABASE MedicalDB;
GO
USE MedicalDB;
GO

-- Doctors
CREATE TABLE Doctors (
    DoctorID INT IDENTITY(1,1) PRIMARY KEY,
    FullName  VARCHAR(100) NOT NULL,
    Specialty VARCHAR(100) NOT NULL,
    Availability BIT NOT NULL DEFAULT 1
);

--Patients
CREATE TABLE Patients (
    PatientID INT IDENTITY(1,1) PRIMARY KEY,
    FullName VARCHAR(100) NOT NULL,
    Email    VARCHAR(150) NOT NULL
);

--Appointments
CREATE TABLE Appointments (
    AppointmentID INT IDENTITY(1,1) PRIMARY KEY,
    DoctorID INT NOT NULL FOREIGN KEY REFERENCES Doctors(DoctorID),
    PatientID INT NOT NULL FOREIGN KEY REFERENCES Patients(PatientID),
    AppointmentDate DATETIME NOT NULL,
    Notes VARCHAR(400) NULL
);

--Sample Doctors
INSERT INTO Doctors (FullName, Specialty, Availability) VALUES
('Dr. Alice Johnson','Cardiology',1),
('Dr. Bob Smith', 'Dermatology', 1),
('Dr. Clara Wilson', 'Neurology', 0),
('Dr. David Brown', 'Pediatrics', 1);

--Sample Patients
INSERT INTO Patients (FullName, Email) VALUES
('John Doe','john@example.com'),
('Mary Jane', 'mary@example.com'),
('Tom Lee', 'tom@example.com');
