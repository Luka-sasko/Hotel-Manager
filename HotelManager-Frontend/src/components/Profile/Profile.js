import { useState, useEffect } from "react";
import { ProfileData } from "./ProfileData";
import { EditProfile } from "./EditProfile";
import { EditPassword } from "./EditPassword";

import { getUser, updatePassword, updateUser } from "../../services/api_user";

export const Profile = () => {
  const [user, setUser] = useState({
    firstName: "",
    lastName: "",
    email: "",
    phone: "",
  });

  const fetchData = async () => {
    try {
      const userData = await getUser();
      delete userData.data.role;
      setUser(userData.data);
    } catch (error) {
      console.error("Error fetching user:", error);
    }
  };

  useEffect(() => {
    fetchData();
  }, []);

  const handleProfileEdit = async (userData) => {
    try {
      await updateUser(userData);
      fetchData();
    } catch (error) {
      console.error(error);
    }
  };

  const handlePasswordEdit = async (passwordData) => {
    try {
      await updatePassword(passwordData);
      fetchData();
    } catch (error) {
      console.error(error);
    }
  };

  return (
    <div className="profile">
      <ProfileData user={user} />
      <div className="profile-forms">
        <EditProfile handleEdit={handleProfileEdit} />
        <EditPassword handleEdit={handlePasswordEdit} />
      </div>
    </div>
  );
};
