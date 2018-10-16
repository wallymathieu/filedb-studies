using System.ComponentModel.DataAnnotations;
using SomeBasicFileStoreApp.Core.Commands;

namespace Web.V1.Models
{
    public class CreateCustomer
    {
        public Command ToCommand()
        {
            return new AddCustomerCommand()
            {
                Id = 0,
                Firstname = Firstname,
                Lastname =Lastname 
            };
        }
        /// <summary>
        /// 
        /// </summary>
        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string Lastname { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string Firstname { get; set; }
    }
}