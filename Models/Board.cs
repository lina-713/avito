using System.Collections.Generic;

namespace avito.Models
{
    public class Board
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Advertisement> Advertisement { get; set; }
    }
}
