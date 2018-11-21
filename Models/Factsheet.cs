using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GBNNSS.Models
{
    public class Factsheet
    {
        public int fact_id { get; set; }
        public string short_desc { get; set; }
        public string biology_ecology { get; set; }
        public string habitat { get; set; }
        public string distribution { get; set; }
        public string impact { get; set; }
        public string management { get; set; }
        public string other_references { get; set; }
        public string authors { get; set; }
        public string last_updated { get; set; }
        public string status_summary { get; set; }
        public string impact_summary { get; set; }
        public string habitat_summary { get; set; }
        public string invasion_history { get; set; }
        public string references { get; set; }
    }
}