export interface CompletedCreateProductForm {
    title: string;
    description: string;
    price: number;
    category_id: number;
    brand_id: number;
    image_url: string;
    tag_ids: object;
    open_food_fact_id: number
    updated_at: string;
    created_at: string;
}