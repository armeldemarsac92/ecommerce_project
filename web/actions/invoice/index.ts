import {axiosInstance} from "@/utils/instance/axios-instance";

async function getInvoiceById(id: number)  {
    return await axiosInstance.get(`/orders/${id}/invoice`);
}

export {
    getInvoiceById
}