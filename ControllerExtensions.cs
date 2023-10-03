
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace AspNetEmployeeSurvey
{
    

    public static class ControllerExtensions
    {
        public static IActionResult RedirectToActionWithRouteName(
            this Controller controller,
            string actionName,
            string controllerName,
            object routeValues,
            string routeName)
        {
            var urlHelperFactory = controller.Url;
            var urlHelper = new UrlHelper(urlHelperFactory.ActionContext);

            var url = urlHelper.Action(actionName, controllerName, routeValues, null, routeName);
            return controller.Redirect(url);
        }
    }
}
