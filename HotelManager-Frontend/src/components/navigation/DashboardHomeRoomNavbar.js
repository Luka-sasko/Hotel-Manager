import React from "react";
import DashboardRoomFilter from "../filter/DashBoardRoomFilter";
import { Link } from "react-router-dom";
import DashboardNavigation from "./DashboardNavigation";

export const DashboardHomeRoomNavbar = () => {
  return (
    <div className="navbar navbar-room-dashboard">
      <DashboardNavigation />
      <DashboardRoomFilter />
      <Link to="/dashBoardRoom/add" className="add-link">
        Add room
      </Link>
    </div>
  );
};
