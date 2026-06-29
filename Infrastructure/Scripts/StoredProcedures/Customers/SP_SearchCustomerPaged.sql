CREATE OR ALTER PROCEDURE SP_SearchCustomerPaged
    @SearchWord NVARCHAR(100),
    @PageNumber INT,
    @RowPerPage INT
AS
BEGIN

    SET NOCOUNT ON;

    ;WITH SearchCTE AS
    (
        SELECT 
            c.CustomerID,
            c.CustomerNumber,
            p.Name,
            c.Address,
            dbo.GetFirstPersonPhoneNumber(p.PersonID) AS PhoneNumber,
            COUNT(*) OVER() AS TotalCount

        FROM Persons p
        INNER JOIN Customers c ON p.PersonID = c.PersonID

        WHERE 
             p.IsActive = 1

           AND (
                (@SearchWord IS NULL OR @SearchWord = '')
                OR c.CustomerNumber = @SearchWord
                OR p.Name LIKE N'%' + @SearchWord + N'%'
                OR c.CustomerID = TRY_CAST(@SearchWord AS INT)
            )
    )

    SELECT *
    FROM SearchCTE
    ORDER BY CustomerID DESC
    OFFSET (@PageNumber - 1) * @RowPerPage ROWS
    FETCH NEXT @RowPerPage ROWS ONLY;

END