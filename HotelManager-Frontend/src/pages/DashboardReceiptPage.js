import DataTable from "../components/Common/DataTable";
import Paging from "../components/Common/Paging";
import { useState, useEffect } from "react";
import api_receipt from "../services/api_dashboard_invoice";
import { useNavigate } from "react-router";
import { formatDate } from "../common/HelperFunctions";
import { DashboardReceiptsNavbar } from "../components/navigation/DashboardReceiptsNavbar";
import { useReceiptFilter } from "../context/ReceiptFilterContext";

const DashboardReceiptPage = () => {
  const { filter } = useReceiptFilter();
  const navigate = useNavigate();
  const [receipts, setReceipts] = useState([]);
  const [query, setQuery] = useState({
    filter: {
      minPrice: 0,
      maxPrice: null,
      isPaid: null,
      userEmailQuery: "",
    },
    currentPage: 1,
    pageSize: 10,
    totalPages: 1,
    sortBy: "DateCreated",
    sortOrder: "ASC",
  });

  useEffect(() => {
    const requestQuery = {
      ...query,
      filter: {
        ...filter,
      },
    }
    api_receipt.getAllDashboardInvoice(requestQuery).then((response) => {
      const [data, totalPages] = response;
      setReceipts(
        data.map((receipt) => ({
          ...receipt,
          dateCreated: formatDate(receipt.dateCreated),
          isPaid: receipt.isPaid ? "Yes" : "No",
        }))
      );
      setQuery({
        ...query,
        totalPages,
      });
    });
  }, [query.currentPage, query.sortBy, query.sortOrder, filter]);

  const handlePageChange = (pageNumber) => {
    setQuery({
      ...query,
      currentPage: pageNumber,
    });
  };

  const columns = [
    { key: "invoiceNumber", label: "Receipt number" },
    { key: "dateCreated", label: "Date of creation" },
    { key: "totalPrice", label: "Total price" },
    { key: "userEmail", label: "User email" },
    { key: "isPaid", label: "Is paid" },
  ];

  const handle = [
    {
      label: "Edit",
      onClick: (receipt) => {
        navigate(`/dashboardReceipt/edit/${receipt.id}`);
      },
    },
    {
      label: "View",
      onClick: (receipt) => {
        navigate(`/dashboardReceipt/view/${receipt.id}`);
      },
    },
  ];

  return (
    <div className="dashboard-receipt-page page">
      <DashboardReceiptsNavbar />
      <div className="container">
        <DataTable data={receipts} columns={columns} handle={handle} />
        <Paging
          totalPages={query.totalPages}
          currentPage={query.currentPage}
          onPageChange={handlePageChange}
        />
      </div>
    </div>
  );
};

export default DashboardReceiptPage;
