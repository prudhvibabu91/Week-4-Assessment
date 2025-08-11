CREATE OR ALTER PROCEDURE DeleteEmployee
    @employeeId INT
AS
BEGIN
    
    UPDATE Employee
    SET managerId = NULL
    WHERE managerId = @employeeId;

    DELETE FROM Employee 
    WHERE employeeId = @employeeId;

    PRINT 'Record Deleted successfully';
END;
GO
EXEC DeleteEmployee @employeeId = 1;

SELECT * FROM Employee;

