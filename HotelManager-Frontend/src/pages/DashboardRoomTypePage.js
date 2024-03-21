import React from "react";
import { DashboardRoomType } from "../components/roomType/DashboardRoomType";
import { DashboardHomeRoomTypeNavbar } from "../components/navigation/DashboardHomeRoomTypeNavbar";

const DashboardRoomTypePage = () => {
  return (
    <div className="dashboard-room-type-page page">
      <DashboardHomeRoomTypeNavbar />
      <div className="container">
        <DashboardRoomType />
      </div>
    </div>
  );
};
export default DashboardRoomTypePage;
