import api from "@/lib/axios";

export const getCategories = async () => {
    return await api.get("/v1/categories")
}