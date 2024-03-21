import React, { useEffect, useState } from "react";
import { Navigate, useNavigate } from "react-router-dom";
import { getUserRole } from "../../services/api_user";

const AdminRouteGuard = ({ children }) => {
  const [userRole, setUserRole] = useState(null);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchUserRole = async () => {
      try {
        const role = await getUserRole();
        setUserRole(role);
      } catch (error) {
        console.error("Error fetching user role:", error);
        setUserRole(null);
      } finally {
        setLoading(false);
      }
    };

    fetchUserRole();
  }, []);

  if (loading) {
    return null;
  }

  if (userRole !== "Admin") {
    return <Navigate to="/error/" />;
  }

  return <>{children}</>;
};

export default AdminRouteGuard;
