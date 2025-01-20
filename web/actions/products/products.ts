import api from '@/lib/axios'
import Product from "@/types/product";

export const getProducts = async (): Promise<Product[]> => {
    return await api.get('/v1/products')
}

export const getProduct = async (id: string): Promise<Product> => {
    return await api.get(`/v1/products/${id}`)
}