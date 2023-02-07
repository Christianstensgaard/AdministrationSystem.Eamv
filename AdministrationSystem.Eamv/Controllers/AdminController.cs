using AdministrationSystem.Eamv.Models;
using AdministrationSystem.Eamv.Models.EntityFramework;
using AdministrationSystem.Eamv.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdministrationSystem.Eamv.Controllers
{
    [Authorize(Policy = "AdminUser")]
    public class AdminController : Controller
    {
        private IUserRepository userRepository;
        private IFeedbackRepository feedbackRepository;

        public AdminController(IUserRepository userRepository, IFeedbackRepository feedbackRepository)
        {
            this.userRepository = userRepository;
            this.feedbackRepository = feedbackRepository;
        }

        public ActionResult Users()
        {
            ViewBag.Users = userRepository.Collection;
            return View();
        }

        public ActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateUser(User user)
        {

            if (user.UserId != 0)
                ModelState.Remove("Password");

            if (ModelState.IsValid)
            {
                if (user.UserId == 0)
                    userRepository.Create(user);
                else
                    userRepository.Update(user);

                return RedirectToAction("Users");
            }

            if (user.UserId != 0)
                return View(user);
            else
                return View();
        }

        [HttpPost]
        public ActionResult DeleteUser(int userId)
        {
            userRepository.Delete(userId);
            return RedirectToAction("Users");
        }

        public ActionResult EditUser(User user)
        {
            return View("CreateUser", user);
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
