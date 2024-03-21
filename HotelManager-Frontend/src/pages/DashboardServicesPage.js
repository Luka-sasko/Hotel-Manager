import { getAllServices } from "../services/api_hotel_service";
import { useNavigate } from "react-router-dom";
import { useState, useEffect } from "react";
import { NavBar } from "../components/Common/NavBar";
import DataTable from "../components/Common/DataTable";
import { deleteService } from "../services/api_hotel_service";
import Paging from "../components/Common/Paging";
import { DashboardServicesHomeNavbar } from "../components/navigation/DashboardServicesHomeNavbar";
import { useServicesFilter } from "../context/ServicesFilterContext";

export const DashboardServicesPage = () => {
  const { filter } = useServicesFilter();
  const [services, setServices] = useState([]);
  const navigate = useNavigate();
  const [query, setQuery] = useState({
    filter: {},
    currentPage: 1,
    pageSize: 10,
    totalPages: 1,
    sortBy: "Name",
    sortOrder: "ASC",
  });

  const fetchData = async () => {
    const requestQuery = {
      ...query,
      filter: {
        ...filter,
      },
    };
    try {
      const [servicesData, newTotalPages] = await getAllServices(requestQuery);
      [...servicesData].map((service) => {
        service.price = service.price + "€";
      });
      setServices([...servicesData]);
      setQuery((prev) => ({ ...prev, totalPages: newTotalPages }));
    } catch (error) {
      console.error("Error fetching service:", error);
    }
  };

  useEffect(() => {
    fetchData();
  }, [query.currentPage, filter]);

  const columns = [
    { key: "name", label: "Service name" },
    { key: "description", label: "Description" },
    { key: "price", label: "Price" },
  ];

  const handle = [
    {
      label: "Delete",
      onClick: async (row) => {
        await deleteService(row.id);
        fetchData();
      },
    },
    {
      label: "Edit",
      onClick: (row) => {
        navigate(`/edit-service/${row.id}`);
      },
    },
  ];

  const handlePageChange = (page) => {
    setQuery((prev) => ({ ...prev, currentPage: page }));
  };

  return (
    <div className="service-list page">
      <DashboardServicesHomeNavbar />
      <div className="container">
        <DataTable data={services} columns={columns} handle={handle} />
      </div>
    </div>
  );
};
