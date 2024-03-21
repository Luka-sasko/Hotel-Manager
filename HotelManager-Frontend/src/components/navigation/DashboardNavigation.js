import React from "react";
import { NavLink } from "react-router-dom";

const DashboardNavigation = () => {
  const links = [
    { to: "/dashboardRoom", text: "Rooms" },
    { to: "/dashboardReceipt", text: "Receipts" },
    { to: "/dashboard-reservation", text: "Reservations" },
    { to: "/dashboardServices", text: "Services" },
    { to: "/dashboard-discount", text: "Discounts" },
    { to: "/dashboard-roomtype", text: "Room types" },
  ];

  return (
    <div className="dashboard-navigation">
      <h2 className="dashboard-navigation-header">Dashboard</h2>
      <nav className="navigation">
        {links.map((link) => (
          <NavLink
            key={link.to}
            to={link.to}
            activeClassName="active-link"
            exact={false}
          >
            {link.text}
          </NavLink>
        ))}
      </nav>
    </div>
  );
};

export default DashboardNavigation;
