namespace TFB8.Services
{
    using Interfaces;
    using TFB8.Models;
    using System;
    using System.Collections.Generic;
    using MySql.Data.MySqlClient;
    using System.Configuration;

    public class SemesterService : ISemesterService
    {
        string connectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;

        public void CreateSemester(Semester semester)
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();

                using (MySqlCommand command = new MySqlCommand(
                        "INSERT into tfb8.semester values (default, @Name, @StartDate, @EndDate)", con))
                {
                    command.Parameters.Add(new MySqlParameter("Name", semester.Name));
                    command.Parameters.Add(new MySqlParameter("StartDate", semester.StartDate));
                    command.Parameters.Add(new MySqlParameter("EndDate", semester.EndDate));
                    command.ExecuteNonQuery();
                    var lastId = command.LastInsertedId;

                    foreach (Discipline discipline in semester.Disciplines)
                    {
                        using (MySqlCommand com = new MySqlCommand(
                            "INSERT into tfb8.semesterdisciplines values (@SemesterId, (Select disciplineid from tfb8.discipline where disciplineid = @DisciplineId), default)", con))
                        {
                            com.Parameters.Add(new MySqlParameter("SemesterId", lastId));
                            com.Parameters.Add(new MySqlParameter("DisciplineId", discipline.DisciplineId));
                            com.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        public void DeleteSemester(int id)
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                // Check is semester is assigned to student

                using (MySqlCommand command = new MySqlCommand(
                    "select 1 from tfb8.scores s" +
                    "           join tfb8.semesterdisciplines sd on sd.semesterdisciplinesid = s.semesterdisciplinesid" +
                    "            where  sd.semesterid = " + id, con))
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        throw new Exception("Can not delete semester that is assigned to student!");
                    }
                }

                using (MySqlCommand command = new MySqlCommand(
                    "DELETE from tfb8.semesterdisciplines where semesterid  = @SemesterId", con))
                {
                    command.Parameters.Add(new MySqlParameter("SemesterId", id));
                    command.ExecuteNonQuery();
                }

                using (MySqlCommand command = new MySqlCommand(
                    "DELETE from tfb8.semester where semesterid = @SemesterId", con))
                {
                    command.Parameters.Add(new MySqlParameter("SemesterId", id));
                    command.ExecuteNonQuery();
                }
            }
        }

        public IEnumerable<Semester> GetAllSemesters()
        {
            List<Semester> semesters = new List<Semester>();

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();

                using (MySqlCommand command = new MySqlCommand(
                    "select s.semesterid, s.name, s.startdate, s.enddate, GROUP_CONCAT(d.disciplineName) as disciplines " +
                    "from tfb8.semester as s " +
                    "left join tfb8.semesterdisciplines sd on sd.semesterid = s.semesterid " +
                    "left join tfb8.discipline d on d.disciplineid = sd.disciplineid group by s.semesterid", con))
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var semester = new Semester();
                        int semesterId = (int)reader["SemesterId"];
                        DateTime startDate = (DateTime)reader["StartDate"];
                        DateTime endDate = (DateTime)reader["EndDate"];
                        string name = (string)reader["Name"];
                        string disciplines = "";
                        if (reader["Disciplines"] != null && reader["Disciplines"] != DBNull.Value)
                        {
                            disciplines = (string)reader["Disciplines"];
                        }
                        semester.SemesterId = semesterId;
                        semester.StartDate = startDate;
                        semester.Name = name;
                        semester.EndDate = endDate;
                        semester.DisciplinesAsString = disciplines;
                        semesters.Add(semester);
                    }
                }

                con.Close();
            }

            return semesters;
        }
    }
}