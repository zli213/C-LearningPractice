using System;
using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using Dapper;
using HelloWorld.Models;
using HelloWorld.Data;
using System.Globalization;
using Microsoft.Extensions.Configuration;

namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            // string connectionString = "Server=localhost;Database=DotNetCourseDatabase;Trusted_Connection=False;TrustServerCertificate=True;User Id=sa;Password=YourStrong@Passw0rd;";
            // IDbConnection dbConnection = new SqlConnection(connectionString);
            DataContextDapper dapper = new DataContextDapper();
            DataContextEF ef = new DataContextEF();
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
            ef.Add(myComputer);
            ef.SaveChanges();
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
        string sqlSelect = @"
        SELECT 
            Computer.ComputerId,
            Computer.Motherboard,
            Computer.HasWifi,
            Computer.HasLTE,
            Computer.ReleaseDate,
            Computer.Price,
            Computer.VideoCard
            FROM TutorialAppSchema.Computer";
        IEnumerable<Computer> computers = dapper.LoadData<Computer>(sqlSelect);

        Console.WriteLine("'ComputerId','Motherboard','HasWifi','HasLTE','ReleaseDate'" 
            + ",'Price','VideoCard'");
        foreach(Computer singleComputer in computers)
        {
            Console.WriteLine("'" + singleComputer.ComputerId 
                + "','" + singleComputer.Motherboard
                + "','" + singleComputer.HasWifi
                + "','" + singleComputer.HasLTE
                + "','" + singleComputer.ReleaseDate.ToString("yyyy-MM-dd")
                + "','" + singleComputer.Price.ToString("0.00", CultureInfo.InvariantCulture)
                + "','" + singleComputer.VideoCard + "'");
        }
        IEnumerable<Computer>? computersEF = ef.Computer?.ToList<Computer>();
        if (computersEF != null)
        {
            Console.WriteLine("'ComputerId', 'Motherboard', HasWifi, HasLTE, ReleaseDate, Price, 'VideoCard'");
            foreach (Computer computer in computersEF)
            {
                Console.WriteLine("'"+ computer.ComputerId + "', " + computer.Motherboard + "', " + computer.HasWifi + ", " + computer.HasLTE + ", " + computer.ReleaseDate + ", " + computer.Price + ", " + computer.VideoCard + "'");
            }
        }

    }
}
}
