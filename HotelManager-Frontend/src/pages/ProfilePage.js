import React from "react";
import { Profile } from "../components/Profile/Profile";
import { NavBar } from "../components/Common/NavBar";

export const ProfilePage = () => {
  return (
    <div className="profile-page page">
      <NavBar />
      <div className="container">
        <Profile />
      </div>
    </div>
  );
};
