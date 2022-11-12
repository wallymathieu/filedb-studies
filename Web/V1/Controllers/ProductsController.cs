using System.Collections;
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
    [Route("api/v1/products")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    public class ProductsController : ControllerBase
    {
        private readonly IRepository _repository;
        private readonly PersistCommandsHandler _persistCommands;

        public ProductsController(IRepository repository, PersistCommandsHandler persistCommands)
        {
            _repository = repository;
            _persistCommands = persistCommands;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<ProductModel[]> Get() => 
            _repository.GetProducts().Select(ProductModel.Map).ToArray();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("{id}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(ProductModel),200)]
        public ActionResult<ProductModel> Get(int id) => 
            _repository.TryGetProduct(id, out var p)
                ?new ActionResult<ProductModel>(ProductModel.Map(p))
                :NotFound();

        /// <summary>
        /// Add product to available products
        /// </summary>
        /// <remarks>
        /// You could for instance add products using
        /// ```JSON
        /// {
        ///   "cost":124, "name": "tea"
        /// }
        /// ```
        /// </remarks>
        /// <param name="body"></param>
        /// <response code="200">The product was added successfully</response>
        /// <response code="400"></response>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [SwaggerResponseExample(400, typeof(BadRequestExample))]
        public ActionResult Post([FromBody] AddProduct body)
        {
            var c = body.ToCommand();
            var res = c.Run(_repository);
            _persistCommands.Append(c);
            return res
                ? (ActionResult) Ok()
                : BadRequest(new Dictionary<string, string>());
        }

        public class BadRequestExample : IExamplesProvider<Dictionary<string, string>>
        {
            public Dictionary<string, string> GetExamples()
            {
                return new Dictionary<string, string>
                {
                    {"cost", "The field Cost must be between 0.1 and 1000000."},
                    {"name", "The Name field is required."}
                };
            }
        }
    }
}