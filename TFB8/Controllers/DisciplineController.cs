namespace TFB8.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web.Http;
    using Interfaces;
    using Services;
    using TFB8.Models;

    public class DisciplineController : ApiController
    {
        IDisciplineService disciplineService = new DisciplineService();

        public DisciplineController(IDisciplineService disciplineService)
        {
            this.disciplineService = disciplineService;
        }
        // GET api/<controller>
        [HttpGet()]
        public IHttpActionResult Get()
        {
            IHttpActionResult res = null;
            List<Discipline> disciplines = new List<Discipline>();

            try
            {
                disciplines = this.disciplineService.GetAllDisciplines().ToList();
                res = Ok(disciplines);
            }
            catch (Exception e)
            {
                res = NotFound();
            }

            return res;
        }

        // GET api/<controller>/id
        [HttpGet()]
        public IHttpActionResult Get(int id)
        {
            IHttpActionResult res;
            Discipline discipline = new Discipline();
            if (discipline == null)
            {
                res = Content(HttpStatusCode.NotFound, "Missing discipline with this id");
            }
            else
            {
                try
                {
                    discipline = this.disciplineService.GetDisciplineById(id);
                    res = Ok(discipline);
                }
                catch (Exception e)
                {
                    res = BadRequest();
                }
            }

            return res;
        }


        // POST api/<controller>
        [HttpPost()]
        public IHttpActionResult Post(Discipline discipline)
        {
            IHttpActionResult res = null;

            if (string.IsNullOrEmpty(discipline.DisciplineName))
            {
                res = Content(HttpStatusCode.BadRequest, "Discipline name is requered!");
            }
            else if (string.IsNullOrEmpty(discipline.ProfessorName))
            {
                res = Content(HttpStatusCode.BadRequest, "Professor name is requered!");
            }
            else
            {
                try
                {
                    this.disciplineService.CreateDiscipline(discipline);
                    res = Content(HttpStatusCode.Created, "Successfully created discipline!");
                }
                catch (Exception e)
                {
                    res = BadRequest();
                }
            }

            return res;
        }

        [HttpPut()]
        public IHttpActionResult Put(int id, Discipline discipline)
        {
            IHttpActionResult res = null;
            if (string.IsNullOrEmpty(discipline.DisciplineName))
            {
                res = Content(HttpStatusCode.BadRequest, "Discipline name is requered!");
            }
            else if (id == 0 || discipline is null)
            {
                res = NotFound();
            }
            else
            {
                try
                {
                    this.disciplineService.UpdateDiscipline(id, discipline);
                    res = Content(HttpStatusCode.OK, "Successfully updated discipline!");
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
                    this.disciplineService.DeleteDiscipline(id);
                    res = Content(HttpStatusCode.OK, "Successfully deleted discipline!");
                }
                catch (Exception e)
                {
                    res = Content(HttpStatusCode.BadRequest, e.Message);
                }
            }
            else
            {
                res = Content(HttpStatusCode.NotFound, "You try to delete discipline that does not exist");
            }

            return res;
        }
    }
}
