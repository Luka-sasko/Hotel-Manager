import AuthenticationFormRow from "./AuthenticationFormRow";
import { Link } from "react-router-dom";

const AuthenticationForm = ({
  formType,
  formLabels,
  formValues,
  handleChange,
  handleSubmit,
}) => {
  const formNames = Object.keys(formValues);
  return (
    <form className="authentication-form">
      <h3 className="authentication-form-header">{formType}</h3>
      {formNames.map((field, index) => {
        return (
          <AuthenticationFormRow
            key={index}
            inputName={field}
            inputType={field}
            label={formLabels[index]}
            value={formValues[field]}
            handleChange={handleChange}
          />
        );
      })}
      <div className="login-register-handle">
        <button
          className="authentication-form-button"
          type="button"
          onClick={handleSubmit}
        >
          {formType}
        </button>
        {formType === "Register" ? (
          <></>
        ) : (
          <Link to="/register">
            <h2 className="login-register-link">
              Dont have an account? Register Here
            </h2>
          </Link>
        )}
      </div>
    </form>
  );
};

export default AuthenticationForm;
