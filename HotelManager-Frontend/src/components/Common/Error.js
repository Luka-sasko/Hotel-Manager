import React from 'react';

const ErrorComponent = ({ errorCode }) => {
  let errorMessage = '';
  switch (errorCode) {
    case 400:
      errorMessage = 'Error 400: Bad Request. Please check your data.';
      break;
    case 404:
      errorMessage = 'Error 404: Page Not Found.';
      break;
    case 500:
      errorMessage = 'Error 500: Internal Server Error.';
      break;
    default:
      errorMessage = 'An error occurred.';
  }

  return (
    <div className="error-container">
      <p className="error-message">{errorMessage}</p>
    </div>
  );
};

export default ErrorComponent;
