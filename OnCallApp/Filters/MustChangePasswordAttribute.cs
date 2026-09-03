using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace OnCallApp.Filters
{
    // Filter to enforce password change on first login.
    public class MustChangePasswordAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var user = context.HttpContext.User;
            
            if (user.Identity != null && user.Identity.IsAuthenticated)
            {
                var mustChangeClaim = user.FindFirst("MustChangePassword")?.Value;
                
                if (mustChangeClaim == "True")
                {
                    var controller = context.RouteData.Values["controller"]?.ToString();
                    var action = context.RouteData.Values["action"]?.ToString();

                    // Allow access only to Account/ChangePassword and Account/Logout
                    if (controller == "Account" && (action == "ChangePassword" || action == "Logout"))
                    {
                        base.OnActionExecuting(context);
                        return;
                    }

                    context.Result = new RedirectToActionResult("ChangePassword", "Account", null);
                }
            }

            base.OnActionExecuting(context);
        }
    }
}
