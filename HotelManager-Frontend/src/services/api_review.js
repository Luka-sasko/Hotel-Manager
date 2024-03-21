import { get, post } from "./api_base";

const URL_PATH = "/api/review/";

const getReviewForRoom = async (roomId) => {
  return await get(URL_PATH+roomId)
};

const getReviewForRoomPaging = async (roomId, pageNumber, pageSize) => {
  return await get(`${URL_PATH}${roomId}?pageNumber=${pageNumber}&pageSize=${pageSize}`);
};

const createReviewForRoom = async (roomId, review) => {
  return await post(`${URL_PATH}${roomId}`, review);
};

export default {
  getReviewForRoom,
  createReviewForRoom,
  getReviewForRoomPaging,
};
