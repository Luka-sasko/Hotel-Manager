import React, { useState, useEffect } from "react";
import { getByIdRoomType, updateRoomType } from "../../services/api_room_type";
import { useNavigate } from "react-router-dom";

const RoomTypeEdit = ({ roomId }) => {
  const [roomType, setRoomType] = useState({});
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchRoomType = async () => {
      try {
        const response = await getByIdRoomType(roomId);
        setRoomType(response.data);
        setLoading(false);
      } catch (error) {
        console.error("Error fetching room type:", error);
      }
    };

    fetchRoomType();
  }, [roomId]);

  const handleChange = (e) => {
    setRoomType({ ...roomType, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    const confirmed = window.confirm(
      "Are you sure you want to update this room type?"
    );
    if (confirmed) {
      try {
        await updateRoomType(roomId, roomType);
        navigate(`/dashboard-roomtype`);
      } catch (error) {
        console.error("Error updating room type:", error);
      }
    }
  };

  if (loading) {
    return <div>Loading...</div>;
  }

  return (
    <div>
      <h2 className="edit-view-header">Edit Room Type</h2>
      <form
        className="roomtype-edit-form edit-view-form"
        id="roomtype-edit-form"
        onSubmit={handleSubmit}
      >
        <div>
          <label htmlFor="name" className="edit-view-label">
            Name:
          </label>
          <input
            className="edit-view-input"
            type="text"
            id="name"
            name="name"
            value={roomType.name || ""}
            onChange={handleChange}
          />
        </div>
        <div>
          <label htmlFor="description" className="edit-view-label">
            Description:
          </label>
          <textarea
            className="edit-view-textarea"
            id="description"
            name="description"
            value={roomType.description || ""}
            onChange={handleChange}
          />
        </div>
        <button type="submit" className="edit-view-button">
          Update Room Type
        </button>
      </form>
    </div>
  );
};

export default RoomTypeEdit;
