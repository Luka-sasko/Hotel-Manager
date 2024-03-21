import React from "react";
import DatePicker from 'react-datepicker';

const DatePickerAdmin = ({startDate, endDate, onChange}) => {

  return (
    <DatePicker
    selected={startDate}
    onChange={onChange}
    startDate={startDate}
    endDate={endDate}
    selectsRange
    selectsDisabledDaysInRange
    inline
    />
  );
};

export default DatePickerAdmin;
