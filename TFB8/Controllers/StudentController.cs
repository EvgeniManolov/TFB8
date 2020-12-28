using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TFB8.Interfaces;
using TFB8.Models;
using TFB8.Services;

namespace TFB8.Controllers
{
    public class StudentController : ApiController
    {
        IStudentService studentService = new StudentService();

        public StudentController(IStudentService studentService)
        {
            this.studentService = studentService;
        }

        // GET api/<controller>
        [HttpGet()]
        public IHttpActionResult Get()
        {
            IHttpActionResult res = null;

            List<Student> students = new List<Student>();

            try
            {
                students = this.studentService.GetAllStudents().ToList();
                res = Ok(students);
            }
            catch (Exception e)
            {
                res = this.BadRequest(e.Message);
            }

            return res;
        }

        // POST api/<controller>
        [HttpPost()]
        public IHttpActionResult Post(Student student)
        {
            IHttpActionResult res = null;

            if (student is null)
            {
                res = BadRequest();
            }
            else if (string.IsNullOrEmpty(student.Name))
            {
                res = Content(HttpStatusCode.BadRequest, "Student name is requered!");
            }
            else if (string.IsNullOrEmpty(student.Surname))
            {
                res = Content(HttpStatusCode.BadRequest, "Student surname is requered!");
            }
            else
            {
                try
                {
                    this.studentService.CreateStudent(student);
                    res = Content(HttpStatusCode.Created, "Successfully created student!");
                }
                catch
                {
                    res = NotFound();
                }
            }

            return res;
        }

        // GET api/<controller>/id
        [HttpGet()]
        public IHttpActionResult Get(int id)
        {
            IHttpActionResult res;
            Student student = new Student();
            if (student == null)
            {
                res = Content(HttpStatusCode.NotFound, "Missing student with this id");
            }
            else
            {
                try
                {
                    student = this.studentService.GetStudentById(id);
                    res = Ok(student);
                }
                catch (Exception e)
                {
                    res = BadRequest(e.Message);
                }
            }

            return res;
        }

        [HttpPut()]
        public IHttpActionResult Put(int id, Student student)
        {
            IHttpActionResult res = null;
            if (string.IsNullOrEmpty(student.Name))
            {
                res = Content(HttpStatusCode.BadRequest, "Student name is requered!");
            }
            else if (id == 0 || student is null)
            {
                res = NotFound();
            }
            else
            {
                try
                {
                    this.studentService.UpdateStudent(id, student);
                    res = Content(HttpStatusCode.OK, "Successfully updated semester!");
                }
                catch (Exception e)
                {
                    res = BadRequest(e.Message);
                }
            }

            return res;
        }
    }
}
