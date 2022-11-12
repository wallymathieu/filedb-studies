using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SomeBasicFileStoreApp.Core;
using SomeBasicFileStoreApp.Core.Commands;
using Swashbuckle.AspNetCore.Filters;
using Web.V1.Models;

namespace Web.V1.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/v1/customers")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    public class CustomersController : ControllerBase
    {
        private readonly IRepository _repository;
        private readonly PersistCommandsHandler _persistCommands;

        /// <inheritdoc />
        public CustomersController(IRepository repository, PersistCommandsHandler persistCommands)
        {
            _repository = repository;
            _persistCommands = persistCommands;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<CustomerModel[]> Get() => 
            _repository.GetCustomers().Select(CustomerModel.Map).ToArray();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(CustomerModel),200)]
        public ActionResult<CustomerModel> Get(int id) => 
            _repository.TryGetCustomer(id, out var c)
                ?new ActionResult<CustomerModel>(CustomerModel.Map(c))
                :NotFound();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [SwaggerResponseExample(400, typeof(BadRequestExample))]
        public ActionResult Post([FromBody] CreateCustomer body)
        {
            var c = body.ToCommand();
            var res = c.Run(_repository);
            _persistCommands.Append(c);
            return res
                ? (ActionResult) Ok()
                : BadRequest(new Dictionary<string, string>());
        }

        public class BadRequestExample : IExamplesProvider<Dictionary<string,string>>
        {
            public Dictionary<string,string> GetExamples()
            {
                return new Dictionary<string, string>
                {
                    {"lastname", "The Lastname field is required."},
                    {"firstname", "The Firstname field is required."}
                };
            }
        }
    }
}