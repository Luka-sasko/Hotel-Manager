import { useNavigate } from "react-router-dom";
import { useState, useEffect } from "react";
import DataTable from "../Common/DataTable";
import { deleteRoomType, getAllRoomType } from "../../services/api_room_type";
import Paging from "../Common/Paging";

export const DashboardRoomType = () => {
  const [roomTypes, setRoomTypes] = useState([]);
  const navigate = useNavigate();
  const [query, setQuery] = useState({
    currentPage: 1,
    pageSize: 10,
    totalPages: 1,
    sortBy: "Name",
    sortOrder: "ASC",
  });

  useEffect(() => {
    const fetchData = async () => {
      try {
        const [items, totalPages] = await getAllRoomType(query);
        if (items && Array.isArray(items)) {
          setRoomTypes([...items]);
          setQuery( (prev) => ({
            ...prev,
            totalPages
          }));
        } else {
          console.error("Unexpected room types data format:", items);
        }
      } catch (error) {
        console.error("Error fetching room types:", error);
      }
    };

    fetchData();
  }, [query.currentPage]);



  const columns = [
    { key: "name", label: "Roomtype name" },
    { key: "description", label: "Description" },
  ];

  const handle = [
    {
      label: "Delete",
      onClick: (row) => {
        const confirmed = window.confirm("Are you sure you want to delete this room type?");
        if (confirmed) {
          deleteRoomType(row.id)
            .then(() => {
              window.location.reload();
            })
            .catch((error) => {
              console.error("Error deleting room type:", error);
            });
        }
      },
    },
    {
      label: "Edit",
      onClick: (row) => {
        navigate(`/dashboard-roomtype/${row.id}`);
      },
    },
  ];

  const handlePageChange = (newPage) => {
    if (newPage >= 1) {
      setQuery({
        ...query,
        currentPage: newPage
      });
    }
  }

  return (
    <div className="service-list">
      <DataTable data={roomTypes} columns={columns} handle={handle} />
      <Paging
        totalPages={query.totalPages}
        currentPage={query.currentPage}
        onPageChange={handlePageChange}
      />
    </div>
  );
};
