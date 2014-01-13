using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FindMyHome.Tests.ViewModels
{
	public class ViewModelValidationController : Controller
	{
		public ViewModelValidationController()
        {
			ControllerContext = (new Mock<ControllerContext>()).Object;
        }

        public bool TestTryValidateModel(object model)
        {
            return TryValidateModel(model);
        }
	}
}
