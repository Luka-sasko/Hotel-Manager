import { buildQueryString } from "../common/HelperFunctions";
import { get, post, put } from "./api_base";

const URL_PATH = "/api/DashBoardRoom";

const updateDashboardRoom = (roomId, roomData) => {
  return put(`${URL_PATH}/${roomId}`, roomData);
};

const getAllDashboardRooms = async (query) => {
  let queryString = "";
  if (query) {
    queryString = buildQueryString(query);
  }
  try {
    const response = await get(`${URL_PATH}${queryString}`);
    if (response.status === 200) {
      return [response.data.items, response.data.totalPages];

    }
    return [null, 1];
  }
  catch (error) {
    return [null, 1];
  }
};

const getDashboardRoomUpdateById = (roomId) => {
  return get(`${URL_PATH}/${roomId}`);
};


const createDashboardRoom = (roomData) => {
  return post(URL_PATH, roomData);
};

export {
  createDashboardRoom,
  updateDashboardRoom,
  getAllDashboardRooms,
  getDashboardRoomUpdateById,
};
