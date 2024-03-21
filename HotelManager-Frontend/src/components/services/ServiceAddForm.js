import React from "react";
import { useState } from "react";
import { createService } from "../../services/api_hotel_service";
import { useNavigate } from "react-router-dom";

export const ServiceAddForm = () => {
  const navigate = useNavigate();
  const [service, setServiceData] = useState({
    name: "",
    description: "",
    price: 0,
  });

  const handleChange = (e) => {
    setServiceData({
      ...service,
      [e.target.name]: e.target.value,
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await createService(service);
      navigate("/");
    } catch (error) {
      console.error(error);
    }
  };

  return (
    <div className="service-add">
      <h2 className="service-add-heading edit-view-header">Add Service</h2>
      <form className="service-add-form edit-view-form" onSubmit={handleSubmit}>
        <label className="service-add-form-label edit-view-label">Name:</label>
        <input
          className="service-add-form-input edit-view-input"
          placeholder="Enter service name"
          type="text"
          name="name"
          onChange={handleChange}
        />
        <label className="service-add-form-label edit-view-label">
          Description:
        </label>
        <textarea
          className="service-add-form-input edit-view-textarea"
          placeholder="Enter service description"
          onChange={handleChange}
          name="description"
        />
        <label className="service-add-form-label edit-view-label">Price:</label>
        <input
          className="service-add-form-input edit-view-input"
          placeholder="Enter service price"
          type="number"
          onChange={handleChange}
          name="price"
        />
        <input type="submit" className="edit-view-button" value="Add" />
      </form>
    </div>
  );
};
