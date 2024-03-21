import AuthenticationForm from "../components/authentication/AuthenticationForm";
import { useState } from "react";
import { createUser } from "../services/api_user";
import { useNavigate } from "react-router-dom";

const RegisterPage = () => {
  const [registerForm, setRegisterForm] = useState({
    email: "",
    firstName: "",
    lastName: "",
    phoneNumber: "",
    password: "",
    confirmPassword: "",
  });
  const navigate = useNavigate();

  const handleChange = (e) => {
    setRegisterForm({
      ...registerForm,
      [e.target.name]: e.target.value,
    });
  };

  const handleSubmit = async () => {
    if (registerForm.password !== registerForm.confirmPassword) {
      alert("Passwords do not match");
      return;
    }
    try {
      const response = await createUser(registerForm);
      if (response === 200) {
        navigate("/login");
      }
    } catch (error) {
      console.error("Register Error:", error);
    }
  };

  return (
    <div className="register-page authentification-page">
      <h2 className="register-page-header authentification-page-header">
        Welcome
      </h2>
      <AuthenticationForm
        formType="Register"
        formLabels={[
          "Email",
          "First Name",
          "Last Name",
          "Phone Number",
          "Password",
          "Confirm Password",
        ]}
        formValues={registerForm}
        handleChange={handleChange}
        handleSubmit={handleSubmit}
      />
    </div>
  );
};

export default RegisterPage;
