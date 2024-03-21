import React, { useState, useEffect } from "react";
import { Radio, RadioGroup, FormControlLabel } from "@mui/material";
import { getAllRoomType } from "../../services/api_room_type";

const FilterRoomType = ({ isDisabled, onChangeHandle }) => {
  const [roomTypes, setRoomTypes] = useState([]);
  const [selectedRoomType, setSelectedRoomType] = useState("");

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = async () => {
    const [data, totalPages] = await getAllRoomType();
    setRoomTypes(data);
  };

  const handleRoomTypeChange = (event) => {
    const selectedType = event.target.value;
    setSelectedRoomType(selectedType);
    onChangeHandle(selectedType);
  };

  return (
    <div className="FilterRoomType filter">
      <label className="filter-label">Room Type:</label>
      <RadioGroup
        value={selectedRoomType}
        onChange={handleRoomTypeChange}
        className="roomTypeList"
      >
        {roomTypes.map((roomType, index) => (
          <FormControlLabel
            key={index}
            value={roomType.id}
            control={<Radio disabled={isDisabled} />}
            label={roomType.name}
          />
        ))}
      </RadioGroup>
    </div>
  );
};

export default FilterRoomType;
