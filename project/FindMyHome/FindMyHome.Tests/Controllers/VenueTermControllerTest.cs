using FindMyHome.Controllers;
using FindMyHome.Domain.Abstract;
using FindMyHome.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyHome.Tests.Controllers
{
	[TestClass]
	public class VenueTermControllerTest
	{
		[TestMethod]
		public void Test_Venue_Terms_Return_Are_2()
		{
			var term = "food";
			var mockService = new Mock<IFindMyHomeService>();
			mockService.Setup(s => s.GetVenueSearchTerms(term)).
				Returns(new List<string>
				{
					"Food",
					"Italian Food"
				});

			var viewModel = Mock.Of<VenueTermViewModel>();
			viewModel.VenueTerm = term;

			var controller = new VenueTermController(mockService.Object);

			var result = controller.Get(viewModel) as List<string>;

			Assert.IsTrue(result.Count() == 2);
		}
	}
}
