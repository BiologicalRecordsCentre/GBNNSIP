using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GBNNSS.Models
{
    public class Species
    {
        public int speciesid { get; set; }
        public string latinname { get; set; }
        public string nbntvk { get; set; }
        public string brcnumber { get; set; }
        public string authority { get; set; }
        public string phylum { get; set; }
        public string family { get; set; }
        public string order { get; set; }
        public string environment { get; set; }
        public string functional { get; set; }
        public string locationfound { get; set; }
        public string firstdate { get; set; }
        public Factsheet factsheet { get; set; }
        public SpeciesStatus speciesStatus { get; set; }
        public PopulationStatus populationStatus { get; set; }
        public List<string> Synonyms { get; set; }
        public List<string> CommonNames { get; set; }
        public List<string> NativeRanges { get; set; }
        public string displaynativemap { get; set; }
        public string displaydistributionmap { get; set; }
    }

    public class Factsheets
    {
        public Species Species { get; set; }
    }
}