using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Hospital_Management_System.ActionFilter
{
    public class SessionCheck : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            bool isAnonymous = context.ActionDescriptor.EndpointMetadata
                .OfType<AllowAnonymousAttribute>().Any();

            if (isAnonymous)
            {
                return; 
            }

            string? userName = context.HttpContext.Session.GetString("username");
            int? userId = context.HttpContext.Session.GetInt32("UserId");

            if (userName == null || userId == null)
            {
                context.Result = new RedirectToActionResult("Login", "Admin", null);
            }
            base.OnActionExecuting(context);
        }
    }
}
