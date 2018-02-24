using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fritz.StreamTools.ViewComponents
{

	public class FooterViewComponent : ViewComponent
	{

		public Task<IViewComponentResult> InvokeAsync()
		{

			return Task.FromResult<IViewComponentResult>(View("default"));

		}


	}
}
