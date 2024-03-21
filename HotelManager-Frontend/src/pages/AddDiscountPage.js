import React from "react";
import DiscountAdd from "../components/discount/DiscountAdd";
import { DashboardEditViewNavbar } from "../components/navigation/DashboardEditViewNavbar";

export const AddDiscountPage = () => {
  return (
    <div className="discount-page page">
      <DashboardEditViewNavbar />
      <div className="container">
        <DiscountAdd />
      </div>
    </div>
  );
};
