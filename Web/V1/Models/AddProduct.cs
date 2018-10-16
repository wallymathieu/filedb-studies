using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SomeBasicFileStoreApp.Core.Commands;

namespace Web.V1.Models
{
    ///
    public class AddProduct
    {
        /// 
        public Command ToCommand()
        {
            return new AddProductCommand
            {
                Id = 0,
                Name= Name,
                Cost=Cost
            };
        }

        /// <summary>
        /// 
        /// </summary>
        [Range(0.1, 1_000_000.0)]
        [DataType(DataType.Currency)]
        [Required]
        public float Cost { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string Name { get; set; }

    }
}