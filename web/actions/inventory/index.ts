import {axiosInstance} from "@/utils/instance/axios-instance";

async function increamentProduct(product_id: number, quantity: number) {
    return await axiosInstance.put(`/inventories/${product_id}/increament`, {quantity: quantity});
}

async function substractProduct(product_id: number, quantity: number) {
    return await axiosInstance.put(`/inventories/${product_id}/substract`, {quantity: quantity});
}

export {
    increamentProduct,
    substractProduct,
}