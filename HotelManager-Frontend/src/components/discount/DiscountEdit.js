import React, { useState, useEffect } from "react";
import { getByIdDiscount, updateDiscount } from "../../services/api_discount";
import { useNavigate } from "react-router-dom";

const DiscountEdit = ({ discountId }) => {
  const [discount, setDiscount] = useState({});
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchDiscount = async () => {
      try {
        const response = await getByIdDiscount(discountId);
        const { validFrom, validTo, ...rest } = response.data;
        // Formatiraj datume iz odgovora u odgovarajući format "yyyy-MM-dd"
        const formattedValidFrom = new Date(validFrom)
          .toISOString()
          .split("T")[0];
        const formattedValidTo = new Date(validTo).toISOString().split("T")[0];
        setDiscount({
          ...rest,
          validFrom: formattedValidFrom,
          validTo: formattedValidTo,
        });
        setLoading(false);
      } catch (error) {
        console.error("Error fetching discount:", error);
      }
    };

    fetchDiscount();
  }, [discountId]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    if (name === "validFrom" || name === "validTo") {
      // Formatirajte datum u odgovarajući format "yyyy-MM-dd"
      const formattedDate = new Date(value).toISOString().split("T")[0];
      setDiscount({ ...discount, [name]: formattedDate });
    } else {
      setDiscount({ ...discount, [name]: value });
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (window.confirm("Are you sure you want to update this discount?")) {
      try {
        await updateDiscount(discountId, discount);
        navigate(`/dashboard-discount`);
      } catch (error) {
        console.error("Error updating discount:", error);
      }
    }
  };

  if (loading) {
    return <div>Loading...</div>;
  }

  return (
    <div>
      <h2 className="edit-view-header">Edit Discount</h2>
      <form
        className="discount-edit-form edit-view-form"
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
            value={discount.code || ""}
            onChange={handleChange}
          />
        </div>
        <div>
          <label htmlFor="percent" className="edit-view-label">
            Percentage:
          </label>
          <input
            className="edit-view-input"
            type="number"
            id="percent"
            name="percent"
            value={discount.percent || ""}
            onChange={handleChange}
          />
        </div>
        <div>
          <label htmlFor="validFrom" className="edit-view-label">
            Valid from:
          </label>
          <input
            className="edit-view-input"
            type="date"
            id="validFrom"
            name="validFrom"
            value={discount.validFrom || ""}
            onChange={handleChange}
          />
        </div>
        <div>
          <label htmlFor="validTo" className="edit-view-label">
            Valid to:
          </label>
          <input
            className="edit-view-input"
            type="date"
            id="validTo"
            name="validTo"
            value={discount.validTo || ""}
            onChange={handleChange}
          />
        </div>
        <button type="submit" className="edit-view-button">
          Update Discount
        </button>
      </form>
    </div>
  );
};

export default DiscountEdit;
