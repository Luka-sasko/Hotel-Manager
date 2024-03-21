import React from "react";
import ReservationForm from "../components/reservation/ReservationForm";
import { DashboardEditViewNavbar } from "../components/navigation/DashboardEditViewNavbar";

export default function ReservationViewEdit() {
  return (
    <div className="dashboard-reservation-view-page page">
      <DashboardEditViewNavbar />
      <div className="container">
        <h2 className="edit-view-header">Reservation Details</h2>
        <ReservationForm />
      </div>
    </div>
  );
}
