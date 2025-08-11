DROP FUNCTION IF EXISTS CountWords;
GO

CREATE FUNCTION CountWords (@sentence NVARCHAR(MAX))
RETURNS INT
AS
BEGIN
    DECLARE @count INT;
    SET @sentence = LTRIM(RTRIM(@sentence));
    SET @sentence = REPLACE(@sentence, '  ', ' ');
    SET @count = LEN(@sentence) - LEN(REPLACE(@sentence, ' ', '')) + 1;
    RETURN @count;
END;
GO

SELECT dbo.CountWords('Hello how are you today') AS WordCount;
