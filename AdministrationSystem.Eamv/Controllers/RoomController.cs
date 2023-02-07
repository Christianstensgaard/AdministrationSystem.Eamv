using AdministrationSystem.Eamv.Models.EntityFramework;
using AdministrationSystem.Eamv.Models;
using Microsoft.AspNetCore.Mvc;
using AdministrationSystem.Eamv.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace AdministrationSystem.Eamv.Controllers
{
    [Authorize]
    public class RoomController : Controller
    {
        private IRepositoryCrud<Department> departmentRepository;
        private IRepositoryCrud<Room> roomRepository;
        private IFeedbackRepository feedbackRepository;

        public RoomController(IRepositoryCrud<Department> departmentRepository, IRepositoryCrud<Room> roomRepository, IFeedbackRepository feedbackRepository)
        {
            this.departmentRepository = departmentRepository;
            this.roomRepository = roomRepository;
            this.feedbackRepository = feedbackRepository;
        }

        public ActionResult RoomList()
        {
            ViewBag.SelectedDepartment = "Vælg afdeling";
            ViewBag.Department = departmentRepository.Collection;
            ViewBag.Room = roomRepository.Collection;
            return View();

        }

        [HttpPost]
        public ActionResult RoomListByDepartment(int departmentId)
        {


            Department department = departmentRepository.GetByID(departmentId);

            ViewBag.SelectedDepartment = department.DepartmentName;
            ViewBag.Department = departmentRepository.Collection;

            ViewBag.Room = roomRepository.Collection.Where(r => r.Department == department);
            return View("RoomList");
        }

        public ActionResult CreateRoom()
        {
            ViewBag.Department = departmentRepository.Collection;
            return View();
        }

        [HttpPost]
        public ActionResult CreateRoom(Room room)
        {
            if (room.Department != null)
                ModelState.Remove("Department.DepartmentName");

            if (ModelState.IsValid)
            {
                room.Department = departmentRepository.GetByID(room.Department.DepartmentId);

                roomRepository.Create(room);

                return RedirectToAction("RoomList");
            }

            ViewBag.Department = departmentRepository.Collection;
            return View();
        }

        public ActionResult DeleteRoom(int roomId)
        {
            roomRepository.Delete(roomId);
            return RedirectToAction("RoomList");
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
