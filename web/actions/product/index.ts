import {axiosInstance} from "@/utils/instance/axios-instance";

async function updateProduct(data: object)  {
    return await axiosInstance.put(`/products/${data.id}`, data)
}


export {
    updateProduct
}