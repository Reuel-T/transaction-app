import { API_BASEURL } from "@/constants/AppConstants";
import axios, { type AxiosInstance } from "axios";

const axiosInstance: AxiosInstance = axios.create({
  baseURL: API_BASEURL
})

export default axiosInstance;