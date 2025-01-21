export interface Product {
    id: number
    stripe_id: string|null
    title: string
    description: string
    price: number
    price_ht: number
    brand_id: number
    category_id: number
    open_food_fact_id: number
    updated_at: string
    created_at: string
    stock: number
    image: string
}