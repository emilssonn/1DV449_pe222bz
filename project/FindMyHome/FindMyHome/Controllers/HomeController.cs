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

		/// <summary>
		/// Default index view, will load the search application
		/// </summary>
		/// <returns></returns>
        public ActionResult Index()
        {
			var viewModel = new IndexViewModel();
			//If the user is logged in, get the last 10 searches
			if (User.Identity.IsAuthenticated)
			{
				var userId = (int)Membership.GetUser().ProviderUserKey;
				viewModel.LastSearches = this._service.GetUserSearches(userId).ToList();
			}

            return View("Index", viewModel);
        }
    }
}
