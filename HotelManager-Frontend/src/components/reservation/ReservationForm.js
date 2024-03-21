import React, { useState, useEffect } from "react";
import { useParams } from "react-router-dom";
import apiReservation from "../../services/api_reservation";
import api_receipt from "../../services/api_dashboard_invoice";
export default function ReservationForm() {
  const { id } = useParams();
  const [mode, setMode] = useState("view");
  const [reservationData, setReservationData] = useState({
    checkInDate: "",
    checkOutDate: "",
    pricePerNight: 0,
    reservationNumber: "",
  });

  useEffect(() => {
    if (id) {
      setMode("view");
      apiReservation
        .getByIdReservation(id)
        .then(async (response) => {
          const reservation = response.data;
          setReservationData({
            ...reservation,
            checkInDate: formatDate(reservation.checkInDate),
            checkOutDate: formatDate(reservation.checkOutDate),
          });
        })
        .catch((error) =>
          console.error("Error fetching reservation details:", error)
        );
    }
  }, [id]);

  const handleFormSubmit = async (e) => {
    e.preventDefault();
    if (mode === "edit") {
    }
    try {
      const invoicesResponse = await api_receipt.getAllDashboardInvoice({
        filter: {
          ReservationId: id,
        },
      });
      const invoices = invoicesResponse[0];
      if (invoices && invoices.length > 0) {
        const invoiceId = invoices[0].id;
        const isUpdated = await apiReservation.updateReservation(
          id,
          invoiceId,
          reservationData
        );
        if (isUpdated) {
          handleSuccess();
          setMode("view");
        } else {
          console.error("Error deleting reservation:", isUpdated);
        }
      } else {
        console.error("No invoices found for reservationId:", id);
      }
    } catch (error) {
      console.error("Error deleting reservation:", error);
    }
  };

  const handleSuccess = () => {
    console.log("Reservation operation successful!");
  };

  const handleEdit = () => {
    setMode("edit");
  };

  const formatDate = (datetime) => {
    if (!datetime) return "";
    const date = new Date(datetime);
    return date.toISOString().split("T")[0];
  };
  return (
    <div>
      <br />
      <form id="reservationForm edit-view-form" onSubmit={handleFormSubmit}>
        <label htmlFor="checkInDate" className="edit-view-label">
          Check-In Date:{" "}
        </label>
        <br />
        <input
          className="edit-view-input"
          type="date"
          id="checkInDate"
          name="checkInDate"
          value={reservationData.checkInDate}
          disabled={mode === "view"}
          onChange={(e) =>
            setReservationData({
              ...reservationData,
              checkInDate: e.target.value,
            })
          }
        />
        <br />
        <br />

        <label htmlFor="checkOutDate" className="edit-view-label">
          Check-Out Date:{" "}
        </label>
        <br />
        <input
          className="edit-view-input"
          type="date"
          id="checkOutDate"
          name="checkOutDate"
          value={reservationData.checkOutDate}
          disabled={mode === "view"}
          onChange={(e) =>
            setReservationData({
              ...reservationData,
              checkOutDate: e.target.value,
            })
          }
        />
        <br />
        <br />

        <label htmlFor="pricePerNight" className="edit-view-label">
          Price per Night:{" "}
        </label>
        <br />
        <input
          className="edit-view-input"
          type="number"
          id="pricePerNight"
          name="pricePerNight"
          value={reservationData.pricePerNight}
          disabled={mode === "view"}
          onChange={(e) =>
            setReservationData({
              ...reservationData,
              pricePerNight: e.target.value,
            })
          }
        />
        <br />
        <br />

        <br />
        {mode === "view" && (
          <button
            type="button"
            className="edit-view-button"
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
      </form>
    </div>
  );
}
