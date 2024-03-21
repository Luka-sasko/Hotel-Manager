import { get, post, put, remove } from "./api_base";
import { buildQueryString } from "../common/HelperFunctions";

const URL_PATH = "/api/hotelService";

const getAllServices = async (query = null) => {
  let queryString = "";
  if (query) {
    queryString = buildQueryString(query);
  }
  try{
    const response = await get(`${URL_PATH}${queryString}`);
    if(response.status === 200) {
      return [response.data.items, response.data.totalPages]
    }
    return [];
  }
  catch(error) {
    console.error("Error fetching services:", error);
    return [];
  }
};

const getByIdService = async (id) => {
  return await get(`${URL_PATH}/${id}`);
};

const createService = async (serviceData) => {
  return await post(`${URL_PATH}`, serviceData);
};

const updateService = async (id, serviceData) => {
  return await put(`${URL_PATH}/${id}`, serviceData);
};

const deleteService = async (id) => {
  return await remove(`${URL_PATH}/${id}`);
};

export {
  getAllServices,
  getByIdService,
  createService,
  updateService,
  deleteService,
};
