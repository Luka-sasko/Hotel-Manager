import DataTable from "../components/Common/DataTable";
import { NavBar } from "../components/Common/NavBar";
import { useState, useEffect } from "react";
import api_reservation from "../services/api_reservation";
import { getUser } from "../services/api_user";
import { useNavigate } from "react-router";
import Paging from "../components/Common/Paging";
import { formatDate } from "../common/HelperFunctions";
import { DashboardEditViewNavbar } from "../components/navigation/DashboardEditViewNavbar";

const MyReservationsPage = () => {
  const navigate = useNavigate();
  const [query, setQuery] = useState({
    filter: {
      userId: "",
    },
    currentPage: 1,
    pageSize: 10,
    totalPages: 1,
    sortBy: "CheckInDate",
    sortOrder: "ASC",
  });

  const [reservations, setReservations] = useState([]);

  useEffect(() => {
    try {
      getUser().then((user) => {
        setQuery((prevQuery) => ({
          ...prevQuery,
          filter: {
            userId: user.data.id,
          },
        }));
      });
    } catch (error) {
      console.error(error);
    }
  }, []);

  useEffect(() => {
    if (!query.filter.userId) return;
    api_reservation.getAllReservations(query).then((response) => {
      const [data, totalPages] = response;
      setReservations(
        data.map((reservation) => ({
          ...reservation,
          checkInDate: formatDate(reservation.checkInDate),
          checkOutDate: formatDate(reservation.checkOutDate),
        }))
      );
      setQuery((prevQuery) => ({
        ...prevQuery,
        totalPages,
      }));
    });
  }, [query.filter.userId, query.currentPage, query.sortBy, query.sortOrder]);

  const handlePageChange = (newPage) => {
    if (newPage >= 1) {
      setQuery({
        ...query,
        currentPage: newPage,
      });
    }
  };

  const handleReviewClick = (item) => {
    const currentDate = new Date();
    const checkOutDate = new Date(item.checkOutDate);
    if (currentDate > checkOutDate) {
      navigate(`/addreview/${item.roomId}`);
    } else {
      alert("You cannot review this reservation yet.");
    }
  };

  const columns = [
    { key: "roomNumber", label: "Room Number" },
    { key: "checkInDate", label: "Start Date" },
    { key: "checkOutDate", label: "End Date" },
    { key: "pricePerNight", label: "Price per night" },
  ];

  const handle = [
    {
      label: "Review it!",
      onClick: handleReviewClick,
    },
  ];

  return (
    <div className="my-reservations-page page">
      <NavBar />
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

export default MyReservationsPage;
