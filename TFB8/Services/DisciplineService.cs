using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TFB8.Services
{
    using Interfaces;
    using MySql.Data.MySqlClient;
    using System.Configuration;
    using TFB8.Models;

    public class DisciplineService : IDisciplineService
    {
        string connectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;

        public void CreateDiscipline(Discipline discipline)
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();

                using (MySqlCommand command = new MySqlCommand(
                    "INSERT into tfb8.discipline values (default, @DisciplineName, @ProfessorName)", con))
                {
                    command.Parameters.Add(new MySqlParameter("DisciplineName", discipline.DisciplineName));
                    command.Parameters.Add(new MySqlParameter("ProfessorName", discipline.ProfessorName));
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteDiscipline(int id)
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                // Check is discipline is assigned to semester

                using (MySqlCommand command = new MySqlCommand(
                    "select 1 from tfb8.semesterdisciplines where disciplineid = " + id, con))
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        throw new Exception("Can not delete discipline that is assigned to semester!");
                    }
                }

                using (MySqlCommand command = new MySqlCommand(
                    "DELETE from tfb8.discipline where disciplineid = @DisciplineId", con))
                {
                    command.Parameters.Add(new MySqlParameter("DisciplineId", id));
                    command.ExecuteNonQuery();
                }
            }
        }

        public IEnumerable<Discipline> GetAllDisciplines()
        {
            List<Discipline> disciplines = new List<Discipline>();

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();

                using (MySqlCommand command = new MySqlCommand("SELECT disciplineId, disciplineName, professorName FROM tfb8.discipline", con))
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var discipline = new Discipline();
                        int disciplineId = (int)reader["disciplineId"];
                        string disciplineName = reader["disciplineName"] as string;
                        string professorName = reader["professorName"] as string;
                        discipline.DisciplineId = disciplineId;
                        discipline.DisciplineName = disciplineName;
                        discipline.ProfessorName = professorName;
                        disciplines.Add(discipline);
                    }
                }

                con.Close();
            }

            return disciplines;
        }

        public Discipline GetDisciplineById(int id)
        {
            Discipline discipline = new Discipline();

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                string sqlCommand =
                    "SELECT disciplineId, disciplineName, professorName FROM tfb8.discipline where disciplineId = " +
                    id;
                using (MySqlCommand command = new MySqlCommand(sqlCommand, con))
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int disciplineId = (int)reader["disciplineId"];
                        string disciplineName = reader["disciplineName"] as string;
                        string professorName = reader["professorName"] as string;
                        discipline.DisciplineId = disciplineId;
                        discipline.DisciplineName = disciplineName;
                        discipline.ProfessorName = professorName;
                    }
                }

                con.Close();
            }

            return discipline;
        }

        public void UpdateDiscipline(int id, Discipline discipline)
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();

                using (MySqlCommand command = new MySqlCommand(
                    "UPDATE  tfb8.discipline set disciplinename =  @DisciplineName, professorName = @ProfessorName where disciplineid = @DisciplineId", con))
                {
                    command.Parameters.Add(new MySqlParameter("DisciplineName", discipline.DisciplineName));
                    command.Parameters.Add(new MySqlParameter("ProfessorName", discipline.ProfessorName));
                    command.Parameters.Add(new MySqlParameter("DisciplineId", id));
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}