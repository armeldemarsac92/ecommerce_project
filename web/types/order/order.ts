type Product = {
    productId: number;
    title: string;
    quantity: number;
    unitPrice: number;
    subtotal: number;
    brand: string;
    category: string;
}


export interface Order {
    id: number;
    userId: string;
    paymentStatus: string;
    totalAmount: number;
    stripeInvoiceId: string;
    stripePaymentIntentId: number | null;
    orderItems: Product[];
    createdAt: string;
}