import { get, post } from './api_base';
import { buildQueryString } from '../common/HelperFunctions';

const URL_PATH = "/api/DashboardReceipt";

const getAllDashboardInvoice = async (query) => {
  const queryString = buildQueryString(query);
  try {
    const response = await get(`${URL_PATH}${queryString}`);
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

const getByIdDashboardInvoice = async (id) => {
  try {
    const response = await get(`${URL_PATH}/${id}`);
    if (response.status === 200) {
      return response.data;
    }
    return null;
  }
  catch (error) {
    console.error(error);
    return null;
  }
};

const sendDashboardInvoice = async (id) => {
  try {
    const response = await post(`${URL_PATH}/${id}`);
    if (response.status === 200) {
      return true;
    }
    return false;
  }
  catch (error) {
    console.error(error);
    return false;
  }
};

export default {
  getAllDashboardInvoice,
  getByIdDashboardInvoice,
  sendDashboardInvoice,
};
