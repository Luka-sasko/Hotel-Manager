import React from "react";
import { DashboardDiscount } from "../components/discount/DashboardDiscount";
import { DashboardHomeDiscountsNavbar } from "../components/navigation/DashboardHomeDiscountNavbar";

const DashboardDiscountPage = () => {
  return (
    <div className="dashboard-discount-page page">
      <DashboardHomeDiscountsNavbar />
      <div className="container">
        <DashboardDiscount />
      </div>
    </div>
  );
};
export default DashboardDiscountPage;
