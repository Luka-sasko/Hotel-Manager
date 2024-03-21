import React from "react";
import PriceRange from "../filter/FilterPriceRange";
import DashboardNavigation from "./DashboardNavigation";

export const DashboardDiscountsNavbar = () => {
  return (
    <div className="navbar">
      <DashboardNavigation />
      <PriceRange />
    </div>
  );
};
