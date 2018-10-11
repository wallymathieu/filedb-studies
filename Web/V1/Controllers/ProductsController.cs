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
    [Route("api/v1/products")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    public class ProductsController : ControllerBase
    {
        private readonly IRepository _repository;
        private readonly PersistCommandsHandler _persistCommands;
        
        public ProductsController (IRepository repository, PersistCommandsHandler persistCommands)
        {
            _repository = repository;
            _persistCommands = persistCommands;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<Product[]> Get()
        {
            return _repository.GetProducts().ToArray();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("{id}")]
        public ActionResult<Product> Get(int id)
        {
            return _repository.GetProduct(id);
        }
        /// <summary>
        /// Add product to available products
        /// </summary>
        /// <remarks>
        /// You could for instance add products using
        /// ```JSON
        /// {
        ///   "id":"12", "cost":124, "name": "tea"
        /// }
        /// ```
        /// </remarks>
        /// <param name="body"></param>
        /// <response code="200">The product was added successfully</response>
        /// <response code="400"></response>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult Post([FromBody] AddProduct body)
        {
            var c = body.ToCommand();
            var res = c.Handle(_repository);
            _persistCommands.Handle(c);
            return res ? (ActionResult) Ok() : BadRequest();
        }
    }
}