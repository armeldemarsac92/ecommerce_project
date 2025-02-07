type Product = {
    product_id: number;
    title: string;
    quantity: number;
    unit_price: number;
    subtotal: number;
    brand: string;
    category: string;
}


export interface Order {
    order_id: number;
    user_id: string;
    stripe_payment_status: string;
    total_amount: number;
    stripe_invoice_id: string;
    stripe_payment_intent_id: number | null;
    order_items: Product[];
    created_at: string;
}