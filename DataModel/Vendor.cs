using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class Vendor
    {
        public int Id { get; set; }
        public string VendorName { get; set; }
        public string VendorCode { get; set; }
        public bool IsActive { get; set; }
    }
}
