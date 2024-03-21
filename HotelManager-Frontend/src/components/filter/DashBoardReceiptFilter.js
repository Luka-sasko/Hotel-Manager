import React from "react";
import PriceRange from "./FilterPriceRange.js";
import IsPaidReceipt from "./FilterIsPaidReceipt.js";
import SearchQuery from "./FilterSearchQuery.js";
import { useReceiptFilter } from "../../context/ReceiptFilterContext.js";

const DashBoardReceiptFilter = () => {
  const { filter, setFilter } = useReceiptFilter();

  const handlePriceRangeChange = (e) => {
    const value = parseInt(e.target.value);
    setFilter((prev) => ({
      ...prev,
      [e.target.id]: value,
    }));
  };

  const handleIsPaidChange = (value) => {
    setFilter((prev) => ({
      ...prev,
      isPaid: value,
    }));
  };

  const handleSearchQueryChange = (e) => {
    setFilter((prev) => ({
      ...prev,
      userEmailQuery: e.target.value,
    }));
  };

  return (
    <div className="DashBoardReceiptFilter filter">
      <PriceRange
        minValue={filter.minPrice}
        maxValue={filter.maxPrice}
        onChange={handlePriceRangeChange}
      />
      <IsPaidReceipt onChange={handleIsPaidChange} />
      <SearchQuery onChange={handleSearchQueryChange} />
    </div>
  );
};

export default DashBoardReceiptFilter;
