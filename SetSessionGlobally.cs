using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;


namespace Regristration_Project
{
    public class SetSessionGlobally:ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var value = context.HttpContext.Session.GetString("UName");
            if (value==null)
            {
                context.Result = new RedirectToRouteResult(
                    new RouteValueDictionary   {
                        { "Controller","Account" },
                             { "Action","Login" 
                        }
                    });
            }
            base.OnActionExecuted(context);
        }

    }
}
