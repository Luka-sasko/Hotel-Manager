import React from "react";
import DateSelectUser from "./FilterDateSelectUser.js";
import PriceRange from "./FilterPriceRange.js";
import NumberOfBeds from "./FilterNumberOfBeds.js";
import RoomTypes from "./FilterRoomTypes.js";
import { useRoomFilter } from "../../context/RoomFilterContext.js";

const RoomFilter = () => {
  const { filter, setFilter } = useRoomFilter();

  const handleDateChange = (dates) => {
    const [start, end] = dates;
    setFilter((prev) => ({
      ...prev,
      startDate: start,
      endDate: end,
    }));
  };

  const handlePriceRangeChange = (e) => {
    const value = parseInt(e.target.value);
    setFilter((prev) => ({
      ...prev,
      [e.target.id]: value,
    }));
  };

  const handleNumberOfBedsChange = (e) => {
    const value = parseInt(e.target.value);
    setFilter((prev) => ({
      ...prev,
      minBeds: value,
    }));
  };

  const handleRoomTypeChange = (value) => {
    setFilter((prev) => ({
      ...prev,
      roomTypeId: value,
    }));
  };

  return (
    <div className="room-filter filter">
      <DateSelectUser
        startDate={filter.startDate}
        endDate={filter.endDate}
        onChange={handleDateChange}
      />
      <PriceRange
        minValue={filter.minPrice}
        maxValue={filter.maxPrice}
        onChange={handlePriceRangeChange}
      />
      <NumberOfBeds
        numberOfBeds={filter.minBeds}
        handleChange={handleNumberOfBedsChange}
      />
      <RoomTypes onChangeHandle={handleRoomTypeChange} />
    </div>
  );
};

export default RoomFilter;
