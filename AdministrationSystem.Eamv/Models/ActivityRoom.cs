namespace AdministrationSystem.Eamv.Models
{
    public class ActivityRoom
    {
        public int ActivityRoomID { get; set; }
        public Room Room { get; set; }

        public static List<ActivityRoom> ActivityRoomCopy(List<ActivityRoom> rooms)
        {
            List<ActivityRoom> roomsCopy = new List<ActivityRoom>();

            foreach (var r in rooms)
            {
                ActivityRoom room = new ActivityRoom();
                room.Room = r.Room;
                roomsCopy.Add(room);
            }

            return roomsCopy;
        }
    }

}

