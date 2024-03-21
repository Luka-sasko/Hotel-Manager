import axios from "axios";

const BASE_URL = "https://localhost:44327";

const get = async (url) => {
    const headers = {
        Authorization: getToken(),
    };
    return await axios.get(`${BASE_URL}${url}`, { headers: headers });
}

const post = async (url, data) => {
    const headers = {
        Authorization: getToken(),
    };
    return await axios.post(`${BASE_URL}${url}`, data, { headers: headers });
}

const put = async (url, data) => {
    const headers = {
        Authorization: getToken(),
    };
    return await axios.put(`${BASE_URL}${url}`, data, { headers: headers });
}

const remove = async (url) => {
    const headers = {
        Authorization: getToken(),
    };
    return await axios.delete(`${BASE_URL}${url}`, { headers: headers });
}

const getToken = () => {
    const token = localStorage.getItem("token");
    return token ? `Bearer ${token}` : "";
}

export { get, post, put, remove, getToken };