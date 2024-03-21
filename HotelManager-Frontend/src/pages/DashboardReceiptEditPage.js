import DataTable from "../components/Common/DataTable";
import { useState, useEffect } from "react";
import api_service_invoice from "../services/api_service_invoice";
import { getAllServices } from "../services/api_hotel_service";
import { useParams } from "react-router-dom";
import { formatDate } from "../common/HelperFunctions";
import { DashboardEditViewNavbar } from "../components/navigation/DashboardEditViewNavbar";

const DashboardReceiptEditPage = () => {
  const { receiptId } = useParams();
  const [services, setServices] = useState([]);
  const [serviceId, setServiceId] = useState("");
  const [quantity, setQuantity] = useState(0);
  const [invoiceServices, setinvoiceServices] = useState([]);

  const servicesQuery = {
    filter: {},
    sortBy: "Name",
    sortOrder: "ASC",
  };
  const invoiceServicesQuery = {
    filter: {
      id: receiptId,
    },
    sortBy: "DateCreated",
    sortOrder: "ASC",
  };

  useEffect(() => {
    fetchServices();
  }, []);

  const fetchServices = async () => {
    const [servicesData] = await getAllServices(servicesQuery);
    setServices(servicesData);
  };

  useEffect(() => {
    api_service_invoice.getByInvoiceId(invoiceServicesQuery).then((data) => {
      if (!data) {
        return;
      }
      setinvoiceServices(
        data.map((service) => ({
          ...service,
          dateCreated: formatDate(service.dateCreated),
        }))
      );
    });
  }, [serviceId]);

  const columns = [
    { key: "serviceName", label: "Service name" },
    { key: "quantity", label: "Quantity" },
    { key: "dateCreated", label: "Date" },
  ];

  const handleAddService = () => {
    if (!serviceId) {
      alert("Service must be selected");
      return;
    } else if (quantity <= 0) {
      alert("Quantity must be greater than 0");
      return;
    }
    const data = {
      serviceId,
      quantity,
      invoiceId: receiptId,
    };
    api_service_invoice.createServiceInvoice(data).then(() => {
      setQuantity(0);
      setServiceId("");
    });
  };

  return (
    <div className="receipt-edit-page page">
      <DashboardEditViewNavbar />
      <div className="container edit-view-form">
        <h2 className="dashboard-receipt-edit-title edit-view-header">
          Edit receipt
        </h2>
        <div className="dashboard-receipt-edit-input-service">
          <label
            className="dashboard-receipt-edit-input-service-label edit-view-label"
            htmlFor="service"
          >
            Service:
          </label>
          <select
            className="dashboard-receipt-edit-input-service-select edit-view-input"
            id="service"
            name="service"
            value={serviceId}
            onChange={(e) => setServiceId(e.target.value)}
          >
            <option value="" className="edit-view-option">
              Select a service
            </option>
            {services.map((service) => (
              <option
                key={service.id}
                value={service.id}
                className="edit-view-option"
              >
                {service.name}
              </option>
            ))}
          </select>
          <label
            className="dashboard-receipt-edit-input-quantity-lable edit-view-label"
            htmlFor="quantity"
          >
            Quantity:
          </label>
          <input
            className="dashboard-receipt-edit-input-quantity-input edit-view-input"
            type="number"
            id="quantity"
            name="quantity"
            value={quantity}
            onChange={(e) => setQuantity(e.target.value)}
          />
          <button
            className="dashboard-receipt-edit-input-add-button edit-view-button"
            onClick={handleAddService}
          >
            Add
          </button>
        </div>
        <div className="dashboard-receipt-edit-display-invoice-services">
          <h3>Service history</h3>
          <DataTable data={invoiceServices} columns={columns} />
        </div>
      </div>
    </div>
  );
};

export default DashboardReceiptEditPage;
