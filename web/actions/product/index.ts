import {axiosInstance} from "@/utils/instance/axios-instance";
import {CompletedProductForm} from "@/types/product/completed-product-form";
import {CompletedCreateProductForm} from "@/types/product/completed-create-product-form";
import {OpenFoodFactProductSearch} from "@/types/product/open-food-fact-product-search";

async function updateProduct(data: CompletedProductForm) {
    return await axiosInstance.put(`/products/${data.id}?productId=${data.id}`, data)
}

async function deleteProduct(id: number)  {
    return await axiosInstance.delete(`/products/${id}`);
}

async function createProduct(data: CompletedCreateProductForm)  {
    return await axiosInstance.post(`/products`, data)
}

async function getOpenFoodFactsProducts(data: OpenFoodFactProductSearch) {
    return await axiosInstance.get(`/open_food_fact?Category=${data.Category}&NutritionGrade=${data.NutritionGrade}&PageSize=15`);
}


export {
    updateProduct,
    deleteProduct,
    createProduct,
    getOpenFoodFactsProducts
}