using AdministrationSystem.Eamv.Models.EntityFramework;
using AdministrationSystem.Eamv.Models;
using Microsoft.AspNetCore.Mvc;
using AdministrationSystem.Eamv.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace AdministrationSystem.Eamv.Controllers
{
    [Authorize]
    public class BannerController : Controller
    {
        private IBannerRepository bannerRepository;
        private IRepositoryCrud<Department> departmentRepository;
        private IFeedbackRepository feedbackRepository;

        public BannerController(IBannerRepository bannerRepository, IRepositoryCrud<Department> departmentRepository, IFeedbackRepository feedbackRepository)
        {
            this.bannerRepository = bannerRepository;
            this.departmentRepository = departmentRepository;
            this.feedbackRepository = feedbackRepository;
        }

        public ActionResult InformationBanner()
        {
            ViewBag.Departments = departmentRepository.Collection;
            ViewBag.Banners = bannerRepository.Collection;
            ViewBag.BannersHerning = bannerRepository.Collection.Where(r => r.Department.DepartmentName == "Herning");
            ViewBag.BannersHolstebro = bannerRepository.Collection.Where(r => r.Department.DepartmentName == "Holstebro");

            return View();
        }

        [HttpPost]
        public ActionResult InformationBanner(Banner banner)
        {
            ModelState.Remove("Department.DepartmentName");
            if (ModelState.IsValid)
            {
                banner.Department = departmentRepository.GetByID(banner.Department.DepartmentId);

                bannerRepository.Create(banner);
            }
            ViewBag.Departments = departmentRepository.Collection;
            return RedirectToAction("InformationBanner");
        }

        [HttpPost]
        public ActionResult DeleteBanner(int bannerId)
        {
            bannerRepository.Delete(bannerId);
            return RedirectToAction("InformationBanner");
        }

        [HttpPost]
        public ActionResult ChangeActiveBanner(int bannerId)
        {
            bannerRepository.ChangeIsActive(bannerId);
            return RedirectToAction("InformationBanner");
        }

        [HttpPost]
        public ActionResult SubmitFeedback(string feedbacktext, string returnUrl)
        {
            Feedback feedback = new Feedback();
            feedback.FeedbackDisc = "Sendt fra page: " + returnUrl + " === FeedbackTekst: " + feedbacktext;

            if (ModelState.IsValid)
            {
                feedbackRepository.Create(feedback);
                return Redirect(returnUrl);
            }

            return View("Index");
        }
    }
}
