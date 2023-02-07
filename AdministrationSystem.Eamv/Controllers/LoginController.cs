using AdministrationSystem.Eamv.Models;
using AdministrationSystem.Eamv.Models.EntityFramework;
using AdministrationSystem.Eamv.Models.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AdministrationSystem.Eamv.Controllers
{
    public class LoginController : Controller
    {
        private IUserRepository repository;
        private IFeedbackRepository feedbackRepository;

        public LoginController(IUserRepository repository, IFeedbackRepository feedbackRepository)
        {
            this.repository = repository;
            this.feedbackRepository = feedbackRepository;
        }

        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("CreateActivity", "Activity");

            return View();
        }

        public ActionResult Logout()
        {
            HttpContext.SignOutAsync("EamvCookie");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Index(User user)
        {
            if (user.UserName != null && user.Password != null)
                ModelState.Remove("UserRole");

            if (ModelState.IsValid)
            {
                if (repository.UserChecker(user))
                {
                    User LoggedInUser = repository.GetByUsername(user.UserName);

                    if (LoggedInUser.UserRole.Equals("Admin"))
                    {
                            List<Claim> claims = new List<Claim> {
                            new Claim("AdminUser", "true")
                        };

                        var identity = new ClaimsIdentity(claims, "EamvCookie");

                        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

                        HttpContext.SignInAsync("EamvCookie", claimsPrincipal);
                    } else
                    {
                            List<Claim> claims = new List<Claim> {
                            new Claim("StandardUser", "true")
                        };

                        var identity = new ClaimsIdentity(claims, "EamvCookie");

                        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

                        HttpContext.SignInAsync("EamvCookie", claimsPrincipal);
                    }
                    return RedirectToAction("CreateActivity", "Activity");
                }
                ViewBag.IncorrectLogin = "Du har indtastet forkert brugernavn eller adgangskode.";
                return View();
            }
            return View();
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
