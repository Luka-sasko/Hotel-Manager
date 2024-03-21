const AuthenticationFormRow = ({ inputName, inputType, label, value, handleChange }) => {
    let placeholder = `Enter your ${label.toLowerCase()}`;
    if (inputType === "confirmPassword") {
        inputType = "password";
        placeholder = "Re-enter your password";
    }
    else if (inputType === "phoneNumber") {
        inputType = "tel";
        placeholder = "Enter your phone number";
    }

    return (
        <p className="authentication-form-row">
            <label className="authentication-form-row-label" htmlFor={inputName}>{label}</label>
            <input className="authentication-form-row-input" type={inputType} id={inputName} name={inputName} value={value} onChange={handleChange} placeholder={placeholder} />
        </p>
    );
}

export default AuthenticationFormRow;