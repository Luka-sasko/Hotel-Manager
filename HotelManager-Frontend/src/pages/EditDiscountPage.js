import React from "react";
import { NavBar } from "../components/Common/NavBar";
import { useParams } from "react-router-dom";
import DiscountEdit from "../components/discount/DiscountEdit";
import { DashboardEditViewNavbar } from "../components/navigation/DashboardEditViewNavbar";

export const EditDiscountPage = () => {
  const { discountId } = useParams();
  return (
    <div className="edit-discount-page page">
      <DashboardEditViewNavbar />
      <div className="container">
        <DiscountEdit discountId={discountId} />
      </div>
    </div>
  );
};
