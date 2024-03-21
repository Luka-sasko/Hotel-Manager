import { get, post } from "./api_base";
import { buildQueryString } from "../common/HelperFunctions";

const BASE_URL = "/api/ServiceInvoice";

const getByInvoiceId = async (query) => {
  const queryString = buildQueryString(query);
  try {
    const response = await get(`${BASE_URL}${queryString}`);
    if (response.status === 200) {
      return response.data.items;
    }
    return null;
  }
  catch (error) {
    console.error(error);
    return null;
  }
};

const getAllServiceInvoice = () => { };

const deleteServiceInvoice = () => { };

const createServiceInvoice = (data) => {
  return post(BASE_URL, data);
};

export default {
  getByInvoiceId,
  getAllServiceInvoice,
  deleteServiceInvoice,
  createServiceInvoice,
};
