using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Producto
    {
        public int Id { get; set; }

        public string Code { get; set; }
        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }  
      
    }
}
