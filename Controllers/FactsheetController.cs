using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GBNNSS.Models;
using GBNNSS.Repository;


namespace GBNNSS.Controllers
{
    public class FactsheetController : ApiController
    {
        private readonly FactsheetRepository fsRepository = new FactsheetRepository();

        public IEnumerable<string> GetFactsheets()
        {
            return new string[] { "Factsheet not found. Please pass species id to get factsheet." };
        }

        public Factsheets GetFactsheets(int id)
        {
            if (fsRepository == null)
            {
                //throw new ArgumentNullException("FactsheetRepository");
                throw new HttpResponseException(HttpStatusCode.NotFound);
                ///throw new ArgumentNullException("Factsheet not found, Please use species id to get factsheet.");
            }

            var fs = this.fsRepository.GetFactsheets().FirstOrDefault();
            if (fs == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);                
            }
            Factsheets factsheets = new Factsheets();            
            factsheets.Species = fs;
            return factsheets;
        }
    }
}
