import React from "react";
import DashboardReceiptsFilter from "../filter/DashBoardReceiptFilter";
import DashboardNavigation from "./DashboardNavigation";

export const DashboardReceiptsNavbar = () => {
  return (
    <div className="navbar">
      <DashboardNavigation />
      <DashboardReceiptsFilter />
    </div>
  );
};
