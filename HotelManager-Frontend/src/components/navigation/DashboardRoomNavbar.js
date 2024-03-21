import React from "react";
import DashboardRoomFilter from "../filter/DashBoardRoomFilter";
import DashboardNavigation from "./DashboardNavigation";

export const DashboardRoomNavbar = () => {
  return (
    <div className="navbar">
      <DashboardNavigation />
      <DashboardRoomFilter />
    </div>
  );
};
