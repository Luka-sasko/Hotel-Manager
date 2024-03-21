import React from "react";
import { Radio, RadioGroup, FormControlLabel } from "@mui/material";

const FilterIsPaid = ({ onChange }) => {
  const handleTypeChange = (event) => {
    onChange(event.target.value);
  };

  return (
    <div className="FilterIsPaid filter">
      <label>Is paid:</label>
      <RadioGroup onChange={handleTypeChange} defaultValue="">
        <FormControlLabel value="" control={<Radio />} label="Both" />
        <FormControlLabel value={true} control={<Radio />} label="Yes" />
        <FormControlLabel value={false} control={<Radio />} label="No" />
      </RadioGroup>
    </div>
  );
};

export default FilterIsPaid;
