import React from "react";
import { NavBar } from "../components/Common/NavBar";
import { ServiceEditForm } from "../components/services/ServiceEditForm";
import { DashboardEditViewNavbar } from "../components/navigation/DashboardEditViewNavbar";

export const EditServicePage = () => {
  return (
    <div className="edit-service-page page">
      <DashboardEditViewNavbar />
      <div className="container">
        <ServiceEditForm />
      </div>
    </div>
  );
};
