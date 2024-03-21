import { DashboardHomeRoomNavbar } from "../components/navigation/DashboardHomeRoomNavbar";
import { RoomListAdmin } from "../components/room/RoomListAdmin";

const DashboardRoomTablePage = () => {
  return (
    <div className="dashboard-room-table-page page">
      <DashboardHomeRoomNavbar />
      <div className="container">
        <RoomListAdmin />
      </div>
    </div>
  );
};

export default DashboardRoomTablePage;
