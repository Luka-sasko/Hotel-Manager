import { get, post, put, remove } from "./api_base";
import { buildQueryString } from "../common/HelperFunctions";

const URL_PATH = "/api/RoomType/";

const getAllRoomType = async (query = null) => {
  let queryString = "";
  if (query !== null) {
    queryString = buildQueryString(query);
  }
  try {
    const response = await get(`${URL_PATH}${queryString}`);
    if (response.status === 200) {
      return [response.data.items, response.data.totalPages];
    }
    return [];
  } catch (error) {
    console.error("Error fetching room types:", error);
    return [];
  }
};

const getByIdRoomType = async (roomyTypeId) => {
  return await get(URL_PATH + roomyTypeId);
};

const createRoomType = async (roomType) => {
  return await post(`${URL_PATH}`, roomType);
};

const updateRoomType = async (roomyTypeId, roomType) => {
  return await put(`${URL_PATH}${roomyTypeId}`, roomType);
};

const deleteRoomType = async (roomyTypeId) => {
  return await remove(`${URL_PATH}${roomyTypeId}`);
};

export {
  getAllRoomType,
  getByIdRoomType,
  createRoomType,
  updateRoomType,
  deleteRoomType,
};
