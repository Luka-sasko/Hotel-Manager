import React from "react";
import { useParams } from "react-router-dom";
import RoomTypeEdit from "../components/roomType/RoomTypeEdit";
import { DashboardEditViewNavbar } from "../components/navigation/DashboardEditViewNavbar";

export const EditRoomTypePage = () => {
  const { roomTypeId } = useParams();
  return (
    <div className="edit-room-type-page page">
      <DashboardEditViewNavbar />
      <div className="container">
        <RoomTypeEdit roomId={roomTypeId} />
      </div>
    </div>
  );
};
