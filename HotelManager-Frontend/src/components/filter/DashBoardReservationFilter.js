import React from "react";
import DateSelectUser from "./FilterDateSelectUser.js";
import PriceRange from "./FilterPriceRange.js";
import SearchQuery from "./FilterSearchQuery.js";
import { useReservationFilter } from "../../context/ReservationFilterContext.js";
import { formatDate } from "../../common/HelperFunctions.js";

const DashBoardReservationFilter = () => {
  const { filter, setFilter } = useReservationFilter();

  const handleDateChange = (dates) => {
    const [start, end] = dates;
    setFilter((prev) => ({
      ...prev,
      checkInDate: formatDate(start),
      checkOutDate: formatDate(end),
    }));
  };

  const handlePriceRangeChange = (e) => {
    const value = parseInt(e.target.value);
    setFilter((prev) => ({
      ...prev,
      [e.target.id]: value,
    }));
  };

  const handleSearchQueryChange = (e) => {
    setFilter((prev) => ({
      ...prev,
      searchQuery: e.target.value,
    }));
  };

  return (
    <div className="DashBoardReservationFilter filter">
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
      <SearchQuery
        userEmailQuery={filter.userEmailQuery}
        onChange={handleSearchQueryChange}
      />
    </div>
  );
};

export default DashBoardReservationFilter;
