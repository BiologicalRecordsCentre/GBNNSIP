using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace GBNNSS.Models
{
    public class NativeRanges
    {
        public string region;         
    }
    public class Natives
    {
        public int objIds { get; set; }
        public int regionIds { get; set; }
    }
}