using ContactManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCADADataContracts;
using SCADAWebServiceClientLibrary;
using System.Security.Claims;

namespace ContactManager.Controllers
{
    public class MapController : Controller
    {
        // GET: Map
        public ActionResult Index()
        {

            Map map = new Map();
            
            var userIdValue = "";
            var claimsIdentity = User.Identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                // the principal identity is a claims identity.
                // now we need to find the NameIdentifier claim
                var userIdClaim = claimsIdentity.Claims
                    .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

                if (userIdClaim != null)
                {
                    userIdValue = userIdClaim.Value;
                }
            }

            ViewBag.userId = userIdValue;
             
            map.Center = SCADAClient.GetUserMapCenter(userIdValue);
            map.FilterComponents = SCADAClient.GetFilterComponents(userIdValue);

            return View(map);
        }
   
    }
}