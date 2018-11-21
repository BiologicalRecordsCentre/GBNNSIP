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
    public class NativeRangeController : ApiController
    {
        private readonly NativerangeRepository nrRepository = new NativerangeRepository();
        public IEnumerable<Natives> GetNativeRange(int id)
        {
            if (nrRepository == null)
            {
                throw new ArgumentNullException("FactsheetRepository");
            }
            return nrRepository.GetNativeRange();
        }
    }
}
