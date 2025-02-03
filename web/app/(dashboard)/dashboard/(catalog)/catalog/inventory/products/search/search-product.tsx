"use client"

import {useState} from "react";
import { Input } from "@/components/shadcn/input";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/shadcn/select";
import { Button } from "@/components/shadcn/button";
import { Card, CardContent } from "@/components/shadcn/card";
import { useRouter } from "next/navigation";
import { Spinner } from "@nextui-org/react";
import { getOpenFoodFactsProducts } from "@/actions/product";
import { useToast } from "@/hooks/use-toast";
import {OpenFoodFactProduct} from "@/types/product/open-food-fact-product";
import { useOpenFoodFactsStore } from '@/store/open-food-fact-store';
import {useProductStore} from "@/store/product-store";


export default function SearchProducts() {
    const [searchQuery, setSearchQuery] = useState("");
    const [nutritionGrade, setNutritionGrade] = useState("");
    const [loading, setLoading] = useState(false);
    const [products, setProducts] = useState<OpenFoodFactProduct[]>([]);
    const [submitted, setSubmitted] = useState(false);
    const router = useRouter();
    const { toast } = useToast();
    const setSelectedProduct = useOpenFoodFactsStore((state) => state.setSelectedProduct);
    const { productToUpdate } = useProductStore();

    const handleSearch = async () => {
        setLoading(true);
        getOpenFoodFactsProducts({Category: searchQuery, NutritionGrade: nutritionGrade}).then((response) => {
            setProducts(response.data.products);
            setLoading(false);
            setSubmitted(true);
        })
            .catch((() => {
                toast({ variant: "destructive", title: "Error", description: "An error occurred"})
                setLoading(false);
            }))
    };

    const handleProductSelect = (product: OpenFoodFactProduct) => {
        setSelectedProduct({
            code: product.code,
            image_url: product.image_url,
        })
        if (productToUpdate) {
            router.push(`/dashboard/catalog/inventory/products/${productToUpdate}`);
        } else {
            router.push(`/dashboard/catalog/inventory/products/new`);
        }
    };

    return (
        <div className="container mx-auto p-4 space-y-6 h-full">
            <div className="flex justify-between gap-3 items-end">
                <Input
                    placeholder="Search for a product..."
                    value={searchQuery}
                    onChange={(e) => setSearchQuery(e.target.value)}
                    className="flex-1 max-w-[400px]"
                />
                <div className="flex gap-3">
                    <Select value={nutritionGrade} onValueChange={setNutritionGrade}>
                        <SelectTrigger className="w-[180px]">
                            <SelectValue placeholder="Nutrition grade" />
                        </SelectTrigger>
                        <SelectContent>
                            <SelectItem value="A">A</SelectItem>
                            <SelectItem value="B">B</SelectItem>
                            <SelectItem value="C">C</SelectItem>
                            <SelectItem value="D">D</SelectItem>
                            <SelectItem value="E">E</SelectItem>
                        </SelectContent>
                    </Select>
                    <Button
                        onClick={handleSearch}
                        disabled={loading}
                        className="w-32 bg-secondary text-white"
                    >
                        {loading ? <Spinner size="sm" /> : "Search"}
                    </Button>
                </div>
            </div>

            {loading ? (
                <div className="flex justify-center items-center h-full">
                    <Spinner size="lg" color="success" />
                </div>
            ) : (
                products.length === 0 ? (
                    <div className="text-center py-10 h-full">
                        <p className="text-gray-500">No products found. Try searching for something.</p>
                    </div>
                ) : (
                    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-5 gap-4">
                        {submitted && products.map((product) => (
                            <Card
                                key={product.code}
                                className="cursor-pointer hover:shadow-lg transition-shadow"
                                onClick={() => handleProductSelect(product)}
                            >
                                <CardContent className="p-4">
                                    <img
                                        src={product.image_url || 'https://placehold.co/300x300'}
                                        alt={product.product_name_fr}
                                        className="w-full h-48 object-cover mb-2 rounded"
                                    />
                                    <p className="text-sm text-gray-600 truncate">{product.product_name_fr}</p>
                                </CardContent>
                            </Card>
                        ))}
                    </div>
                )
            )}
        </div>
    );
}