import {axiosInstance} from "@/utils/instance/axios-instance";

export const axiosSWRFetcher = (url: string) => axiosInstance.get(url).then(res => res.data);