﻿using System;
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
    [Route("api/v1/orders")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    public class OrdersController : ControllerBase
    {
        private readonly IRepository _repository;
        private readonly PersistCommandsHandler _persistCommands;
        private readonly Func<DateTime> _now;

        public OrdersController (IRepository repository, PersistCommandsHandler persistCommands, Func<DateTime> now)
        {
            _repository = repository;
            _persistCommands = persistCommands;
            _now = now;
        }

        [HttpGet]
        public ActionResult<Order[]> Get()
        {
            return _repository.GetOrders().ToArray();
        }
        [HttpGet("{id}")]
        public ActionResult<Order> Get(int id)
        {
            return _repository.GetOrder(id);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Post([FromBody] AddOrder body)
        {
            var c = body.ToCommand(_now());
            var res = c.Handle(_repository);
            _persistCommands.Handle(c);
            return res ? (ActionResult) Ok() : BadRequest();
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
        public ActionResult PostProduct(int id,[FromBody] AddProductToOrder body)
        {
            var c = body.ToCommand(id);
            var res = c.Handle(_repository);
            _persistCommands.Handle(c);
            return res ? (ActionResult) Ok() : BadRequest();
        }
    }
}