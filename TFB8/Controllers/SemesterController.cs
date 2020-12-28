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
    public class SemesterController : ApiController
    {
        string connectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;

        ISemesterService semesterService = new SemesterService();

        public SemesterController(ISemesterService semesterService)
        {
            this.semesterService = semesterService;
        }

        // GET api/<controller>
        [HttpGet()]
        public IHttpActionResult Get()
        {
            IHttpActionResult res = null;
            List<Semester> semesters = new List<Semester>();

            try
            {
                semesters = this.semesterService.GetAllSemesters().ToList();
                res = Ok(semesters);
            }
            catch (Exception e)
            {
                res = NotFound();
            }

            return res;
        }


        // POST api/<controller>
        [HttpPost()]
        public IHttpActionResult Post(Semester semester)
        {
            IHttpActionResult res = null;

            if (semester is null)
            {
                res = BadRequest();
            }
            else if (string.IsNullOrEmpty(semester.Name))
            {
                res = Content(HttpStatusCode.BadRequest, "Semester name is requered!");
            }
            else
            {
                try
                {
                    this.semesterService.CreateSemester(semester);
                    res = Content(HttpStatusCode.Created, "Successfully created semester!");
                }
                catch
                {
                    res = NotFound();
                }
            }

            return res;
        }

        [HttpPut()]
        public IHttpActionResult Put(int id, Semester semester)
        {
            IHttpActionResult res = null;
            if (string.IsNullOrEmpty(semester.Name))
            {
                res = Content(HttpStatusCode.BadRequest, "Semester name is requered!");
            }
            else if (id == 0 || semester is null)
            {
                res = NotFound();
            }
            else
            {
                try
                {
                    this.semesterService.UpdateSemester(id, semester);
                    res = Content(HttpStatusCode.OK, "Successfully updated semester!");
                }
                catch (Exception e)
                {
                    res = BadRequest();
                }
            }

            return res;
        }

        // DELETE api/<controller>/5
        [HttpDelete()]
        public IHttpActionResult Delete(int id)
        {
            IHttpActionResult res = null;

            if (id != 0)
            {
                try
                {
                    this.semesterService.DeleteSemester(id);
                    res = Content(HttpStatusCode.OK, "Successfully deleted semester!");
                }
                catch (Exception e)
                {
                    res = Content(HttpStatusCode.BadRequest, e.Message);
                }
            }
            else
            {
                res = Content(HttpStatusCode.NotFound, "You try to delete semester that does not exist");
            }

            return res;
        }

        // GET api/<controller>/id
        [HttpGet()]
        public IHttpActionResult Get(int id)
        {
            IHttpActionResult res;
            Semester semester = new Semester();
            if (semester == null)
            {
                res = Content(HttpStatusCode.NotFound, "Missing semester with this id");
            }
            else
            {
                try
                {
                    semester = this.semesterService.GetSemesterById(id);
                    res = Ok(semester);
                }
                catch (Exception e)
                {
                    res = BadRequest();
                }
            }

            return res;
        }
    }
}
