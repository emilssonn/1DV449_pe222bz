using FindMyHome.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyHome.Tests.ViewModels
{
	[TestClass]
	public class SearchViewModelTest
	{
		[TestMethod]
		public void Test_SearchTerm_Length_Is_Invalid()
		{
			//Arrange
			var searchTerm = "m";

			var viewModel = Mock.Of<SearchViewModel>();
			viewModel.SearchTerms = searchTerm;

			var controller = new ViewModelValidationController();

			//Act
			var result = controller.TestTryValidateModel(viewModel);

			//Assert
			Assert.IsFalse(result);

			var modelState = controller.ModelState;

			Assert.AreEqual(1, modelState.Keys.Count);

			Assert.IsTrue(modelState.Keys.Contains("SearchTerms"));
			Assert.IsTrue(modelState["SearchTerms"].Errors.Count == 1);
			Assert.AreEqual(FindMyHome.Properties.Resources.SearchTermLengthSwe, modelState["SearchTerms"].Errors[0].ErrorMessage);
		}
	}
}
