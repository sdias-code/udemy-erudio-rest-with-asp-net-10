DECLARE @i INT = 1;

WHILE @i <= 1000
BEGIN
    INSERT INTO dbo.books (author, launch_date, price, title)
    VALUES (
        CONCAT('Author ', @i),
        DATEADD(DAY, -ABS(CHECKSUM(NEWID())) % 4000, GETDATE()),
        ROUND(RAND(CHECKSUM(NEWID())) * 150 + 20, 2),
        CONCAT('Programming Book Volume ', @i)
    );

    SET @i = @i + 1;
END
