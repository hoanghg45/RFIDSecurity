using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFIDSecurity
{
    public class Tag
    {
        public List<unpaids> unpaids { get; set; }
    }

    public class unpaids
    {
        public string IdEPC { get; set; }
    }


}
