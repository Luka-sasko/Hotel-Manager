import React from "react";
import DashboardNavigation from "./DashboardNavigation";
import { Link } from "react-router-dom";

export const DashboardRoomTypeNavbar = () => {
  return (
    <div className="navbar">
      <DashboardNavigation />
      <Link to="/dashboard-roomtype/add" className="add-link">
        Add room type
      </Link>
    </div>
  );
};
