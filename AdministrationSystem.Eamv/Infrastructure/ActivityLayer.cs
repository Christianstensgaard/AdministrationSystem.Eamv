using AdministrationSystem.Eamv.Models.EntityFramework;
using AdministrationSystem.Eamv.Models;
using AdministrationSystem.Eamv.Models.Interfaces;
using System.Diagnostics;
using Activity = AdministrationSystem.Eamv.Models.Activity;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace AdministrationSystem.Eamv.Infrastructure
{
    public class ActivityLayer
    {

        //Hjælpmetode til at returnere en liste af rooms ud fra en string af roomID'er
        public List<ActivityRoom> GetRoomsFromIDList(string roomids, IRepositoryCrud<Room> roomRepository)
        {
            List<ActivityRoom> rooms = new List<ActivityRoom>();

            string[] roomidlist = roomids.Split(",");

            foreach (string RoomID in roomidlist)
            {
                ActivityRoom room = new ActivityRoom();
                room.Room = roomRepository.GetByID(int.Parse(RoomID));
                rooms.Add(room);
            }
            return rooms;
        }

        public void AddActivityToList(List<Activity> activities, string moredates, Activity activity)
        {
            string[] dates = moredates.Split(",");

            foreach (string d in dates)
            {
                Activity a = new Activity();
                a = Activity.Copy(activity);
                a.Date = DateTime.Parse(d.Trim());
                activities.Add(a);
            }
        }

        public string PreselectRooms(IActiveRepository activityRepository, int ActivityID)
        {
            StringBuilder selectedrooms = new StringBuilder();
            foreach (var item in activityRepository.Collection.Include(a => a.Rooms).ThenInclude(r => r.Room).Where(a => a.Activityid == ActivityID))
            {
                foreach (var room in item.Rooms)
                {
                    var last = item.Rooms.Last();

                    selectedrooms.Append((room.Room.RoomId) + "");
                    if (last == item.Rooms.Last())
                        selectedrooms.Append(",");
                }
            }

            return selectedrooms.ToString();
        }

    }
}
