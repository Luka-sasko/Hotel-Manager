import React, { useState } from "react";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";

const DatePickerUser = ({ startDate, endDate, onChange }) => {
  const today = new Date();

  return (
    <DatePicker
      className="date-picker"
      selected={startDate}
      onChange={onChange}
      startDate={startDate}
      minDate={today}
      endDate={endDate}
      selectsRange
      selectsDisabledDaysInRange
      inline
    />
  );
};

export default DatePickerUser;
