CREATE OR ALTER PROCEDURE DeleteEmployee
    @employeeId INT
AS
BEGIN
    -- Step 1: Remove this employee as a manager for others
    UPDATE Employee
    SET managerId = NULL
    WHERE managerId = @employeeId;

    -- Step 2: Delete the employee
    DELETE FROM Employee 
    WHERE employeeId = @employeeId;

    PRINT 'Record Deleted';
END;
GO

-- Sample execution
EXEC DeleteEmployee @employeeId = 1;

-- Check remaining records
SELECT * FROM Employee;
