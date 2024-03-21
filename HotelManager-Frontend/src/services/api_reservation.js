import { buildQueryString } from "../common/HelperFunctions";
import { get, remove, put, post } from "./api_base";

const BASE_URL = "/api/reservation";

const getAllReservations = async (query) => {
  const queryString = buildQueryString(query);
  try {
    const response = await get(`${BASE_URL}${queryString}`);
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

const getByIdReservation = async (reservationId) => {
  try {
    const response = await get(`${BASE_URL}/${reservationId}`)
    if (response.status === 200) {
      return response;
    }
    return [];
  }
  catch (error) {
    console.error(error);
    return [];
  }
};

const createReservation = async (reservationData) => {
  try {
    const response = await post(`${BASE_URL}/`, reservationData);
    if (response.status === 200) {
      alert("Sucessfully created reservation!")
      return true
    } else {
      return false
    }
  } catch (error) {
    console.error("Error creating reservation:", error);
    return false;
  }
};


const updateReservation = async (reservationId, invoiceId, reservationData) => {
  try {
    const response = await put(`${BASE_URL}/${reservationId}?invoiceId=${invoiceId}`, reservationData);
    if (response.status === 200) {
      return true
    } else {
      return false
    }
  }
  catch (error) {
    console.error("Error deleting reservation:", error);
    return false;
  }


};

const deleteReservation = async (reservationId, invoiceId) => {
  try {
    const response = await remove(`${BASE_URL}/${reservationId}?invoiceId=${invoiceId}`);
    if (response.status === 200) {
      return true;

    }
    return false;
  } catch (error) {
    console.error("Error deleting reservation:", error);
    return false;
  }
};


export default {
  getAllReservations,
  getByIdReservation,
  createReservation,
  updateReservation,
  deleteReservation,
};

