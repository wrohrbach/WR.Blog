using System;
using System.Linq;
using System.Web.Mvc;

using WR.Blog.Data.Models;
using System.Data;
using WR.Blog.Business.Services;
using AutoMapper;

namespace WR.Blog.Mvc.Areas.SiteAdmin.Controllers
{
    public class AdminHomeController : SiteAdminBaseController
    {
        private readonly ISiteManagerService manager;

        public AdminHomeController(ISiteManagerService manager)
        {
            this.manager = manager;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Settings()
        {
            SiteSettingsDto settings = manager.GetSiteSettings();

            return View(settings);
        }

        [HttpPost, ActionName("Settings")]
        public ActionResult SettingsPost()
        {
            var settingsModel = new SiteSettingsDto();
            TryUpdateModel(settingsModel);

            if (ModelState.IsValid)
            {
                var settings = manager.GetSiteSettings();
                Mapper.Map<SiteSettingsDto, SiteSettingsDto>(settingsModel, settings);

                var user = manager.GetUser(User.Identity.Name);
                
                if (user == null)
                {
                    return new HttpUnauthorizedResult();
                }

                settings.LastModifiedBy = user;
                settings.LastModifiedDate = DateTime.Now;                

                manager.AddOrUpdateSiteSettings(settings);

                return RedirectToAction("Index");
            }

            return View(settingsModel);
        }
    }
}
