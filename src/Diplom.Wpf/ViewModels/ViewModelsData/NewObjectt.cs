using Diplom.Data.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom.ViewModels.ViewModelsData
{
    public class NewObjectt
    {
      
            public string? Name { get; set; }
            public int? IdMaterial { get; set; }
            public string? Temperature { get; set; }
            public string? Radius { get; set; }
            public string? Height { get; set; }
            public string? DistanceFromSurface { get; set; }
            public string? WallThickness { get; set; } // толщина стенки
            public string? Clutter { get; set; }
            public Fabric? fabric { get; set; }

        }
    
}
