using System;
using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using Dapper;
using HelloWorld.Models;
using HelloWorld.Data;

namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            // string connectionString = "Server=localhost;Database=DotNetCourseDatabase;Trusted_Connection=False;TrustServerCertificate=True;User Id=sa;Password=YourStrong@Passw0rd;";
            // IDbConnection dbConnection = new SqlConnection(connectionString);
            DataContextDapper dapper = new DataContextDapper();
            DateTime dateTime = dapper.LoadDataSingle<DateTime>("SELECT GETDATE()");
            Console.WriteLine(dateTime);
            Computer myComputer = new Computer()
            {
                Motherboard = "Z690" ,
                HasWifi = true,
                HasLTE = false,
                ReleaseDate = DateTime.Now,
                Price = 943.87m,
                VideoCard = "RTX 2060",
            
            };
       string sql = @"INSERT INTO TutorialAppSchema.Computer (
                Motherboard,
                HasWifi,
                HasLTE,
                ReleaseDate,
                Price,
                VideoCard
            ) VALUES ('" + myComputer.Motherboard 
                    + "','" + myComputer.HasWifi
                    + "','" + myComputer.HasLTE
                    + "','" + myComputer.ReleaseDate.ToString("yyyy-MM-dd")
                    + "','" + myComputer.Price
                    + "','" + myComputer.VideoCard
            + "')";

        // int result = dapper.ExecuteSqlWithRowCount(sql);
        bool result = dapper.ExecuteSql(sql);

        Console.WriteLine(sql);
        Console.WriteLine(result);
        string sqlSelect = @"SELECT * FROM TutorialAppSchema.Computer";

        IEnumerable<Computer> computers = dapper.LoadData<Computer>(sqlSelect);
        List<Computer> computerList = computers.ToList(); 
        foreach (Computer computer in computerList)
        {
            Console.WriteLine("'" + computer.Motherboard + "', " + computer.HasWifi + ", " + computer.HasLTE + ", " + computer.ReleaseDate + ", " + computer.Price + ", " + computer.VideoCard + "'");
        }
    }
}
}
