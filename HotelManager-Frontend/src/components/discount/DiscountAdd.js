import React, { useState } from "react";
import { createDiscount } from "../../services/api_discount";
import { useNavigate } from "react-router-dom";

const DiscountAdd = () => {
  const navigate = useNavigate();

  const [formData, setFormData] = useState({
    code: "",
    percent: 0,
    validFrom: "",
    validTo: "",
  });

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (window.confirm("Are you sure you want to create this discount?")) {
      try {
        await createDiscount(formData);
        navigate(`/dashboard-discount/`);
      } catch (error) {
        console.error("Error adding discount:", error);
      }
    }
  };

  return (
    <div>
      <h2 className="edit-view-header">Add New Discount</h2>
      <form
        className="discount-add-form edit-view-form"
        onSubmit={handleSubmit}
      >
        <div>
          <label htmlFor="code" className="edit-view-label">
            Code:
          </label>
          <input
            className="edit-view-input"
            type="text"
            id="code"
            name="code"
            value={formData.code}
            onChange={handleChange}
          />
        </div>
        <div>
          <label htmlFor="percent" className="edit-view-label">
            Percent:
          </label>
          <input
            className="edit-view-input"
            type="number"
            id="percent"
            name="percent"
            value={formData.percent}
            onChange={handleChange}
          />
        </div>
        <div>
          <label htmlFor="validFrom" className="edit-view-label">
            Valid From:
          </label>
          <input
            className="edit-view-input"
            type="date"
            id="validFrom"
            name="validFrom"
            value={formData.validFrom}
            onChange={handleChange}
          />
        </div>
        <div>
          <label htmlFor="validTo" className="edit-view-label">
            Valid To:
          </label>
          <input
            className="edit-view-input"
            type="date"
            id="validTo"
            name="validTo"
            value={formData.validTo}
            onChange={handleChange}
          />
        </div>
        <button type="submit" className="edit-view-button">
          Add Discount
        </button>
      </form>
    </div>
  );
};

export default DiscountAdd;
