using AdministrationSystem.Eamv.Models.EntityFramework;
using AdministrationSystem.Eamv.Models;
using Microsoft.AspNetCore.Mvc;
using AdministrationSystem.Eamv.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace AdministrationSystem.Eamv.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        private IRepositoryCrud<Department> departmentRepository;
        private IFeedbackRepository feedbackRepository;

        public DepartmentController(IRepositoryCrud<Department> departmentRepository, IFeedbackRepository feedbackRepository)
        {
            this.departmentRepository = departmentRepository;
            this.feedbackRepository = feedbackRepository;
        }

        public ActionResult Index()
        {
            ViewBag.Departments = departmentRepository.Collection;
            return View();
        }

        [HttpPost]
        public ActionResult Index(Department department)
        {
            if (ModelState.IsValid)
            {
                departmentRepository.Create(department);
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult DeleteDepartment(int departmentId)
        {
            departmentRepository.Delete(departmentId);
            return RedirectToAction("Index");
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
