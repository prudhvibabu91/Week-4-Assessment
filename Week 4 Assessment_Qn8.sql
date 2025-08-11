CREATE OR ALTER PROCEDURE AddOrUpdateEmployee
    @employeeId INT,
    @name NVARCHAR(50),
    @exp INT,
    @salary INT,
    @departmentId INT
AS
BEGIN
    IF EXISTS (SELECT 1 FROM Employee WHERE employeeId = @employeeId)
    BEGIN
        UPDATE Employee
        SET name = @name,
            exp = @exp,
            salary = @salary,
            departmentId = @departmentId
        WHERE employeeId = @employeeId;
        PRINT 'Record Updated';
    END
    ELSE
    BEGIN
        INSERT INTO Employee (employeeId, name, exp, salary, departmentId)
        VALUES (@employeeId, @name, @exp, @salary, @departmentId);
        PRINT 'Record Added';
    END
END;
GO

-- Sample input: inserting a new record
EXEC AddOrUpdateEmployee 
    @employeeId = 1,
    @name = 'John Doe',
    @exp = 5,
    @salary = 20000,
    @departmentId = 2;

-- View table data
SELECT * FROM Employee;
