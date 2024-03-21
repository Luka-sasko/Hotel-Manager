import { useParams } from "react-router-dom";
import api_dashboard_invoice from "../services/api_dashboard_invoice";
import api_service_invoice from "../services/api_service_invoice";
import { useState, useEffect } from "react";
import { formatDate, formatCurrency } from "../common/HelperFunctions";
import ServiceList from "../components/receipt/ReceiptServiceList";
import { useNavigate } from "react-router-dom";
import { DashboardEditViewNavbar } from "../components/navigation/DashboardEditViewNavbar";

const DashboardReceiptViewPage = () => {
  const navigation = useNavigate();
  const { receiptId } = useParams();
  const [receipt, setReceipt] = useState(null);
  const [services, setServices] = useState([]);
  const query = {
    filter: {
      id: receiptId,
    },
    sortBy: "DateCreated",
    sortOrder: "ASC",
    pageSize: 100,
    pageNumber: 1,
  };

  const fetchReceipt = async () => {
    const data = await api_dashboard_invoice.getByIdDashboardInvoice(receiptId);
    if (!data) {
      console.error("Error fetching receipt");
      return;
    }
    setReceipt(data);
  };

  const fetchServices = async () => {
    const data = await api_service_invoice.getByInvoiceId(query);
    if (!data) {
      console.error("Error fetching services");
      return;
    }
    setServices(data);
  };

  useEffect(() => {
    fetchReceipt();
    fetchServices();
  }, []);

  const calculateNumberOfNights = (checkInDate, checkOutDate) => {
    const oneDayMilliseconds = 24 * 60 * 60 * 1000;
    const startDate = new Date(checkInDate);
    const endDate = new Date(checkOutDate);

    const numberOfNights = Math.round(
      (endDate - startDate) / oneDayMilliseconds
    );
    return numberOfNights;
  };

  const subtotalServicePrice = services.reduce(
    (total, service) => total + service.price * service.quantity,
    0
  );
  const totalDiscountedPrice = receipt
    ? receipt.totalPrice * (1 - receipt.percent / 100)
    : 0;
  const totalPrice =
    (totalDiscountedPrice || receipt?.totalPrice || 0) + subtotalServicePrice;

  const handleSendReceipt = async () => {
    try {
      const isSent = await api_dashboard_invoice.sendDashboardInvoice(
        receiptId
      );
      if (isSent) {
        alert("Receipt sent successfully");
      } else {
        alert("Error sending receipt");
      }
    } catch (error) {
      console.error("Error sending receipt", error);
    }
  };

  const handleEditReceipt = () => {
    navigation(`/dashboardReceipt/edit/${receiptId}`);
  };

  return (
    <div className="dashboard-receipt-view-page page">
      <DashboardEditViewNavbar />
      <div className="container">
        <h2 className="dashboard-receipt-view-title edit-view-header">
          View receipt
        </h2>
        <div className="dashboard-receipt-view-info">
          <h3 className="dashboard-receipt-view-info-title">
            Receipt Overview
          </h3>
          {receipt && (
            <div
              style={{
                display: "flex",
                flexDirection: "column",
                gap: "0.2rem",
              }}
            >
              <p>Receipt Number: {receipt.invoiceNumber}</p>
              <p>Issue Date: {formatDate(new Date())}</p>
              <p>Check-In Date: {formatDate(receipt.checkInDate)}</p>
              <p>Check-Out Date: {formatDate(receipt.checkOutDate)}</p>
              <p>Room Number: {receipt.roomNumber}</p>
              <h4>Guest Information:</h4>
              <p>Name: {`${receipt.firstName} ${receipt.lastName}`}</p>
              <p>Email: {receipt.email}</p>
              <h4>Reservation Details:</h4>
              <p>Reservation number: {receipt.reservationNumber}</p>
              <p>
                Number of Nights:{" "}
                {calculateNumberOfNights(
                  receipt.checkInDate,
                  receipt.checkOutDate
                )}
              </p>
              <p>Price per Night: {formatCurrency(receipt.pricePerNight)}</p>
              <p>Subtotal: {formatCurrency(receipt.totalPrice)}</p>
              {receipt.discountId && (
                <>
                  <p>
                    Discount: {receipt.code} ({receipt.percent}%)
                  </p>
                  <p>
                    Subtotal with discount:{" "}
                    {formatCurrency(
                      receipt.totalPrice * (1 - receipt.percent / 100)
                    )}
                  </p>
                </>
              )}
              {services.length > 0 && (
                <>
                  <ServiceList services={services} />
                  <p>Subtotal: {formatCurrency(subtotalServicePrice)}</p>
                </>
              )}

              <h4>Total Price:</h4>
              <p>{formatCurrency(totalPrice)}</p>
            </div>
          )}
        </div>
        <div className="dashboard-receipt-view-buttons">
          <button
            className="dashboard-receipt-view-button-paid edit-view-button"
            onClick={handleSendReceipt}
          >
            Paid
          </button>
          <button
            className="dashboard-receipt-view-button-edit edit-view-button"
            onClick={handleEditReceipt}
          >
            Edit
          </button>
        </div>
      </div>
    </div>
  );
};

export default DashboardReceiptViewPage;
