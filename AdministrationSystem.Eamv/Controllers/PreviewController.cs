using AdministrationSystem.Eamv.Models;
using AdministrationSystem.Eamv.Models.EntityFramework;
using AdministrationSystem.Eamv.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdministrationSystem.Eamv.Controllers
{
    [Authorize]
    public class PreviewController : Controller
    {

        private IRepositoryCrud<Department> departmentRepository;
        private IBannerRepository bannerRepository;
        private IActiveRepository activityRepository;
        private IFeedbackRepository feedbackRepository;

        public PreviewController(IRepositoryCrud<Department> departmentRepository, IBannerRepository bannerRepository, IActiveRepository activityRepository, IFeedbackRepository feedbackRepository)
        {
            this.departmentRepository = departmentRepository;
            this.bannerRepository = bannerRepository;
            this.activityRepository = activityRepository;
            this.feedbackRepository = feedbackRepository;
        }

        public ActionResult Index()
        {
            ViewBag.Departments = departmentRepository.Collection;
            return View();
        }

        public ActionResult PreviewScreenPopUp(int departmentId, string selectedDate)
        {
            ViewBag.returnUrl = "Preview/DepartmentID=" + departmentId + "&SelectedDate=" + selectedDate;

            ViewBag.Banners = bannerRepository.Collection.Where(d => d.Department.DepartmentId == departmentId);
            if (ModelState.IsValid)
                return View(activityRepository.Collection
                    .Include(a => a.Department)
                    .Include(a => a.Rooms).ThenInclude(r => r.Room)
                    .Where(a => a.Department.DepartmentId == departmentId && a.Date == DateTime.Parse(selectedDate)));

            return RedirectToAction("PreviewScreen");
        }

        public ActionResult InfoScreenHolstebro()
        {
            ViewBag.Banners = bannerRepository.Collection.Where(b => b.Department.DepartmentName == "Holstebro");
            ViewBag.DepartmentName = "Holstebro";

            return View("PartialInfoScreen", activityRepository.Collection
                .Include(a => a.Department)
                .Include(a => a.Rooms).ThenInclude(r => r.Room)
                .Where(a => a.Date == DateTime.Today && a.Department.DepartmentName == "Holstebro"));
        }

        public ActionResult InfoScreenHerning()
        {
            ViewBag.Banners = bannerRepository.Collection.Where(b => b.Department.DepartmentName == "Herning");
            ViewBag.DepartmentName = "Herning";

            return View("PartialInfoScreen", activityRepository.Collection
                .Include(a => a.Department)
                .Include(a => a.Rooms).ThenInclude(r => r.Room)
                .Where(a => a.Date == DateTime.Today && a.Department.DepartmentName == "Herning"));
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
