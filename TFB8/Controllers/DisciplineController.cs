namespace TFB8.Controllers
{
    using MySql.Data.MySqlClient;
    using System.Collections.Generic;
    using System.Web.Http;
    using TFB8.Models;

    public class DisciplineController : ApiController
    {
        string connectionString =
            @"server=localhost;userid=emanolov;password=test;database=tfb8";

        // GET api/<controller>
        [HttpGet()]
        public IHttpActionResult Get()
        {
            IHttpActionResult result = null;
            List<Discipline> list = new List<Discipline>();

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
                        list.Add(discipline);
                    }
                }

                con.Close();
            }


            if (list.Count > 0)
            {
                result = Ok(list);
            }
            else
            {
                result = NotFound();
            }

            return result;
        }

        // GET api/<controller>/id
        [HttpGet()]
        public IHttpActionResult Get(int id)
        {
            IHttpActionResult result;
            List<Discipline> list = new List<Discipline>();
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

            if (discipline == null)
            {
                result = NotFound();
            }
            else
            {
                result = Ok(discipline);
            }

            return result;
        }
    }
}
