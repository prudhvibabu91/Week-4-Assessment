-- Check if tables exist and drop them to prevent errors on re-run
IF OBJECT_ID('dbo.ContractEmployees', 'U') IS NOT NULL
DROP TABLE dbo.ContractEmployees;

IF OBJECT_ID('dbo.PayrollEmployees', 'U') IS NOT NULL
DROP TABLE dbo.PayrollEmployees;

IF OBJECT_ID('dbo.Employees', 'U') IS NOT NULL
DROP TABLE dbo.Employees;


-- Create the Employees table for common data
CREATE TABLE Employees (
    EmployeeID INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    ReportingManager NVARCHAR(100) NOT NULL,
    EmployeeType NVARCHAR(20) NOT NULL -- 'Payroll' or 'Contract'
);

-- Create the PayrollEmployees table
CREATE TABLE PayrollEmployees (
    EmployeeID INT PRIMARY KEY,
    JoiningDate DATE NOT NULL,
    Experience INT NOT NULL,
    BasicSalary DECIMAL(18, 2) NOT NULL,
    DA DECIMAL(18, 2) NULL,
    HRA DECIMAL(18, 2) NULL,
    PF DECIMAL(18, 2) NULL,
    NetSalary DECIMAL(18, 2) NULL,
    FOREIGN KEY (EmployeeID) REFERENCES Employees(EmployeeID)
);

-- Create the ContractEmployees table
CREATE TABLE ContractEmployees (
    EmployeeID INT PRIMARY KEY,
    ContractDate DATE NOT NULL,
    Duration INT NOT NULL, -- Duration in months
    Charges DECIMAL(18, 2) NOT NULL,
    FOREIGN KEY (EmployeeID) REFERENCES Employees(EmployeeID)
);