using Supermarket.API.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Supermarket.API.Resources
{
    public class SaveProductResource
    {
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public short QuantityInPackage { get; set; }
        public EUnitOfMeasurement UnitOfMeasurement { get; set; }
    }
}
