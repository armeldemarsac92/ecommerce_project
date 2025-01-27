import {axiosInstance} from "@/utils/instance/axios-instance";

async function updateProduct(data: object)  {
    return await axiosInstance.put(`/products/${data.id}`, data)
}

async function deleteProduct(id: number)  {
    return await axiosInstance.delete(`/products/${id}`);
}

async function createProduct(data: object)  {
    return await axiosInstance.post(`/products`, data)
}


export {
    updateProduct,
    deleteProduct,
    createProduct,
}