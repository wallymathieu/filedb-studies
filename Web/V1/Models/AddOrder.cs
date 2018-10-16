using System;
using System.ComponentModel.DataAnnotations;
using SomeBasicFileStoreApp.Core.Commands;

namespace Web.V1.Models
{
    public class AddOrder
    {
        public Command ToCommand(DateTime now)
        {
            return new AddOrderCommand()
            {
                Id = 0,
                Customer=Customer,
                OrderDate = now
            };
        }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public int Customer { get; set; }
    }
}