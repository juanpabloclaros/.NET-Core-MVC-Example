﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProyectoWeb.Data;

namespace ProyectoWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<SchoolContext>();
                    DbInitializer.Initialize(context);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}

//using System;
//using System.Data.SqlClient;   // System.Data.dll
////using System.Data;           // For:  SqlDbType , ParameterDirection

//namespace ProyectoWeb
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {

//            try
//            {
//                var cb = new SqlConnectionStringBuilder();
//                cb.DataSource = "proyectowebserver.database.windows.net";
//                cb.UserID = "juanpabloclaros";
//                cb.Password = "TJqHbMf8";
//                cb.InitialCatalog = "Proyectoweb1";

//                using (var connection = new SqlConnection(cb.ConnectionString))
//                {
//                    connection.Open();

//                    Submit_Tsql_NonQuery(connection, "2 - Create-Tables", Build_2_Tsql_CreateTables());

//                    Submit_Tsql_NonQuery(connection, "3 - Inserts", Build_3_Tsql_Inserts());

//                    Submit_Tsql_NonQuery(connection, "4 - Update-Join", Build_4_Tsql_UpdateJoin(),
//                        "@csharpParmDepartmentName", "Accounting");

//                    Submit_Tsql_NonQuery(connection, "5 - Delete-Join", Build_5_Tsql_DeleteJoin(),
//                        "@csharpParmDepartmentName", "Legal");

//                    Submit_6_Tsql_SelectEmployees(connection);
//                }
//            }
//            catch (SqlException e)
//            {
//                Console.WriteLine(e.ToString());
//            }

//            Console.WriteLine("View the report output here, then press any key to end the program...");
//            Console.ReadKey();
//        }

//        static string Build_2_Tsql_CreateTables()
//        {
//            return @"
//        DROP TABLE IF EXISTS tabEmployee;
//        DROP TABLE IF EXISTS tabDepartment;  -- Drop parent table last.

//        CREATE TABLE tabDepartment
//        (
//            DepartmentCode  nchar(4)          not null    PRIMARY KEY,
//            DepartmentName  nvarchar(128)     not null
//        );

//        CREATE TABLE tabEmployee
//        (
//            EmployeeGuid    uniqueIdentifier  not null  default NewId()    PRIMARY KEY,
//            EmployeeName    nvarchar(128)     not null,
//            EmployeeLevel   int               not null,
//            DepartmentCode  nchar(4)              null
//            REFERENCES tabDepartment (DepartmentCode)  -- (REFERENCES would be disallowed on temporary tables.)
//        );
//    ";
//        }

//        static string Build_3_Tsql_Inserts()
//        {
//            return @"
//        -- The company has these departments.
//        INSERT INTO tabDepartment (DepartmentCode, DepartmentName)
//        VALUES
//            ('acct', 'Accounting'),
//            ('hres', 'Human Resources'),
//            ('legl', 'Legal');

//        -- The company has these employees, each in one department.
//        INSERT INTO tabEmployee (EmployeeName, EmployeeLevel, DepartmentCode)
//        VALUES
//            ('Alison'  , 19, 'acct'),
//            ('Barbara' , 17, 'hres'),
//            ('Carol'   , 21, 'acct'),
//            ('Deborah' , 24, 'legl'),
//            ('Elle'    , 15, null);
//    ";
//        }

//        static string Build_4_Tsql_UpdateJoin()
//        {
//            return @"
//        DECLARE @DName1  nvarchar(128) = @csharpParmDepartmentName;  --'Accounting';

//        -- Promote everyone in one department (see @parm...).
//        UPDATE empl
//        SET
//            empl.EmployeeLevel += 1
//        FROM
//            tabEmployee   as empl
//        INNER JOIN
//            tabDepartment as dept ON dept.DepartmentCode = empl.DepartmentCode
//        WHERE
//            dept.DepartmentName = @DName1;
//    ";
//        }

//        static string Build_5_Tsql_DeleteJoin()
//        {
//            return @"
//        DECLARE @DName2  nvarchar(128);
//        SET @DName2 = @csharpParmDepartmentName;  --'Legal';

//        -- Right size the Legal department.
//        DELETE empl
//        FROM
//            tabEmployee   as empl
//        INNER JOIN
//            tabDepartment as dept ON dept.DepartmentCode = empl.DepartmentCode
//        WHERE
//            dept.DepartmentName = @DName2

//        -- Disband the Legal department.
//        DELETE tabDepartment
//            WHERE DepartmentName = @DName2;
//    ";
//        }

//        static string Build_6_Tsql_SelectEmployees()
//        {
//            return @"
//        -- Look at all the final Employees.
//        SELECT
//            empl.EmployeeGuid,
//            empl.EmployeeName,
//            empl.EmployeeLevel,
//            empl.DepartmentCode,
//            dept.DepartmentName
//        FROM
//            tabEmployee   as empl
//        LEFT OUTER JOIN
//            tabDepartment as dept ON dept.DepartmentCode = empl.DepartmentCode
//        ORDER BY
//            EmployeeName;
//    ";
//        }
//        static void Submit_6_Tsql_SelectEmployees(SqlConnection connection)
//        {
//            Console.WriteLine();
//            Console.WriteLine("=================================");
//            Console.WriteLine("Now, SelectEmployees (6)...");

//            string tsql = Build_6_Tsql_SelectEmployees();

//            using (var command = new SqlCommand(tsql, connection))
//            {
//                using (SqlDataReader reader = command.ExecuteReader())
//                {
//                    while (reader.Read())
//                    {
//                        Console.WriteLine("{0} , {1} , {2} , {3} , {4}",
//                            reader.GetGuid(0),
//                            reader.GetString(1),
//                            reader.GetInt32(2),
//                            (reader.IsDBNull(3)) ? "NULL" : reader.GetString(3),
//                            (reader.IsDBNull(4)) ? "NULL" : reader.GetString(4));
//                    }
//                }
//            }
//        }

//        static void Submit_Tsql_NonQuery(
//            SqlConnection connection,
//            string tsqlPurpose,
//            string tsqlSourceCode,
//            string parameterName = null,
//            string parameterValue = null
//            )
//        {
//            Console.WriteLine();
//            Console.WriteLine("=================================");
//            Console.WriteLine("T-SQL to {0}...", tsqlPurpose);

//            using (var command = new SqlCommand(tsqlSourceCode, connection))
//            {
//                if (parameterName != null)
//                {
//                    command.Parameters.AddWithValue(  // Or, use SqlParameter class.
//                        parameterName,
//                        parameterValue);
//                }
//                int rowsAffected = command.ExecuteNonQuery();
//                Console.WriteLine(rowsAffected + " = rows affected.");
//            }
//        }
//    } // EndOfClass
//}
