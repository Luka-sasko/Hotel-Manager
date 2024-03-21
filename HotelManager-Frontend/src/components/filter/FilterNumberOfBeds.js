import React, { useState } from "react";

const FilterNumberOfBeds = ({
  numberOfBeds,
  handleChange,
  isDisabled = false,
}) => {
  return (
    <label className="number-of-beds-row filter-label filter">
      <span>Number of beds:</span>
      <input
        className="filter-input number-of-beds-input"
        type="number"
        value={numberOfBeds}
        disabled={isDisabled}
        onChange={handleChange}
      />
    </label>
  );
};

export default FilterNumberOfBeds;
