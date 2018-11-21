using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GBNNSS.Models
{
    public class SpeciesNames
    {
        public int speciesid {get; set; }
        public string scientific { get; set; }
        public List<string> latinnames2 { get; set; }
        public List<string> englishnames { get; set; }
    }

    public class Names
    {
        public List<SpeciesNames> Species;
    }
}