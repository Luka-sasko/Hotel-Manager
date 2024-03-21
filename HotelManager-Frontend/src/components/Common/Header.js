import React, { useState, useEffect } from "react";
import { Link } from "react-router-dom";
import { getUserRole } from "../../services/api_user";
import { useNavigate } from "react-router-dom";

export const Header = () => {
  const [userRole, setUserRole] = useState(null);
  const navigate = useNavigate();

  const handleLogout = () => {
    localStorage.removeItem("token");
    navigate("/");
    window.location.reload();
  };

  useEffect(() => {
    const checkTokenAndFetchUserRole = async () => {
      try {
        const token = window.localStorage.getItem("token");
        if (token) {
          const role = await getUserRole();
          console.log(role);
          setUserRole(role);
        }
      } catch (error) {
        console.error("Error fetching user role:", error);
      }
    };

    checkTokenAndFetchUserRole();
  }, []);

  return (
    <>
      <Link to="/">
        <h1 className="main-title">Hotel Manager</h1>
      </Link>
      <ul className="headerLinks">
        {userRole === "Admin" && (
          <>
            <li className="headerLink">
              <Link to="/dashboardroom">Admin Dashboard</Link>
            </li>
            <li className="headerLink">
              <Link to="/my-profile">My Profile</Link>
            </li>
            <button className="headerLink logout" onClick={handleLogout}>
              Logout
            </button>
          </>
        )}
        {userRole === "User" && (
          <>
            <li className="headerLink">
              <Link to="/my-reservations">My reservations</Link>
            </li>
            <li className="headerLink">
              <Link to="/my-profile">My profile</Link>
            </li>
            <button className="headerLink logout" onClick={handleLogout}>
              Logout
            </button>
          </>
        )}
        {!userRole && (
          <>
            <li className="headerLink">
              <Link to="/login">Login</Link>
            </li>
            <li className="headerLink">
              <Link to="/register">Register</Link>
            </li>
          </>
        )}
      </ul>
    </>
  );
};
