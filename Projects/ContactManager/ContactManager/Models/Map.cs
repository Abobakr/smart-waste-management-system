using SCADADataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ContactManager.Models
{
    public class Map
    {
        public LocationType Center { get; set; }

        public IList<FilterComponent> FilterComponents { get; set; }
    }
}