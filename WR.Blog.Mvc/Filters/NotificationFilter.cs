using System.Web.Mvc;
using WR.Blog.Business.Services;

namespace WR.Blog.Mvc.Filters
{
    public class NotificationFilter : ActionFilterAttribute
    {
        private readonly IBlogService blogger;

        public NotificationFilter()
        { }

        public NotificationFilter(IBlogService blogger)
        {
            this.blogger = blogger;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var user = filterContext.HttpContext.User;
            var viewBag = filterContext.Controller.ViewBag;
            var url = new UrlHelper(filterContext.RequestContext);

            if (user.IsInRole(System.Configuration.ConfigurationManager.AppSettings["AdministratorRole"]) && viewBag.BlogSettings.ModerateComments)
            {
                int commentCount = blogger.GetUnapprovedCommentCount();

                if (commentCount > 0)
                {
                    viewBag.MessageClass = "message-success";
                    viewBag.Message = "There are comments awaiting moderation. <a href=\"" + url.Action("Comments", "BlogAdmin", routeValues: new { Area = "SiteAdmin" }) + "\">Click here</a>";
                }
            }

            base.OnActionExecuted(filterContext);
        }
    }
}