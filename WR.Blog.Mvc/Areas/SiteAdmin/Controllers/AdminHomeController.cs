using System;
using System.Web.Mvc;
using AutoMapper;
using WR.Blog.Business.Services;
using WR.Blog.Data.Models;
using WR.Blog.Mvc.Areas.SiteAdmin.Models;

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
            BlogSettingsDto settingsDto = manager.GetBlogSettings();

            BlogSettings settingsModel = Mapper.Map<BlogSettings>(settingsDto);

            return View(settingsModel);
        }

        [HttpPost, ActionName("Settings")]
        public ActionResult SettingsPost()
        {
            var settingsModel = new BlogSettings();
            TryUpdateModel(settingsModel);

            if (ModelState.IsValid)
            {
                var settingsDto = manager.GetBlogSettings();
                Mapper.Map<BlogSettings, BlogSettingsDto>(settingsModel, settingsDto);

                var user = manager.GetUser(User.Identity.Name);
                
                if (user == null)
                {
                    return new HttpUnauthorizedResult();
                }

                settingsDto.LastModifiedBy = user;
                settingsDto.LastModifiedDate = DateTime.Now;                

                manager.AddOrUpdateBlogSettings(settingsDto);

                return RedirectToAction("Index");
            }

            return View(settingsModel);
        }
    }
}
