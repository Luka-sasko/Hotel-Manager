import React from "react";
import RoomTypeAdd from "../components/roomType/RoomTypeAdd";
import { DashboardEditViewNavbar } from "../components/navigation/DashboardEditViewNavbar";

export const AddRoomTypePage = () => {
  return (
    <div className="add-room-type-page page">
      <DashboardEditViewNavbar />
      <div className="container">
        <RoomTypeAdd />
      </div>
    </div>
  );
};
