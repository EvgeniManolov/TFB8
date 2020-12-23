namespace TFB8.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http;
    using TFB8.Models;

    public class DisciplineController : ApiController
    {
        [HttpGet()]
        public IHttpActionResult Get()
        {
            IHttpActionResult result = null;
            List<Discipline> list = new List<Discipline>();

            list = CreateMockData();
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

        private List<Discipline> CreateMockData()
        {
            List<Discipline> result = new List<Discipline>();

            result.Add(new Discipline()
            {
                DisciplineId = 1,
                DisciplineName = "Economy of trate",
                ProfessorName = "prof. J.Smith",
            });

            result.Add(new Discipline()
            {
                DisciplineId = 2,
                DisciplineName = "Mathematics",
                ProfessorName = "prof. Y.Yordanov",
            });

            return result;
        }
    }
}
