DECLARE @startDate DATE = DATEADD(MONTH, -12, GETDATE());
DECLARE @endDate DATE = GETDATE();

DECLARE @currentDate DATE = @startDate;
WHILE @currentDate <= @endDate
BEGIN
    DECLARE @IdentificationNumber VARCHAR(50) = NEWID();

    INSERT INTO invoices (PayerName, IdentificationNumber, IssueDate, BillingDate, Amount, StatusId)
    VALUES
    ('Empresa A', @IdentificationNumber, @currentDate, DATEADD(DAY, 15, @currentDate), RAND() * 1000, CASE WHEN @currentDate < DATEADD(MONTH, -6, GETDATE()) THEN 3 ELSE 4 END);

    SET @currentDate = DATEADD(MONTH, 1, @currentDate);
END;