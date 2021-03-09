using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp01.Shared.Domain
{
    public class Person
    {
        public Guid Id { get; set; }
        public string Name {get; set;}
        public DateTime DateOfBith { get; set; }
        public string Nickname { get; set; }
        public int Weight { get; set; }

    }
}
