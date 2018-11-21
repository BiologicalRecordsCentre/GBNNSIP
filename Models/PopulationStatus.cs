using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace GBNNSS.Models
{
    public class PopulationStatus
    {
        public int speciesid { get; set; }
        [ForeignKey("speciesid")]
        public string gb { get; set; }
        public string england { get; set; }
        public string scotland { get; set; }
        public string wales { get; set; }
    }
}