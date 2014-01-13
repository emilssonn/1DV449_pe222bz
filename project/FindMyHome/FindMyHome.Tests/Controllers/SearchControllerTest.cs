using FindMyHome.Controllers;
using FindMyHome.Domain.Abstract;
using FindMyHome.Domain.Entities;
using FindMyHome.Domain.Entities.Booli;
using FindMyHome.Domain.Entities.Foursquare;
using FindMyHome.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Mvc;

namespace FindMyHome.Tests.Controllers
{
	[TestClass]
	public class SearchControllerTest
	{
		[TestMethod]
		public void Test_Search_Return_0_Ads()
		{
			var searchTerm = "Kalmar";
			var mockService = new Mock<IFindMyHomeService>();
			mockService.Setup(s => s.SearchAds(searchTerm, null, 0, 0, 0, 0, 0)).
				Returns(new AdsContainer
				{
					Ads = new List<Ad>()
				});

			var viewModel = Mock.Of<SearchViewModel>();
			viewModel.SearchTerms = searchTerm;	
			var venueViewModel = Mock.Of<VenueSearchViewModel>();

			var identity = new GenericIdentity("Test");
			var principal = new Mock<IPrincipal>();
			principal.SetupGet(p => p.Identity).Returns(identity);
			principal.SetupGet(p => p.Identity.IsAuthenticated).Returns(false);

			Thread.CurrentPrincipal = principal.Object;
			
			var controller = new SearchController(mockService.Object);

			var result = controller.Get(viewModel, venueViewModel) as SearchResult;

			Assert.IsTrue(result.AdsContainer.Ads.Count == 0);
			Assert.IsTrue(result.Venues.Count == 0);
		}

		[TestMethod]
		public void Test_Search_Return_Ads_No_Venues()
		{
			var searchTerm = "Kalmar";
			var categories = "Food";
			var mockService = new Mock<IFindMyHomeService>();
			mockService.Setup(s => s.SearchAds(searchTerm, null, 0, 0, 0, 0, 0)).
				Returns(new AdsContainer
				{
					Ads = new List<Ad>() { new Ad() }
				});

			mockService.Setup(s => s.SearchVenues(searchTerm, categories)).
				Returns(new List<Venue>());

			var viewModel = Mock.Of<SearchViewModel>();
			viewModel.SearchTerms = searchTerm;
			var venueViewModel = Mock.Of<VenueSearchViewModel>();
			venueViewModel.Venues = categories;

			var identity = new GenericIdentity("Test");
			var principal = new Mock<IPrincipal>();
			principal.SetupGet(p => p.Identity).Returns(identity);
			principal.SetupGet(p => p.Identity.IsAuthenticated).Returns(false);

			Thread.CurrentPrincipal = principal.Object;

			var controller = new SearchController(mockService.Object);

			var result = controller.Get(viewModel, venueViewModel) as SearchResult;

			Assert.IsTrue(result.AdsContainer.Ads.Count == 1);
			Assert.IsTrue(result.Venues.Count == 0);
		}
	}
}
