using FindMyHome.Controllers;
using FindMyHome.Domain.Abstract;
using FindMyHome.Domain.Entities.Foursquare;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Hosting;

namespace FindMyHome.Tests.Controllers
{
	[TestClass]
	public class CategoriesControllerTest
	{
		[TestMethod]
		public void Test_Get_Throw_500_Error()
		{
			//Arrange
			var mockService = new Mock<IFindMyHomeService>();

			mockService.Setup(c => c.RefreshCategories()).
				Throws(new ApplicationException());

			var controller = new CategoriesController(mockService.Object);
			controller.Request = new HttpRequestMessage();
			controller.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

			//Act
			try
			{
				var result = controller.Get() as List<Category>;

				//Should not get here
				Assert.Fail();
			}
			catch (HttpResponseException e)
			{
				if (e.Response.StatusCode != HttpStatusCode.InternalServerError)
					Assert.Fail();
			}
		}

	}
}
