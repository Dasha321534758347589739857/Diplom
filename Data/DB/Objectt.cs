using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom.Data.DB
{
    public class Objectt
    {
        public int Id { get; set; }

        public int? IdMaterial { get; set; }

        public string? Name { get; set; }

        public string? Temperature { get; set; }

        public string? Radius { get; set; } // ширина

        public string? Height { get; set; } // высота

        public string? DistanceFromSurface { get; set; } // расстояние от поверхности

        public string? WallThickness { get; set; } // толщина стенки

        public string? Clutter { get; set; } // загромождённость

        public Fabric? Fabric { get; set; }
    }
}
