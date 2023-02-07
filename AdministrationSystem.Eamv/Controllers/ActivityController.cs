using AdministrationSystem.Eamv.Infrastructure;
using AdministrationSystem.Eamv.Models;
using AdministrationSystem.Eamv.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace AdministrationSystem.Eamv.Controllers
{
    [Authorize]
    public class ActivityController : Controller
    {
        private IRepositoryCrud<Department> departmentRepository;
        private IRepositoryCrud<Room> roomRepository;
        private IActiveRepository activityRepository;
        private IFeedbackRepository feedbackRepository;
        private IUserRepository userRepository;
        private IBannerRepository bannerRepository;

        public ActivityController(IRepositoryCrud<Department> departmentRepository, IRepositoryCrud<Room> roomRepository, IFeedbackRepository feedbackRepository, IUserRepository userRepository, IActiveRepository activityRepository, IBannerRepository bannerRepository)
        {
            this.departmentRepository = departmentRepository;
            this.roomRepository = roomRepository;
            this.feedbackRepository = feedbackRepository;
            this.userRepository = userRepository;
            this.activityRepository = activityRepository;
            this.bannerRepository = bannerRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoadRooms(int departmentId)
        {
            return Json(roomRepository.Collection.Where(r => r.Department.DepartmentId ==
            departmentId).Select(s => new
            {
                Id = s.RoomId,
                Name = s.RoomName
            }));
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

        public ActionResult CreateActivity()
        {
            foreach (var item in roomRepository.Collection.Where(r => r.RoomName == "Ingen"))
            {
                ViewBag.noRooms = item.RoomId;
            }

            LoadRooms(departmentRepository.Collection.FirstOrDefault().DepartmentId);
            ViewBag.Departments = departmentRepository.Collection;
            return View();
        }

        [HttpPost]
        public ActionResult CreateActivity(Activity activity, string moreDates, string roomIds, string returnUrl)
        {
            List<Activity> activities = new List<Activity>();

            /* Checks if Endtime is before Starttime - already validates on clientside, this is an extra catch in case of something goes wrong
            with the clientside validation */
            if (activity.StartTime != null && activity.EndTime != null)
                if (TimeOnly.Parse(activity.StartTime) > TimeOnly.Parse(activity.EndTime))
                {
                    ViewBag.Departments = departmentRepository.Collection;
                    // Must return whole object if it comes from the Edit-page, in this case activityid is != 0
                    return activity.Activityid == 0 ? View() : View(activity);
                }

            if (activity.Date != default(DateTime) && roomIds != null && activity.Department != null)
            {
                ModelState.Remove("moredates");
                activity.Rooms = new ActivityLayer().GetRoomsFromIDList(roomIds, roomRepository);
                activity.Department = departmentRepository.GetByID(activity.Department.DepartmentId);
                activities.Add(activity);
            }
            else if (moreDates != null && roomIds != null && activity.Department != null)
            {
                ModelState.Remove("date");
                activity.Rooms = new ActivityLayer().GetRoomsFromIDList(roomIds, roomRepository);
                activity.Department = departmentRepository.GetByID(activity.Department.DepartmentId);
                // Makes a new instance for each date and adds them to the list
                new ActivityLayer().AddActivityToList(activities, moreDates, activity);
            }

            // Removes modelstate check on the following properties as they are not relavent.
            if (activity.Rooms != null)
            {
                ModelState.Remove("Rooms");
                ModelState.Remove("Room.Department");
                ModelState.Remove("Room.RoomName");
            } else
            {
                ViewBag.Departments = departmentRepository.Collection;
                // Must return whole object if it comes from the Edit-page
                return activity.Activityid == 0 ? View() : View(activity);
            }

            // Not relavent for modelstate to check these.
            ModelState.Remove("Department.DepartmentName");
            ModelState.Remove("returnUrl");

            if (ModelState.IsValid)
            {
                if (activity.Activityid == 0)
                    foreach (Activity a in activities)
                    {
                        activityRepository.Create(a);
                    }
                else // If activity ID != 0
                {
                    activityRepository.Update(activity);
                    if (returnUrl != null)
                    {
                        return RedirectToAction("SearchActivity", new RouteValueDictionary(
                            new
                            {
                                controller = "Activity",
                                action = "SearchActivity",
                                returnUrl = returnUrl
                            }));

                    }
                    else
                        return RedirectToAction("SearchActivity");
                }

            return RedirectToAction("SearchActivity", "Activity");

            }
            else // Safe catch, ModelState should never be invalid, as validation happens on clientside.
            {
                ViewBag.Departments = departmentRepository.Collection;
                if (activity.Date != null)
                    ViewBag.ReturnDate = activity.Date.ToString("yyyy-MM-dd");

                // Must return whole object if it comes from the Edit-page
                return activity.Activityid == 0 ? View() : View(activity);
            }
        }

        public ActionResult DeleteActivity(int activityId, string returnUrl)
        {
            if (returnUrl == null)
                ModelState.Remove("returnUrl");

            if (ModelState.IsValid)
            {
                activityRepository.Delete(activityId);
                return RedirectToAction("SearchActivity", new RouteValueDictionary(
                    new
                    {
                        controller = "Activity",
                        action = "SearchActivity",
                        returnUrl = returnUrl
                    }));
            }

            return RedirectToAction("SearchActivity");
        }

        public ActionResult EditActivity(int activityId, string returnUrl)
        {

            if (activityId == 0)
                return RedirectToAction("SearchActivity");

            ViewBag.SelectedRooms = new ActivityLayer().PreselectRooms(activityRepository, activityId);

            foreach (var item in roomRepository.Collection.Where(r => r.RoomName == "Ingen"))
            {
                ViewBag.noRooms = item.RoomId;
            }

            ModelState.Remove("returnUrl");

            ViewBag.returnUrl = returnUrl;

            if (ModelState.IsValid)
            {
                Activity activity = activityRepository.Collection // finds the correct activity in the database.
                    .Include(a => a.Rooms).ThenInclude(a => a.Room)
                    .Include(a => a.Department)
                    .FirstOrDefault(a => a.Activityid == activityId);

                LoadRooms(activity.Department.DepartmentId);
                ViewBag.Departments = departmentRepository.Collection;
                return View("CreateActivity", activity);

            }
            return RedirectToAction("SearchActivity");
        }

        public ActionResult CancelActivity(int activityId, string status, string returnUrl)
        {
            if (returnUrl == null)
                ModelState.Remove("returnUrl");

            if (ModelState.IsValid)
            {
                if (activityId != null && status != null)
                    if (status.Equals("true") || status.Equals("false"))
                    {
                        activityRepository.ChangeActivityStatus(activityId, bool.Parse(status));

                        if (returnUrl != null)
                        {
                            returnUrlAnalyzer analyzer = new returnUrlAnalyzer(returnUrl);

                            if (analyzer.CheckPath())
                            {
                                analyzer.Analyze();
                                return RedirectToAction("PreviewScreenPopUp", new RouteValueDictionary(
                                new
                                {
                                    controller = "Preview",
                                    action = "PreviewScreenPopUp",
                                    DepartmentID = analyzer.ValueSet["DepartmentID"],
                                    SelectedDate = analyzer.ValueSet["SelectedDate"]
                                }));
                            }
                        }
                        return RedirectToAction("SearchActivity", new RouteValueDictionary(
                            new
                            {
                                controller = "Activity",
                                action = "SearchActivity",
                                returnUrl = returnUrl
                            }));
                    }
            }
            ViewBag.ErrorOnBool = "Der opstod en fejl, kontakt udvikleren hvis fejlen fortsætter";
            ViewBag.Departments = departmentRepository.Collection;
            return View("SearchActivity");
        }

        public ActionResult SearchActivity(string returnUrl)
        {
            ViewBag.Departments = departmentRepository.Collection;

            if (returnUrl != null)
            {
                returnUrlAnalyzer analyzer = new returnUrlAnalyzer(returnUrl);
                analyzer.Analyze();

                int departmentid = int.Parse(analyzer.GetValue("DepartmentID"));
                string selecteddate = analyzer.GetValue("SelectedDate");
                string bywhom = analyzer.GetValue("ByWhom");
                string title = analyzer.GetValue("Title");


                ViewBag.DepartmentID = departmentid;
                try // Avoids error in case the user makes department input directly in the URL
                {
                    ViewBag.SelectedDepartment = departmentRepository.GetByID(departmentid).DepartmentName;
                }
                catch (Exception)
                {
                    ViewBag.ErrorOnDepartment = "Den indtastede afdeling findes ikke!";
                }

                ViewBag.SelectedDate = selecteddate;
                ViewBag.ByWhom = bywhom;
                ViewBag.Title = title;
                ViewBag.returnUrl = returnUrl;

                return View(activityRepository.Collection
                    .Include(a => a.Department)
                    .Include(a => a.Rooms).ThenInclude(r => r.Room)
                    .Where(a => a.Department.DepartmentId == departmentid &&
                    a.ByWhom.Contains(bywhom) &&
                    a.Title.Contains(title) &&
                    (selecteddate != "" ? a.Date == DateTime.Parse(selecteddate) : a.Date >= DateTime.Today)));
            }

            // Page-load without returnUrl information
            ViewBag.SelectedDepartment = "Vælg afdeling";
            return View(activityRepository.Collection
                .Include(a => a.Department)
                .Include(a => a.Rooms).ThenInclude(r => r.Room)
                .Where(a => a.Date >= DateTime.Today));
        }

        [HttpPost]
        public ActionResult SearchActivity(int departmentId, string selectedDate, string byWhom, string title)
        {
            ViewBag.returnUrl = "DepartmentID="+departmentId + (selectedDate == null ? "" : "&SelectedDate="+selectedDate) + (byWhom == null ? "" : "&ByWhom="+byWhom) + (title == null ? "" : "&Title="+title);

            ViewBag.Departments = departmentRepository.Collection;
            ViewBag.DepartmentID = departmentId;
            ViewBag.SelectedDepartment = departmentId == 0 ? "" : departmentRepository.GetByID(departmentId).DepartmentName;
            ViewBag.SelectedDate = selectedDate;
            ViewBag.ByWhom = byWhom;
            ViewBag.Title = title;

            return View(activityRepository.Collection
                .Include(a => a.Department)
                .Include(a => a.Rooms).ThenInclude(r => r.Room)
                .Where(a => a.Department.DepartmentId == departmentId && 
                a.ByWhom.Contains(byWhom == null ? "" : byWhom) && 
                a.Title.Contains(title == null ? "" : title) && 
                (selectedDate != null ? a.Date == DateTime.Parse(selectedDate) : a.Date >= DateTime.Today)));
        }
    }

}
