using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazingShop.Shared.Models
{
    public class Products
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public byte[] Image { get; set; }  
       

        public decimal Price { get; set; }
       public decimal OrginalPrice { get; set; }
     
      
     
    }
}
