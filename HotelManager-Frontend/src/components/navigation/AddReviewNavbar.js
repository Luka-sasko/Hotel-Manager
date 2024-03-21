import React from "react";
import { Link } from "react-router-dom";

export const AddReviewNavbar = () => {
  return (
    <div className="navbar">
      <Link to="/my-reservations">Go back</Link>
    </div>
  );
};
