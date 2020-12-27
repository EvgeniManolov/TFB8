using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TFB8.Models;

namespace TFB8.Controllers
{
    public class StudentController : ApiController
    {
        string connectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;


        // GET api/<controller>
        [HttpGet()]
        public IHttpActionResult Get()
        {
            IHttpActionResult result = null;
            List<Student> list = new List<Student>();

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();

                using (MySqlCommand command = new MySqlCommand("SELECT studentId, name, surname, dateofbirth FROM tfb8.student", con))
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var student = new Student();
                        int studentId = (int)reader["studentId"];
                        string name = reader["name"] as string;
                        string surname = reader["surname"] as string;
                        DateTime dateofbirth = (DateTime)reader["dateofbirth"];
                        student.StudentId = studentId;
                        student.Name = name;
                        student.Surname = surname;
                        student.DateOfBirth = dateofbirth;
                        list.Add(student);
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
    }
}
