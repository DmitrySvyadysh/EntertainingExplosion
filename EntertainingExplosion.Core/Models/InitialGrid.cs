using System.Collections.Generic;

namespace EntertainingExplosion.Core.Models
{
    public class InitialGrid
    {
        public List<InitialCell> Cells { get; set; }

        public InitialCell ExternalCell { get; set; }
    }
}
