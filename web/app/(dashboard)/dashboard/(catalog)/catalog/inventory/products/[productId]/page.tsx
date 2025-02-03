import ProductClient from "@/app/(dashboard)/dashboard/(catalog)/catalog/inventory/products/[productId]/product-client";

export default function ProductPage({params}: {params: {productId: string}}) {
    return <ProductClient params={params} />
}