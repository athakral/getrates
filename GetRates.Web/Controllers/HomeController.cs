using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GetRates.Services;

namespace GetRates.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEnumerable<IProviderService> providerServices;
        private readonly ICalculationService calculationService;
        private readonly IProviderService referenceProviderService;

        public HomeController(IEnumerable<IProviderService> providerServices, ICalculationService calculationService, IProviderService referenceProviderService)
        {
            this.providerServices = providerServices;
            this.calculationService = calculationService;
            this.referenceProviderService = referenceProviderService;
        }

        public ActionResult Index()
        {
            ViewBag.Rate = referenceProviderService.GetRate(1).Rate;
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
