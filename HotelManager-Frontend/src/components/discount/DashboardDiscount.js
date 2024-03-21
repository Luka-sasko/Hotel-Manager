import { useNavigate } from "react-router-dom";
import { useState, useEffect } from "react";
import { format } from "date-fns";
import DataTable from "../Common/DataTable";
import { deleteDiscount, getAllDiscounts } from "../../services/api_discount";
import Paging from "../Common/Paging";
import { useDiscountFilter } from "../../context/DiscountFilterContext";

export const DashboardDiscount = () => {
  const { filter } = useDiscountFilter();
  const [discounts, setDiscounts] = useState([]);
  const navigate = useNavigate();
  const [query, setQuery] = useState({
    currentPage: 1,
    pageSize: 10,
    totalPages: 1,
    sortBy: "DateCreated",
    sortOrder: "DESC",
  });

  useEffect(() => {
    const fetchData = async () => {
      const requestQuery = {
        ...query,
        filter: {
          ...filter,
        },
      };
      try {
        const [items, totalPages] = await getAllDiscounts(requestQuery);
        if (items && Array.isArray(items)) {
          const formattedDiscounts = items.map(discount => ({
            ...discount,
            validFrom: format(new Date(discount.validFrom), 'dd-MM-yyyy'),
            validTo: format(new Date(discount.validTo), 'dd-MM-yyyy')
          }));
          setDiscounts(formattedDiscounts);
          setQuery((prevQuery) => ({
            ...prevQuery,
            totalPages
          }));
        } else {
          console.error("Unexpected discount data format:", items);
        }
      } catch (error) {
        console.error("Error fetching room types:", error);
      }
    };
  
    fetchData();
  }, [query.currentPage, filter]);
  
  const columns = [
    { key: "code", label: "Code" },
    { key: "percent", label: "Percentage" },
    { key: "validFrom", label: "Valid From" },
    { key: "validTo", label: "Valid To" },
  ];

  const handle = [
    {
      label: "Delete",
      onClick: (row) => {
        const confirmed = window.confirm("Are you sure you want to delete this discount?");
        if (confirmed) 
        {
          deleteDiscount(row.id)
            .then(() => {
              window.location.reload();
            })
            .catch((error) => {
              console.error("Error deleting discount:", error);
            });
        }
      },
    },
    {
      label: "Edit",
      onClick: (row) => {
        navigate(`/dashboard-discount/${row.id}`);
      },
    },
  ];

  const handlePageChange = (newPage) => {
    if (newPage >= 1) {
      setQuery({
        ...query,
        currentPage: newPage
      });
    }
  }

  return (
    <div className="discount-list">
      <DataTable data={discounts} columns={columns} handle={handle} />
      <Paging
        currentPage={query.currentPage}
        totalPages={query.totalPages}
        onPageChange={handlePageChange}
      />
    </div>
  );
};
