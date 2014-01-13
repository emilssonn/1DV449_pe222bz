using FindMyHome.Controllers;
using FindMyHome.Domain.Abstract;
using FindMyHome.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FindMyHome.Tests.Controllers
{
	[TestClass]
	public class SearchTermControllerTest
	{
		[TestMethod]
		public void Test_Search_Terms_Return_Are_2()
		{
			var searchTerm = "ma";
			var mockService = new Mock<IFindMyHomeService>();
			mockService.Setup(s => s.GetSearchTerms(searchTerm)).
				Returns(new List<string>
				{
					"Kalmar",
					"Emmaboda"
				});

			var viewModel = Mock.Of<SearchTermViewModel>();
			viewModel.SearchTerms = searchTerm;

			var controller = new SearchTermController(mockService.Object);

			var result = controller.Get(viewModel) as List<string>;

			Assert.IsTrue(result.Count() == 2);
		}

		[TestMethod]
		public void Test_Search_Terms_Return_Are_1()
		{
			var searchTerm = "Kalmar";
			var mockService = new Mock<IFindMyHomeService>();
			mockService.Setup(s => s.GetSearchTerms(searchTerm)).
				Returns(new List<string>
				{
					"kAlMaR",
					"Emmaboda"
				});

			var viewModel = Mock.Of<SearchTermViewModel>();
			viewModel.SearchTerms = searchTerm;

			var controller = new SearchTermController(mockService.Object);

			var result = controller.Get(viewModel) as List<string>;

			Assert.IsTrue(result.Count() == 2);
		}
	}
}
