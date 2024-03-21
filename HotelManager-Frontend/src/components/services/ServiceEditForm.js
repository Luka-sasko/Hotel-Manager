import React, { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import {
  getByIdService,
  updateService,
} from "../../services/api_hotel_service";

export const ServiceEditForm = () => {
  const navigate = useNavigate();
  const { serviceId } = useParams();
  const [service, setService] = useState({
    name: "",
    description: "",
    price: 0,
  });

  const fetchData = async () => {
    const res = await getByIdService(serviceId);
    const data = res.data;
    setService(data);
  };

  useEffect(() => {
    fetchData();
  }, []);

  const handleChange = (e) => {
    setService({ ...service, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await updateService(serviceId, service);

      navigate("/dashboardServices");
    } catch (error) {
      console.error(error);
    }
  };

  return (
    <div className="service-edit">
      <h2 className="service-edit-heading edit-view-header">Edit Service</h2>
      <form
        className="service-edit-form edit-view-form"
        onSubmit={handleSubmit}
      >
        <label className="service-edit-form-label edit-view-label">Name:</label>
        <input
          className="service-edit-form-input edit-view-input"
          type="text"
          id="name"
          name="name"
          value={service.name}
          onChange={handleChange}
        />
        <label className="service-edit-form-label edit-view-label">
          Description:
        </label>
        <textarea
          className="service-edit-form-input edit-view-textarea"
          id="description"
          name="description"
          value={service.description}
          onChange={handleChange}
        />
        <label className="service-edit-form-label edit-view-label">
          Price:
        </label>
        <input
          className="service-edit-form-input edit-view-input"
          type="number"
          id="price"
          name="price"
          value={service.price}
          onChange={handleChange}
        />
        <input type="submit" className="edit-view-button" value="Apply" />
      </form>
    </div>
  );
};
