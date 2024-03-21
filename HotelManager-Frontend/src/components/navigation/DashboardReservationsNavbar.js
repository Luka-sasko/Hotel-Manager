import React from "react";
import DashBoardReservationFilter from "../filter/DashBoardReservationFilter";
import DashboardNavigation from "./DashboardNavigation";

export const DashboardReservationsNavbar = () => {
  return (
    <div className="navbar">
      <DashboardNavigation />
      <DashBoardReservationFilter />
    </div>
  );
};
