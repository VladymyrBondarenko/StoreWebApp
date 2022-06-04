using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class OrderModel
    {
        public int OrderId { get; set; }

        public UserModel User { get; set; }

        public ProductModel Product { get; set; }

        public bool IsApproved { get; set; }
    }
}
