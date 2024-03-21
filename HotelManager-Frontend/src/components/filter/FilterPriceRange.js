import React from "react";

const PriceRange = ({
  minValue,
  maxValue,
  onChange,
  minId = "minPrice",
  maxId = "maxPrice",
}) => {
  return (
    <div className="range-price filter">
      <div className="input-container">
        <label htmlFor="minPrice" className="filter-label">
          Min:
        </label>
        <input
          className="input-container filter-input"
          type="number"
          id={minId}
          value={minValue}
          onChange={onChange}
        />
      </div>
      <div className="input-container">
        <label htmlFor="maxPrice" className="filter-label">
          Max:
        </label>
        <input
          className="input-container filter-input"
          type="number"
          id={maxId}
          value={maxValue}
          onChange={onChange}
        />
      </div>
    </div>
  );
};

export default PriceRange;
