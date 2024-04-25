using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class AdminAccess : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var adminId = context.HttpContext.Session.GetString("AdminId");

        if (string.IsNullOrEmpty(adminId))
        {
            context.Result = new RedirectToActionResult("Login", "Admin", null);
        }

        base.OnActionExecuting(context);
    }
}