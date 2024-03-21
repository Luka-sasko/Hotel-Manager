import React from "react";
import PriceRange from "../filter/FilterPriceRange";
import { Link } from "react-router-dom";
import DashboardNavigation from "./DashboardNavigation";
import { useDiscountFilter } from "../../context/DiscountFilterContext";

export const DashboardHomeDiscountsNavbar = () => {
  const { filter, setFilter } = useDiscountFilter();

  const handlePriceRangeChange = (e) => {
    const value = parseInt(e.target.value);
    setFilter((prev) => ({
      ...prev,
      [e.target.id]: value,
    }));
  };

  return (
    <div className="navbar">
      <DashboardNavigation />
      <PriceRange
        minValue={filter.startingValue}
        maxValue={filter.endValue}
        onChange={handlePriceRangeChange}
        minId="startingValue"
        maxId="endValue"
      />
      <Link to="/dashboard-discount/add" className="add-link">
        Add discount
      </Link>
    </div>
  );
};
