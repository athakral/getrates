using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using RateCheck.Services;

namespace RateCheck.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEnumerable<IProviderService> providerServices;
        private readonly ICalculationService calculationService;

        public HomeController(IEnumerable<IProviderService> providerServices,ICalculationService calculationService)
        {
            this.providerServices = providerServices;
            this.calculationService = calculationService;
        }

        public ActionResult Index() {
            return View();
        }

        [HttpPost]
        public ActionResult Index(decimal? amount) {
            if (amount == null)
                return Request.IsAjaxRequest()? (ActionResult) PartialView("_ConversionTable"):View();

            var returnedAmounts = providerServices.Aggregate(new List<CalculatedData>(), (seed, item) => {
                seed.Add(calculationService.Process(item.GetRate(amount.Value),amount.Value));
                                                                            return seed;
                                                                        });
            ViewBag.Amount = amount;
            if (Request.IsAjaxRequest())
                return PartialView("_ConversionTable", returnedAmounts.AsEnumerable());
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
