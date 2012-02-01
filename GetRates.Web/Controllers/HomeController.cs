using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GetRates.Services;
using Ninject.Activation;

namespace GetRates.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IEnumerable<IExchangeRateProvider> providerServices;
        private readonly ICalculationService calculationService;
        private readonly IExchangeRateProvider referenceExchangeRateProvider;

        public HomeController(IEnumerable<IExchangeRateProvider> providerServices, ICalculationService calculationService, IExchangeRateProvider referenceExchangeRateProvider)
        {
            this.providerServices = providerServices;
            this.calculationService = calculationService;
            this.referenceExchangeRateProvider = referenceExchangeRateProvider;
        }

        public ActionResult Index()
        {
            ViewBag.Rate = referenceExchangeRateProvider.GetRate(1).Rate;
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

    public class BaseController : Controller
    {
        protected override void OnActionExecuted(ActionExecutedContext ctx) {
            base.OnActionExecuted(ctx);
            ViewBag.IsOnWeb = !Request.IsLocal;
        }
    }
}
