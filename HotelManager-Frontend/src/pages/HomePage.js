import React from "react";
import { Outlet } from "react-router-dom";
import { Header } from "../components/Common/Header";

export const HomePage = () => {
  return (
    <>
      <header className="header">
        <Header />
      </header>
      <main className="main">
        <Outlet />
      </main>
    </>
  );
};
