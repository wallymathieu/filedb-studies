using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SomeBasicFileStoreApp.Core;
using SomeBasicFileStoreApp.Core.Commands;
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
        public ActionResult<Customer[]> Get()
        {
            return _repository.GetCustomers().ToArray();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<Customer> Get(int id)
        {
            return _repository.GetCustomer(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Post([FromBody] CreateCustomer body)
        {
            var c = body.ToCommand();
            var res = c.Handle(_repository);
            _persistCommands.Handle(c);
            return res ? (ActionResult) Ok() : BadRequest();
        }
    }
}