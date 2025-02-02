export interface CompletedCreateProductForm {
    title: string;
    description: string;
    price: number;
    category_id: number;
    brand_id: number;
    image_url: string;
    quantity: number;
    tag_ids: object;
    open_food_fact_id: string
    updated_at: string;
    created_at: string;
}