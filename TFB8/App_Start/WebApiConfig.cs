namespace TFB8
{
    using System.Configuration;
    using System.Web.Http;
    using Interfaces;
    using MySql.Data.MySqlClient;
    using Services;
    using Unity;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var container = new Unity.UnityContainer();
            container.RegisterType(typeof(IDisciplineService), typeof(DisciplineService));
            container.RegisterType(typeof(ISemesterService), typeof(SemesterService));
            container.RegisterType(typeof(IStudentService), typeof(StudentService));
            container.RegisterType(typeof(IScoreService), typeof(ScoreService));
            container.RegisterType(typeof(IAggregateDataService), typeof(AggregateDataService));
            config.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);
            SeedDatabase();
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

        private static void SeedDatabase()
        {
            var connectionString = "server=localhost;userid=emanolov;password=Evgeni+9004210547;";
            bool isDatabaseCreated = false;

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();

                using (MySqlCommand command = new MySqlCommand("show DATABASES LIKE 'tfb8'", con))
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader.HasRows)
                        {
                            isDatabaseCreated = true;
                        }
                    }
                }

                if (!isDatabaseCreated)
                {

                    // CreateDatabase
                    using (MySqlCommand command = new MySqlCommand(
                        "CREATE DATABASE tfb8", con))
                    {
                        command.ExecuteNonQuery();

                        //Create Tables
                        command.CommandText = "CREATE TABLE `tfb8`.`discipline` ( "
                                              + " `disciplineId` int NOT NULL AUTO_INCREMENT,"
                                              + " `disciplinename` varchar(250) DEFAULT NULL,"
                                              + " `professorname` varchar(250) DEFAULT NULL,"
                                              + " PRIMARY KEY(`disciplineId`)"
                                              + " ) ENGINE = InnoDB AUTO_INCREMENT = 1 DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci; ";
                        command.ExecuteNonQuery();

                        command.CommandText = "CREATE TABLE `tfb8`.`semester` ("
                                              + " `semesterid` int NOT NULL AUTO_INCREMENT,"
                                              + " `name` varchar(250) DEFAULT NULL,"
                                              + " `startdate` datetime DEFAULT NULL,"
                                              + " `enddate` datetime DEFAULT NULL,"
                                              + " PRIMARY KEY(`semesterid`)"
                                              + " ) ENGINE = InnoDB AUTO_INCREMENT = 1 DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci; ";
                        command.ExecuteNonQuery();

                        command.CommandText = "CREATE TABLE `tfb8`.`semesterdisciplines` ("
                                              + " `semesterid` int NOT NULL,"
                                              + " `disciplineid` int NOT NULL,"
                                              + " `semesterdisciplinesid` int NOT NULL AUTO_INCREMENT,"
                                              + " PRIMARY KEY(`semesterdisciplinesid`),"
                                              + " KEY `disciplineid_idx` (`disciplineid`),"
                                              + " KEY `semester_idx` (`semesterid`),"
                                              + " CONSTRAINT `discipline` FOREIGN KEY(`disciplineid`) REFERENCES `discipline` (`disciplineId`),"
                                              + " CONSTRAINT `semester` FOREIGN KEY(`semesterid`) REFERENCES `semester` (`semesterid`)"
                                              + " ) ENGINE = InnoDB AUTO_INCREMENT = 1 DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci; ";
                        command.ExecuteNonQuery();

                        command.CommandText = "CREATE TABLE `tfb8`.`student` ("
                                              + " `studentid` int NOT NULL AUTO_INCREMENT,"
                                              + " `name` varchar(50) NOT NULL,"
                                              + " `surname` varchar(50) NOT NULL,"
                                              + " `dateofbirth` datetime NOT NULL,"
                                              + " PRIMARY KEY(`studentid`)"
                                              + " ) ENGINE = InnoDB AUTO_INCREMENT = 1 DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci; ";
                        command.ExecuteNonQuery();

                        command.CommandText = "CREATE TABLE `tfb8`.`scores` ("
                                              + " `studentid` int NOT NULL,"
                                              + " `semesterdisciplinesid` int NOT NULL,"
                                              + " `id` int NOT NULL AUTO_INCREMENT,"
                                              + " `score` int DEFAULT NULL,"
                                              + " PRIMARY KEY(`id`),"
                                              + " KEY `studentid_idx` (`studentid`),"
                                              + " KEY `semesterdisciplines_idx` (`semesterdisciplinesid`),"
                                              + " CONSTRAINT `semesterdisciplines` FOREIGN KEY(`semesterdisciplinesid`) REFERENCES `semesterdisciplines` (`semesterdisciplinesid`),"
                                              + " CONSTRAINT `studentid` FOREIGN KEY(`studentid`) REFERENCES `student` (`studentid`)"
                                              + " ) ENGINE = InnoDB AUTO_INCREMENT = 1 DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci; ";
                        command.ExecuteNonQuery();

                        command.CommandText = "INSERT INTO `tfb8`.`discipline` " +
                                              " values(default, 'Discipline 1.1', 'Professor 1.1')," +
                                              " (default, 'Discipline 1.2', 'Professor 1.2')," +
                                              " (default, 'Discipline 1.3', 'Professor 1.3')," +
                                              " (default, 'Discipline 2.1', 'Professor 2.1')," +
                                              " (default, 'Discipline 2.2', 'Professor 2.2')," +
                                              " (default, 'Discipline 2.3', 'Professor 2.3'); ";
                        command.ExecuteNonQuery();

                        command.CommandText = "INSERT INTO `tfb8`.`semester` " +
                                              " values(default, 'Semester 1', '2020-08-08', '2020-09-09')," +
                                              " (default, 'Semester 2', '2020-10-10', '2020-11-11') ";
                        command.ExecuteNonQuery();

                        command.CommandText = "INSERT INTO `tfb8`.`semesterdisciplines` " +
                                              " values(1, 1, default)," +
                                              " (1, 2, default)," +
                                              " (1, 3, default)," +
                                              " (2, 4, default)," +
                                              " (2, 5, default)," +
                                              " (2, 6, default)";
                        command.ExecuteNonQuery();


                        command.CommandText = "INSERT INTO `tfb8`.`student` " +
                                              " values(default, 'Student 1', 'Surname 1', '2020-09-09')," +
                                              " (default, 'Student 2', 'Surname 2', '2020-09-09')," +
                                              " (default, 'Student 3', 'Surname 3', '2020-03-09')," +
                                              " (default, 'Student 4', 'Surname 4', '2020-09-07')," +
                                              " (default, 'Student 5', 'Surname 5', '2020-09-09')," +
                                              " (default, 'Student 6', 'Surname 6', '2020-09-05')," +
                                              " (default, 'Student 7', 'Surname 7', '2020-04-09')," +
                                              " (default, 'Student 8', 'Surname 8', '2020-09-09')," +
                                              " (default, 'Student 9', 'Surname 9', '2020-11-11') ";
                        command.ExecuteNonQuery();

                        command.CommandText = "INSERT INTO `tfb8`.`scores` " +
                                              " values(1, 1, default, 5)," +
                                              " (1, 2, default, 6)," +
                                              " (1, 3, default, 5)," +
                                              " (2, 1, default, 4)," +
                                              " (2, 2, default, 5)," +
                                              " (2, 3, default, 5)," +
                                              " (3, 1, default, 4)," +
                                              " (3, 2, default, 4)," +
                                              " (3, 3, default, 4)," +
                                              " (4, 1, default, 3)," +
                                              " (4, 2, default, null)," +
                                              " (4, 3, default, 6)," +
                                              " (5, 4, default, 3)," +
                                              " (5, 5, default, null)," +
                                              " (5, 6, default, 6)," +
                                              " (6, 4, default, 6)," +
                                              " (6, 5, default, 6)," +
                                              " (6, 6, default, 6)," +
                                              " (7, 4, default, 6)," +
                                              " (7, 5, default, 3)," +
                                              " (7, 6, default, 5)," +
                                              " (8, 4, default, 2)," +
                                              " (8, 5, default, 3)," +
                                              " (8, 6, default, 5)," +
                                              " (9, 4, default, null)," +
                                              " (9, 5, default, null)," +
                                              " (9, 6, default, 5) ";
                        command.ExecuteNonQuery();
                    }
                }

                con.Close();
            }
        }
    }
}
