import axios from "axios";
import { buildQueryString } from "../common/HelperFunctions";
import { get } from "./api_base";

const BASE_URL = "https://localhost:44327/api/room";

const getAllRooms = async (query) => {
  const queryString = buildQueryString(query);
  try {
    const response = await get(`/api/room${queryString}`);
    if (response.status === 200) {
      return [response.data.items, response.data.totalPages];
    }
    return [];
  }
  catch (error) {
    console.error(error);
    return [];
  }
};

const getByIdRoom = async (id) => {
  return await axios.get(`${BASE_URL}/${id}`);
};

export { getAllRooms, getByIdRoom };
