import React, { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { getAllDashboardRooms } from "../../services/api_dashboard_room";
import { useRoomFilter } from "../../context/RoomFilterContext";
import { formatDate, formatReservationDate } from "../../common/HelperFunctions";

export const RoomListAdmin = () => {
  const [rooms, setRooms] = useState([]);
  const { filter } = useRoomFilter();
  const [query, setQuery] = useState({
    filter: {},
    currentPage: 1,
    pageSize: 10,
    totalPages: 1,
    sortBy: "Number",
    sortOrder: "ASC",
  });

  const fetchData = async () => {
    try {
      const requestQuery = {
        filter: {
          searchQuery: filter.searchQuery,
          minPrice: filter.minPrice,
          maxPrice: filter.maxPrice,
          minBeds: filter.minBeds,
          roomTypeId: filter.roomTypeId,
        },
      };
      const [roomData, totalPages] = await getAllDashboardRooms(requestQuery);
      setQuery({
        ...query,
        totalPages,
      });
      if (!roomData) {
        setRooms([]);
      }
      setRooms(roomData);
    } catch (error) {
      console.error("Error fetching room:", error);
    }
  };

  useEffect(() => {
    fetchData();
  }, [filter]);

  return (
    <div className="room-list">
      {rooms && rooms.map((room) => (
        <div key={room.id}>
          <Link to={`/dashBoardRoom/${room.id}`} className="room-link">
            <img src={room.imageUrl} alt="room" className="room-link-image" />
            <div className="room-link-info">
              <p className="room-link-number">Room {room.number}</p>
              <p className="room-link-price">{room.price}€/per night</p>
            </div>
          </Link>
        </div>
      ))}
    </div>
  );
};
