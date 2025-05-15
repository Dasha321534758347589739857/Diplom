using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom.Data.DB
{
    public class Role
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}
