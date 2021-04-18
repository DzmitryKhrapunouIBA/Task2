namespace IBA_Task_2.DbData
{
    public class StoredProcedures
    {
        /// <summary>
        /// Procedure for retrieving all users from the database
        /// </summary>
        public string sp_GetAllUsers = @"CREATE PROCEDURE [dbo].[sp_GetAllUsers]
                       AS
                       SELECT Users.Id, Users.FirstName, Users.LastName, Users.SurName, Users.Date, Cities.CityName, Countries.CountryName 
                       FROM Users
                       JOIN Cities ON Cities.Id=Users.Id
                       JOIN Countries ON Countries.Id=Users.Id
                       GO";

        /// <summary>
        /// Procedure for extracting users from a database by certain parameters
        /// </summary>
        public string sp_GetUsers = @"CREATE PROCEDURE [dbo].[sp_GetUsers]
                           @FirstName NVARCHAR(30) NULL,
                           @LastName NVARCHAR(30) NULL,
                           @SurName NVARCHAR(30) NULL,
                           @Date DATETIME  NULL,
                           @City NVARCHAR(30) NULL,
                           @Country NVARCHAR(30) NULL
                       AS
                       SELECT Users.Id, Users.FirstName, Users.LastName, Users.SurName, Users.Date, Cities.CityName, Countries.CountryName 
                       FROM Users
                       JOIN Cities ON Cities.Id=Users.Id
                       JOIN Countries ON Countries.Id=Users.Id
                       WHERE  (@FirstName is NULL OR Users.FirstName = @FirstName) AND (@LastName IS NULL OR Users.LastName = @LastName)  
															                       AND (@SurName IS NULL OR Users.SurName = @SurName)
															                       AND (@Date IS NULL OR Users.Date = @Date)
															                       AND (@City IS NULL OR Cities.CityName = @City)
															                       AND (@Country IS NULL OR Countries.CountryName = @Country) 
                       GO";
    }
}
