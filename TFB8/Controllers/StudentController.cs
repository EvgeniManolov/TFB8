﻿using MySql.Data.MySqlClient;
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
                res = NotFound();
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
    }
}
