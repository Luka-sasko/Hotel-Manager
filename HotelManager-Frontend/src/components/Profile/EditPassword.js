import { useState, useEffect } from "react";

export const EditPassword = ({ handleEdit }) => {
  const [passwordData, setPasswordData] = useState({});
  const [passwordError, setPasswordError] = useState("");

  const handleChange = (e) => {
    setPasswordData({ ...passwordData, [e.target.name]: e.target.value });
  };

  useEffect(() => {
    handlePasswordValidation();
  }, [passwordData]);

  const handlePasswordValidation = () => {
    if (passwordData.passwordNew !== passwordData.passwordConfirm) {
      setPasswordError("Passwords do not match");
    } else {
      setPasswordError("");
    }
  };

  const handleSubmit = (e) => {
    e.preventDefault();

    if (!passwordError) {
      handleEdit(passwordData);
      e.target.reset();
    } else {
      alert("Passwords do not match");
    }
  };

  return (
    <div className="edit-profile profile-form">
      <h2 className="form-heading">CHANGE PASSWORD</h2>
      <form className="form" onSubmit={handleSubmit}>
        <input
          placeholder="Current password"
          type="password"
          name="passwordOld"
          id="currentPassword"
          className="form-input"
          onChange={handleChange}
        />
        <input
          placeholder="New password"
          type="password"
          id="newPassword"
          name="passwordNew"
          className="form-input"
          onChange={handleChange}
        />
        <input
          placeholder="New password again"
          type="password"
          id="newPasswordConfirm"
          name="passwordConfirm"
          className="form-input"
          onChange={handleChange}
        />

        <input type="submit" className="form-submit-button" value="Apply" />
      </form>
    </div>
  );
};
