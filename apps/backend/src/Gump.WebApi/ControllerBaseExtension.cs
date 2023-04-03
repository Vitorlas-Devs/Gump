using Gump.Data;
using Microsoft.AspNetCore.Mvc;

namespace Gump.WebApi;

public static class ControllerBaseExtension
{
	public static IActionResult Run(this ControllerBase controller, Func<IActionResult> action)
	{
		try
		{
			return action();
		}
		catch (AggregateException e)
		{
			return controller.Problem(e.Message);
		}
		catch (ArgumentException e)
		{
			return controller.Problem(e.Message);
		}
		catch (DuplicateException e)
		{
			return controller.Problem(e.Message);
		}
		catch (NotFoundException e)
		{
			return controller.NotFound(e.Message);
		}
		catch (RestrictedException e)
		{
			return controller.Unauthorized(e.Message);
		}
		catch (Exception e)
		{
			return controller.Problem(e.Message);
		}
	}

	public static async Task<IActionResult> Run(this ControllerBase controller, Func<Task<IActionResult>> action)
	{
		try
		{
			return await action();
		}
		catch (AggregateException e)
		{
			return controller.Problem(e.Message);
		}
		catch (ArgumentException e)
		{
			return controller.Problem(e.Message);
		}
		catch (DuplicateException e)
		{
			return controller.Problem(e.Message);
		}
		catch (NotFoundException e)
		{
			return controller.NotFound(e.Message);
		}
		catch (RestrictedException e)
		{
			return controller.Unauthorized(e.Message);
		}
		catch (Exception e)
		{
			return controller.Problem(e.Message);
		}
	}
}

