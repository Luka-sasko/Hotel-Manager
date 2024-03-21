import React from "react";
import DashboardNavigation from "./DashboardNavigation";
import { Link } from "react-router-dom";

export const DashboardHomeRoomTypeNavbar = () => {
  return (
    <div className="navbar">
      <DashboardNavigation />
      <Link to="/addroomtype" className="add-link">
        Add room type
      </Link>
    </div>
  );
};
