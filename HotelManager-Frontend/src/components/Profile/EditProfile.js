import { useState } from "react";

export const EditProfile = ({ handleEdit }) => {
  const [newUserData, setNewUserData] = useState({
    firstName: "",
    lastName: "",
    email: "",
    phone: "",
  });

  const handleChange = (e) => {
    setNewUserData({ ...newUserData, [e.target.name]: e.target.value });
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    handleEdit(newUserData);
    e.target.reset();
  };

  return (
    <div className="edit-profile profile-form">
      <h2 className="form-heading">EDIT PROFILE</h2>
      <form className="form" onSubmit={handleSubmit}>
        <input
          placeholder="Name"
          name="firstName"
          type="text"
          id="name"
          className="form-input"
          onChange={handleChange}
        />
        <input
          placeholder="Surname"
          name="lastName"
          type="text"
          id="surname"
          className="form-input"
          onChange={handleChange}
        />
        <input
          placeholder="Email"
          name="email"
          type="email"
          id="email"
          className="form-input"
          onChange={handleChange}
        />
        <input
          placeholder="Phone number"
          name="phone"
          id="phone"
          type="text"
          className="form-input"
          onChange={handleChange}
        />
        <input type="submit" className="form-submit-button" value="Apply" />
      </form>
    </div>
  );
};
