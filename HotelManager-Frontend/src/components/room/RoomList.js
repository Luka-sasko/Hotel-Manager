import React, { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { getAllRooms } from "../../services/api_room";
import Paging from "../Common/Paging";
import { useRoomFilter } from "../../context/RoomFilterContext";
import {
  formatDate,
  formatReservationDate,
} from "../../common/HelperFunctions";

export const RoomList = () => {
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
        ...query,
        filter: {
          ...filter,
          startDate: formatReservationDate(formatDate(filter.startDate), 13),
          endDate: formatReservationDate(formatDate(filter.endDate), 10),
        },
      };
      const [roomsData, totalPages] = await getAllRooms(requestQuery);
      setQuery({
        ...query,
        totalPages,
      });
      if (!roomsData) {
        setRooms([]);
      }
      setRooms(roomsData);
    } catch (error) {
      console.error("Error fetching room:", error);
    }
  };

  useEffect(() => {
    fetchData();
  }, [query.currentPage, filter]);

  const handlePageChange = (pageNumber) => {
    setQuery({
      ...query,
      currentPage: pageNumber,
    });
  };

  return (
    <div className="room-list">
      {rooms &&
        rooms.map((room) => (
          <div key={room.id}>
            <Link
              to={`/room/${room.id}?startDate=${formatDate(
                filter.startDate
              )}&endDate=${formatDate(filter.endDate)}`}
              className="room-link"
            >
              <img src={room.imageUrl} alt="room" className="room-link-image" />
              <div className="room-link-info">
                <p className="room-link-number">Room {room.number}</p>
                <p className="room-link-price">{room.price}€/per night</p>
              </div>
            </Link>
          </div>
        ))}
      <Paging
        totalPages={query.totalPages}
        currentPage={query.currentPage}
        onPageChange={handlePageChange}
      />
    </div>
  );
};
