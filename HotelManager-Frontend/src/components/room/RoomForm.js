import React, { useState, useEffect } from "react";
import { useParams } from "react-router-dom";
import {
  updateDashboardRoom,
  getDashboardRoomUpdateById,
  createDashboardRoom,
} from "../../services/api_dashboard_room";
import RoomType from "../filter/FilterRoomTypes";

export default function RoomForm() {
  const { id } = useParams();
  const [mode, setMode] = useState("view");
  const [roomData, setRoomData] = useState({
    imageUrl: "",
    number: "",
    bedCount: 0,
    price: 0,
    typeId: "",
    isActive: true,
  });

  useEffect(() => {
    if (id) {
      getDashboardRoomUpdateById(id)
        .then((response) => {
          setRoomData(response.data);
        })
        .catch((error) => console.error("Error fetching room details:", error));
    } else {
      setMode("add");
    }
  }, [id]);

  const handleFormSubmit = (e) => {
    e.preventDefault();
    if (mode === "add") {
      createDashboardRoom(roomData)
        .then(() => handleSuccess())
        .catch((error) => console.error("Error creating room:", error));
    } else if (mode === "edit") {
      updateDashboardRoom(id, roomData)
        .then(() => handleSuccess())
        .catch((error) => console.error("Error updating room:", error));
      setMode("view");
    }
  };

  const handleSuccess = () => {
    console.log("Room operation successful!");
  };

  const handleRoomTypeChange = (selectedRoomType) => {
    setRoomData({
      ...roomData,
      typeId: selectedRoomType,
    });
  };

  const handleEdit = () => {
    setMode("edit");
  };

  return (
    <div>
      <br />
      <form
        id="roomForm"
        className="edit-view-form"
        onSubmit={handleFormSubmit}
      >
        <label htmlFor="imageUrl" className="edit-view-label">
          Image url:{" "}
        </label>
        <input
          className="edit-view-input"
          type="text"
          id="imageUrl"
          name="imageUrl"
          value={roomData.imageUrl}
          disabled={mode === "view"}
          onChange={(e) =>
            setRoomData({ ...roomData, imageUrl: e.target.value })
          }
        />

        <label htmlFor="roomNumber" className="edit-view-label">
          Room number:{" "}
        </label>
        <br />
        <input
          className="edit-view-input"
          type="text"
          id="roomNumber"
          name="roomNumber"
          value={roomData.number}
          disabled={mode === "view"}
          onChange={(e) => setRoomData({ ...roomData, number: e.target.value })}
        />
        <br />
        <br />

        <label className="NumberOfBedsRow edit-view-label">
          <span>Number of beds:</span>
          <br />
          <br />
          <input
            className="edit-view-input"
            type="text"
            value={roomData.bedCount}
            disabled={mode === "view"}
            onChange={(e) =>
              setRoomData({ ...roomData, bedCount: e.target.value })
            }
          />
        </label>
        <br />
        <label htmlFor="price" className="edit-view-label">
          Price for room:{" "}
        </label>
        <br />
        <input
          className="edit-view-input"
          type="number"
          id="price"
          name="price"
          value={roomData.price}
          disabled={mode === "view"}
          onChange={(e) => setRoomData({ ...roomData, price: e.target.value })}
        />

        <RoomType
          onChangeHandle={handleRoomTypeChange}
          isDisabled={mode === "view"}
        />

        <label className="edit-view-label">
          Is Active:
          <input
            className="edit-view-input"
            type="value"
            name="isActive"
            value={roomData.isActive}
            disabled={mode === "view"}
            onChange={(e) =>
              setRoomData({ ...roomData, isActive: e.target.value })
            }
          />
        </label>

        <br />
        {mode === "view" && (
          <button
            className="edit-view-button"
            type="button"
            onClick={handleEdit}
          >
            Edit
          </button>
        )}
        {mode === "edit" && (
          <button className="edit-view-button" type="submit">
            {mode === "view" ? "Edit" : "Finish"}
          </button>
        )}
        {mode === "add" && (
          <button className="edit-view-button" type="submit">
            {mode === "view" ? "Edit" : "Finish"}
          </button>
        )}
      </form>
    </div>
  );
}
