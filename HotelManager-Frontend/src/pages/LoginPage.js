import AuthenticationForm from "../components/authentication/AuthenticationForm";
import { useState } from "react";
import { loginUser } from "../services/api_user";
import { useNavigate } from "react-router-dom";

const LoginPage = () => {
  const [loginForm, setLoginForm] = useState({
    email: "",
    password: "",
  });
  const navigate = useNavigate();

  const handleChange = (e) => {
    setLoginForm({
      ...loginForm,
      [e.target.name]: e.target.value,
    });
  };

  const handleSubmit = async () => {
    try {
      const response = await loginUser(loginForm);
      if (response.status === 200) {
        window.localStorage.setItem("token", response.data.access_token);
        navigate("/");
      }
    } catch (error) {
      console.error("Login Error:", error);
      if (error.response.data.error === "invalid_grant") {
        alert(error.response.data.error_description);
      }
    }
  };

  return (
    <div className="login-page authentification-page">
      <h2 className="login-page-header authentification-page-header">
        Welcome
      </h2>
      <AuthenticationForm
        formType="Log in"
        formLabels={["Email", "Password"]}
        formValues={loginForm}
        handleChange={handleChange}
        handleSubmit={handleSubmit}
      />
    </div>
  );
};

export default LoginPage;
