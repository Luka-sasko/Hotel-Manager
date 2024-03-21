import React, { useState } from "react";

const SearchInput = ({ searchQuery, onChange }) => {

  const handleSubmit = (event) => {
    event.preventDefault();
    alert("Searching \t" + searchQuery);
    //onSearch(searchQuery);
  };

  return (
    <form className="FilterSearchQuery filter" onSubmit={handleSubmit}>
      <input
        className="filter-input"
        type="text"
        value={searchQuery}
        onChange={onChange}
        placeholder="Search by email..."
      />
      <button type="submit" className="submit-filter">
        Search
      </button>
    </form>
  );
};

export default SearchInput;
