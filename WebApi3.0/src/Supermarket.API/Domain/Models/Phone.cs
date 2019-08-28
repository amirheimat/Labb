using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Supermarket.API.Domain.Models
{
    public class Phone : Product
    {
        public int Memory { get; set; }
        public EBand Band { get; set; }
    }
}
