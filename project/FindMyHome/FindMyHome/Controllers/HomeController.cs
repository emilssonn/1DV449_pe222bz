using FindMyHome.Domain.Abstract;
using FindMyHome.Filters;
using FindMyHome.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace FindMyHome.Controllers
{
	[InitializeSimpleMembership]
    public class HomeController : Controller
    {
		private IFindMyHomeService _service;

		public HomeController(IFindMyHomeService service)
        {
            this._service = service;
        }

        public ActionResult Index()
        {
			var viewModel = new IndexViewModel();
			if (User.Identity.IsAuthenticated)
			{
				var userId = (int)Membership.GetUser().ProviderUserKey;
				viewModel.LastSearches = this._service.GetUserSearches(userId).ToList();
			}

            return View("Index", viewModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
