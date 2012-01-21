using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RateCheck.Services;

namespace RateCheck.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEnumerable<IProviderService> providerServices;

        public HomeController()
            : this(new List<IProviderService>(){
             new GoogleProviderService()
            ,new MoneyToIndiaProviderService()
            ,new RemitToIndiaProviderService()
            ,new XoomProviderService()
        }) {

        }

        public HomeController(IEnumerable<IProviderService> providerServices) {
            this.providerServices = providerServices;
        }

        public ActionResult Index() {
            ViewBag.Message = "Use this application to view USD to INR converted amount from 3 major transfer service providers. For your reference, google finance rate is also provided.";

            return View();
        }

        [HttpPost]
        public ActionResult Index(float? amount) {
            ViewBag.Message = "Use this application to view USD to INR converted amount from 3 major transfer service providers. For your reference, google finance rate is also provided.";

            if (amount == null)
                return View();

            var returnedAmounts = providerServices.Aggregate(new List<ReturnedAmount>(), (seed, item) => {
                                                                            seed.Add(item.GetRate(amount.Value));
                                                                            return seed;
                                                                        });
            ViewBag.Amount = amount;
            return View(returnedAmounts.AsEnumerable());
        }

        public ActionResult About() {
            ViewBag.Message = "Your quintessential app description page.";

            return View();
        }

        public ActionResult Contact() {
            ViewBag.Message = "Your quintessential contact page.";

            return View();
        }
    }
}
