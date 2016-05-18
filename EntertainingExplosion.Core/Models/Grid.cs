using System.Collections.Generic;

namespace EntertainingExplosion.Core.Models
{
    public class Grid
    {
        public List<Cell> Cells { get; set; }

        public Cell ExternalCell { get; set; }
    }
}
