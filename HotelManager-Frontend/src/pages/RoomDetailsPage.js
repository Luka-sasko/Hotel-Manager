import React, { useEffect, useState, } from "react";
import { createSearchParams, useLocation, useParams } from "react-router-dom";
import { useNavigate } from 'react-router-dom';
import { getByIdRoom } from "../services/api_room";
import { getAllDiscounts } from "../services/api_discount";
import { formatReservationDate } from "../common/HelperFunctions";
import apiReservation from "../services/api_reservation";
import Reviews from "../components/review/Reviews";
import { getUserRole } from "../services/api_user";

export const RoomDetailsPage = () => {
  const { id } = useParams();
  const [room, setRoom] = useState(null);
  const [discount, setDiscount] = useState([""]);
  const [query, setQuery] = useState({
    filter: {
      code: "",
    },
    currentPage: 1,
    pageSize: 1,
    sortBy: "DateCreated",
    sortOrder: "ASC",
  });
  const location = useLocation();
  const queryParams = new URLSearchParams(location.search);
  const startDate = queryParams.get("startDate");
  const endDate = queryParams.get("endDate");
  const [userRole, setUserRole] = useState("");
 

  useEffect(() => {
    const fetchRoom = async () => {
      try {
        const roomData = await getByIdRoom(id);
        setRoom(roomData.data);
      } catch (error) {
        console.error("Error fetching room:", error);
      }
    };

    fetchRoom();
  }, [id]);

  useEffect(() => {
    const fetchUserRole = async () => {
      try {
        const role = await getUserRole();
        setUserRole(role);
      } catch (error) {
        console.error("Error fetching user role: ", error);
      }
    };

    fetchUserRole();
  }, []);

  useEffect(() => { }, [query]);

  const fetchDiscounts = async () => {
    try {
      const [discountsData, totalPages] = await getAllDiscounts(query);
      setDiscount(discountsData);
    } catch (error) {
      console.error("Error fetching discounts:", error);
      setDiscount([]);
    }
  };

  const handleQueryChange = (discountCode) => {
    setQuery({ ...query, filter: { code: discountCode } });
  };


  const navigate = useNavigate();

  const handleReserve = () => {
    const token = localStorage.getItem("token");

    if (token != null) {
      const isConfirmed = window.confirm(
        "Are you sure you want to make this reservation?"
      );
      if (isConfirmed) {
        const appliedDiscount = discount[0];
        if (appliedDiscount) {
        }
        const reservationData = {
          discountId: appliedDiscount ? appliedDiscount.id : null,
          roomId: room.id,
          pricePerNight: room.price,
          checkInDate: formatReservationDate(startDate, 13),
          checkOutDate: formatReservationDate(endDate, 10),
        };

        apiReservation.createReservation(reservationData);
      }
      else{
        return false;
      }
    }
    else {
      navigate("/login");

    }
  };

  if (!room) {
    return <div>Loading...</div>;
  }
  return (
    <div className="room-detail">
      <div className="room-info-section">
        <img src={room.imageUrl} alt="room" className="room-detail-image" />
        <div className="room-detail-info">
          <p className="room-detail-number">Room number: {room.number}</p>
          <p className="room-detail-number">Price per day: {room.price}€</p>
          <p className="room-detail-number">
            Number of beds: {room.bedCount}🛏️
          </p>
          <p className="room-detail-number">Room type: {room.typeName}</p>
        </div>
      </div>
      {userRole !== "Admin" && (
        <div className="discount-reservation-form">
          <div className="reservation-data">
            <p>Reservaton start date: {startDate}</p>
            <p>Reservation end date: {endDate}</p>
          </div>
          <div className="discount-form">
            <input
              className="discount-form-input"
              type="text"
              placeholder="Enter discount code"
              value={query.filter.code}
              onChange={(e) => handleQueryChange(e.target.value)}
            />
            <button onClick={fetchDiscounts} className="discount-form-button">
              Apply
            </button>
            <br />
            <button onClick={handleReserve} className="discount-form-button">
              Reserve
            </button>
          </div>
        </div>
      )}

      <Reviews id={id} />
    </div>
  );
};
