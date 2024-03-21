import DataTable from "../components/Common/DataTable";
import { useState, useEffect } from "react";
import api_reservation from "../services/api_reservation";
import api_receipt from "../services/api_dashboard_invoice";
import { useNavigate } from "react-router";
import Paging from "../components/Common/Paging";
import { DashboardReservationsNavbar } from "../components/navigation/DashboardReservationsNavbar";
import { formatDate } from "../common/HelperFunctions";
import { useReservationFilter } from "../context/ReservationFilterContext";

const DashBoardReservationsPage = () => {
  const { filter } = useReservationFilter();
  const navigate = useNavigate();
  const [query, setQuery] = useState({
    filter: {},
    currentPage: 1,
    pageSize: 10,
    totalPages: 1,
    sortBy: "CheckInDate",
    sortOrder: "ASC",
  });

  const [queryReceipt, setQueryReceipt] = useState({
    filter: {
      ReservationId: "",
    },
  });

  const [reservations, setReservations] = useState([]);


  useEffect(() => {
    fetch();
  }, [query.currentPage, query.sortBy, query.sortOrder, filter]);

  const fetch = () => {
    const requestQuery = {
      ...query,
      filter: {
        ...filter,
        checkInDate: formatDate(filter.checkInDate),
        checkOutDate: formatDate(filter.checkOutDate),
      },
    };
    api_reservation.getAllReservations(requestQuery).then((response) => {
      const [data, totalPages] = response;
      setReservations(
        data.map((reservation) => ({
          ...reservation,
          checkInDate: formatDate(reservation.checkInDate),
          checkOutDate: formatDate(reservation.checkOutDate),
        }))
      );
      setQuery({
        ...query,
        totalPages,
      });
    });
  };

  const handlePageChange = (newPage) => {
    if (newPage >= 1) {
      setQuery({
        ...query,
        currentPage: newPage,
      });
    }
  };

  const columns = [
    { key: "reservationNumber", label: "Reservation Number" },
    { key: "checkInDate", label: "Start Date" },
    { key: "checkOutDate", label: "End Date" },
    { key: "pricePerNight", label: "Price per night" },
    { key: "userEmail", label: "Email" },
  ];

  const handle = [
    {
      label: "Delete",
      onClick: (item) => handleDeleteReservation(item.id),
    },
    {
      label: "View",
      onClick: (item) => navigate(`/dashboard-reservation/view/${item.id}`),
    },
  ];

  useEffect(() => { }, [queryReceipt]);

  const handleDeleteReservation = async (reservationId) => {
    try {
      const invoicesResponse = await api_receipt.getAllDashboardInvoice({
        filter: {
          ReservationId: reservationId,
        },
      });
      const invoices = invoicesResponse[0];
      if (invoices.length > 0) {
        const invoiceId = invoices[0].id;

        // Prikaži potvrdu korisniku
        const isConfirmed = window.confirm(
          "Are you sure you want to delete this reservation?"
        );
        if (isConfirmed) {
          const isDeleted = await api_reservation.deleteReservation(
            reservationId,
            invoiceId
          );
          if (isDeleted) {
            fetch();
            return true;
          } else {
            console.error("Error deleting reservation:", isDeleted);
            return false;
          }
        } else {
          // Ako korisnik odustane od brisanja
          return false;
        }
      } else {
        console.error("No invoices found for reservationId:", reservationId);
        return false;
      }
    } catch (error) {
      console.error("Error deleting reservation:", error);
      return false;
    }
  };

  return (
    <div className="dashboard-reservations-page page">
      <DashboardReservationsNavbar />
      <div className="container">
        <DataTable data={reservations} columns={columns} handle={handle} />
        <Paging
          totalPages={query.totalPages}
          currentPage={query.currentPage}
          onPageChange={handlePageChange}
        />
      </div>
    </div>
  );
};

export default DashBoardReservationsPage;
