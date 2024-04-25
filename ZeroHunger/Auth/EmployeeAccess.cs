using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class EmployeeAccess : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var empId = context.HttpContext.Session.GetString("EmpId");

        if (string.IsNullOrEmpty(empId))
        {
            context.Result = new RedirectToActionResult("Login", "Employee", null);
        }

        base.OnActionExecuting(context);
    }
}