using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GBNNSS.Repository;
using GBNNSS.Models;

namespace GBNNSS.Controllers
{
    public class SpeciesController : ApiController
    {
        private readonly SpeciesRepository speciesRepository = new SpeciesRepository();

        // GET api/values/{speciesname}
        public Names GetSpecies()
        {
            if (speciesRepository == null)
            {
                //throw new ArgumentNullException("SpeciesRepository");
                throw new ArgumentNullException("Species not found.");
            }

            var species = this.speciesRepository.GetSpecies();
            Names name = new Names();
            name.Species = species.ToList();
            return name;
        }

    }
}
