using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class RestaurantAccess : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var resId = context.HttpContext.Session.GetString("ResId");

        if (string.IsNullOrEmpty(resId))
        {
            context.Result = new RedirectToActionResult("Login", "Restaurant", null);
        }

        base.OnActionExecuting(context);
    }
}