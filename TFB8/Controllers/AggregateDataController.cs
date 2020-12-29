namespace TFB8.Controllers
{
    using System;
    using System.Web.Http;
    using TFB8.Interfaces;
    using TFB8.Models;
    using TFB8.Services;

    public class AggregateDataController : ApiController
    {
        IAggregateDataService aggregateDataService = new AggregateDataService();

        public AggregateDataController(IAggregateDataService aggregateDataService)
        {
            this.aggregateDataService = aggregateDataService;
        }

        [HttpGet()]
        public IHttpActionResult Get()
        {
            IHttpActionResult res = null;
           AggregateData aggregateData = new AggregateData();

            try
            {
                aggregateData = this.aggregateDataService.GetAllAggregateData();
                res = Ok(aggregateData);
            }
            catch (Exception e)
            {
                res = NotFound();
            }

            return res;
        }
    }
}
