import React from "react";

const Paging = ({ totalPages, currentPage, onPageChange }) => {
  const pageNumbers = Array.from({ length: totalPages }, (_, i) => i + 1);

  return (
    <div className="paging">
      {pageNumbers.map((pageNumber) => (
        <button
          className="paging-button"
          key={pageNumber}
          onClick={() => onPageChange(pageNumber)}
          disabled={pageNumber === currentPage}
        >
          {pageNumber}
        </button>
      ))}
    </div>
  );
};

export default Paging;
