import {axiosInstance} from "@/utils/instance/axios-instance";
import {CompletedProductForm} from "@/types/product/completedProductForm";

async function updateProduct(data: CompletedProductForm)  {
    return await axiosInstance.put(`/products/${data.id}?productId=${data.id}`, data)}

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