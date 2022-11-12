using System;
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
    [Route("api/v1/orders")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    public class OrdersController : ControllerBase
    {
        private readonly IRepository _repository;
        private readonly PersistCommandsHandler _persistCommands;
        private readonly Func<DateTime> _now;

        public OrdersController(IRepository repository, PersistCommandsHandler persistCommands, Func<DateTime> now)
        {
            _repository = repository;
            _persistCommands = persistCommands;
            _now = now;
        }

        [HttpGet]
        public ActionResult<OrderModel[]> Get() => 
            _repository.GetOrders().Select(OrderModel.Map).ToArray();

        [HttpGet("{id}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(OrderModel),200)]
        public ActionResult<OrderModel> Get(int id) => 
            _repository.TryGetOrder(id, out var o)
                ? new ActionResult<OrderModel>(OrderModel.Map(o))
                : NotFound();

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [SwaggerResponseExample(400, typeof(AddOrderBadRequestExample))]
        public ActionResult Post([FromBody] AddOrder body)
        {
            var c = body.ToCommand(_now());
            var res = c.Run(_repository);
            _persistCommands.Append(c);
            return res
                ? (ActionResult) Ok()
                : BadRequest(new Dictionary<string, string>());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">order id</param>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost("{id}/products")]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [SwaggerResponseExample(400, typeof(AddProductToOrderBadRequestExample))]
        public ActionResult PostProduct(int id, [FromBody] AddProductToOrder body)
        {
            var c = body.ToCommand(id);
            var res = c.Run(_repository);
            _persistCommands.Append(c);
            return res ? (ActionResult) Ok() : BadRequest(new Dictionary<string, string>());
        }

        ///
        public class AddProductToOrderBadRequestExample : IExamplesProvider<Dictionary<string,string>>
        {
            /// <inheritdoc />
            public Dictionary<string, string> GetExamples()
            {
                return new Dictionary<string, string>
                {
                    {"productId", "The ProductId field is required."},
                };
            }
        }

        ///
        public class AddOrderBadRequestExample : IExamplesProvider<Dictionary<string,string>>
        {
            /// <inheritdoc />
            public Dictionary<string, string> GetExamples()
            {
                return new Dictionary<string, string>
                {
                    {"customer", "The Customer field is required."},
                };
            }
        }
    }
}