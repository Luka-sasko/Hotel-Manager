import { get, post, put } from "./api_base";

const URL_PATH = "/api/user";

const getUserRole = async () => {
  var personData = await get("/api/user");
  var role = personData.data.role;
  return role;
};

const getUser = async () => {
  return await get("/api/user");
};

const createUser = async (registerData) => {
  try {
    const response = await post(`${URL_PATH}`, registerData);
    return response.status;
  } catch (error) {
    throw error;
  }
};
const updateUser = async (userData) => {
  return await put(URL_PATH, userData);
};

const updatePassword = async (passwordData) => {
  return await put(
    `${URL_PATH}/updatePassword`,
    {
      passwordOld: passwordData.passwordOld,
      passwordNew: passwordData.passwordNew,
    }
  );
};

const loginUser = async ({ email, password }) => {
  const loginData = new URLSearchParams();
  loginData.append("username", email);
  loginData.append("password", password);
  loginData.append("grant_type", "password");
  try {
    const response = await post(`/login`, loginData);
    return response;
  } catch (error) {
    throw error;
  }
};

export { getUser, getUserRole, createUser, updateUser, updatePassword, loginUser };
