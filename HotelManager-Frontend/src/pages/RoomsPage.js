import React from "react";
import { RoomList } from "../components/room/RoomList";
import { NavBar } from "../components/Common/NavBar";
import { HomePageNavbar } from "../components/navigation/HomePageNavbar";

export const RoomsPage = () => {
  return (
    <div className="rooms-page page">
      <HomePageNavbar />
      <div className="container">
        <RoomList />
      </div>
    </div>
  );
};
