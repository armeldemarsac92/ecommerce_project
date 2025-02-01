import {axiosInstance} from "@/utils/instance/axios-instance";
import {CreateInventoryForm} from "@/types/inventory/createInventoryForm";

async function createInventory(data: CreateInventoryForm) {
    return await axiosInstance.post(`/inventories`, data)
}

export {
    createInventory
}