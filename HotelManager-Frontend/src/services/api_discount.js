import { get, post, put, remove} from "./api_base";
import { buildQueryString } from "../common/HelperFunctions";

const URL_PATH = "/api/Discount/";

const getAllDiscounts = async (query) => {
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

const getDiscounts = async () => {
  const response = await get(`${URL_PATH}`);
  try{
    if (response.status === 200) {
      return [response.data.items];
    }
    return [];
  }
  catch (error) {
    console.error(error);
    return [];
  }
}

const getByIdDiscount = async (discountId) => {
  return await get(URL_PATH+discountId)
};

const createDiscount = async (discount) => {
  return await post(`${URL_PATH}`, discount)
};

const updateDiscount = async (discountId, discount) => {
  return await put(`${URL_PATH}${discountId}`, discount)
};

const deleteDiscount = async (discountId) => {
  return await remove(`${URL_PATH}${discountId}`)
};

export {
  getAllDiscounts,
  getDiscounts,
  getByIdDiscount,
  createDiscount,
  updateDiscount,
  deleteDiscount
};
