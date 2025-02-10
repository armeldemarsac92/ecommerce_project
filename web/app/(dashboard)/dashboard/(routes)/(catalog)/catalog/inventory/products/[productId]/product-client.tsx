"use client"

import { z } from "zod";
import { motion } from "framer-motion";
import { useEffect, useState } from "react";
import {Image, Spinner} from "@nextui-org/react";
import * as React from "react";
import {useToast} from "@/hooks/use-toast";
import {useProduct} from "@/hooks/swr/products/use-product";
import {useCategories} from "@/hooks/swr/categories/use-categories";
import {useBrands} from "@/hooks/swr/brands/use-brands";
import {Select, SelectContent, SelectItem, SelectTrigger, SelectValue} from "@/components/shadcn/select";
import {updateProduct, createProduct} from "@/actions/product";
import {ProductForm} from "@/types/product/product-form"
import {useRouter} from "next/navigation";
import { Button } from "@/components/shadcn/button"
import {useTags} from "@/hooks/swr/tags/use-tags";
import {useForm} from "react-hook-form";
import {zodResolver} from "@hookform/resolvers/zod";
import {Form, FormField, FormItem, FormLabel, FormControl, FormMessage} from "@/components/shadcn/form";
import {Input} from "@/components/shadcn/input";
import {Textarea} from "@/components/shadcn/textarea";
import {CompletedProductForm} from "@/types/product/completed-product-form";
import {Popover, PopoverTrigger, PopoverContent} from "@/components/shadcn/popover";
import {Command, CommandEmpty, CommandInput, CommandList, CommandGroup, CommandItem} from "@/components/shadcn/command";
import { Check, ChevronsUpDown } from "lucide-react"
import { cn } from "@/lib/utils"
import {CompletedCreateProductForm} from "@/types/product/completed-create-product-form";
import { useOpenFoodFactsStore } from '@/store/open-food-fact-store';
import {useProductStore} from "@/store/product-store";


export default function ProductClient({params}: {params: {productId: string}}) {
    const [isNewProduct, setIsNewProduct] = useState(false);
    const { toast } = useToast()
    const router = useRouter();
    const [loading, setLoading] = useState(true);
    const { selectedProduct, clearSelectedProduct } = useOpenFoodFactsStore();
    const { clearProductToUpdate } = useProductStore();
    const { product, loadingSWRProduct, errorSWRProduct } = useProduct(parseInt(params.productId));
    const { categories, loadingSWRCategories, errorSWRCategories } = useCategories();
    const { brands, loadingSWRBrands, errorSWRBrands } = useBrands();
    const { tags, loadingSWRTags, errorSWRTags } = useTags();

    const formSchema = z.object({
        id: z.number({}),
        title: z
            .string({ required_error: 'Title is required' })
            .describe('Title')
            .min(2, {
                message: "Title must be at least 2 characters",
            }),
        description: z.string({
            required_error: 'Description is required'
        })
            .describe('Description'),
        price: z.coerce.number()
            .min(0, { message: 'Price must be greater than 0'
            })
            .describe('Price')
            .default(0)
        ,
        quantity: z.coerce.number()
            .min(0, { message: 'Quantity must be greater than 0'})
            .describe('Quantity'),
        category: z.string().describe('Category'),
        brand: z.string().describe('Brand'),
        tag: z.array(z.number()).describe('Tags'),
    });

    type FormValues = z.infer<typeof formSchema>;

    const form = useForm<FormValues>({
        resolver: zodResolver(formSchema),
        defaultValues: {
            id: 0,
            title: "",
            description: "",
            price: 0,
            quantity: 0,
            category: "",
            brand: "",
            tag: [],
        },
    });

    useEffect(() => {
        if (errorSWRProduct || errorSWRCategories || errorSWRBrands || errorSWRTags) {
            toast({ variant: "destructive", title: "Error", description: "An error occurred"})
        }

        if (!loadingSWRProduct && !loadingSWRBrands && !loadingSWRCategories && !loadingSWRTags) {
            setLoading(false);
        }

        if(params.productId === "new" && selectedProduct) {
            setIsNewProduct(true);
        } else {
            if (!isNewProduct && categories && brands && product) {
                form.reset({
                        id: product?.id ?? 0,
                        title: product?.title ?? "",
                        description: product?.description ?? "",
                        price: product?.price ?? 0,
                        quantity: 0,
                        category:  categories.find(cat => cat.title === product.category_title)?.id.toString() ?? "",
                        brand: brands.find(brand => brand.title === product.brand_title)?.id.toString() ?? "",
                        tag: tags?.filter(tag => product?.tags?.includes(tag.title)).map(tag => tag.id) ?? [],
                })
            }
        }

    }, [params, categories, brands, product, isNewProduct, form, selectedProduct]);

    const onSubmit = (formData: ProductForm) => {
        if (isNewProduct) {
            const now = new Date();
            const completedFormData: CompletedCreateProductForm = {
                title: formData.title,
                description: formData.description,
                price: formData.price,
                brand_id: parseInt(formData.brand),
                category_id: parseInt(formData.category),
                image_url: selectedProduct?.image_url ?? "",
                quantity: formData.quantity,
                tag_ids: formData.tag ?? [],
                open_food_fact_id: selectedProduct?.code ?? "",
                updated_at: now.toISOString(),
                created_at: now.toISOString()
            }

            createProduct(completedFormData).then(() => {
                toast({ variant: "success", title: "Success", description: "Product successfully created"})
                clearSelectedProduct()
                router.push(`/dashboard/catalog/inventory`);
            })
            .catch(() => {
                toast({ variant: "destructive", title: "Error", description: "An error occurred"})

            })
        } else {
            const completedFormData: CompletedProductForm = {
                id: formData.id,
                title: formData.title,
                description: formData.description,
                price: formData.price,
                brand_id: parseInt(formData.brand),
                category_id: parseInt(formData.category),
                image_url: selectedProduct?.image_url || (product?.image_url ?? ""),
                tag_ids: formData.tag ?? [],
                open_food_fact_id: parseInt(selectedProduct?.code ?? "0")
            }

            updateProduct(completedFormData).then((response)=>  {
                if (response.status === 200) {
                    toast({ variant: "success", title: "Success", description: "Product successfully updated"})
                    clearSelectedProduct()
                    clearProductToUpdate()
                    router.push(`/dashboard/catalog/inventory`);
                }})
                .catch(() => {
                    toast({ variant: "destructive", title: "Error", description: "An error occurred"})
                })
        }
    }

    return (
        <div
            data-cy="product-form"
            className="h-full"
        >
            { !loading ? (
                <div className={`${isNewProduct ? "" : "grid m-5 grid-cols-1 lg:grid-cols-2"}`}>
                    {!isNewProduct && (
                        <div className="flex justify-center mb-6 lg:mb-0 items-center">
                            <Image
                                width="100%"
                                alt={product?.title ?? "No image"}
                                src={product?.image_url && product?.image_url.startsWith('http')
                                    ? product?.image_url : 'https://placehold.co/300x300'
                                }
                            />
                        </div>
                    )}

                    <div className="w-11/12 flex mx-auto my-4 justify-center">
                        <motion.div
                            className="w-full max-w-2xl space-y-6"
                            initial={{ opacity: 0 }}
                            animate={{ opacity: 1 }}
                            transition={{ duration: 0.5 }}
                        >
                            <div className="flex flex-col items-start gap-8">
                                <h1 className="text-2xl font-bold w-full text-center">
                                    {params.productId !== "new" ? 'Modify product' : 'Create product'}
                                </h1>
                                {!isNewProduct && (
                                    <Button
                                        type="button"
                                        onClick={() =>
                                        {
                                            useProductStore.getState().setProductToUpdate(params.productId)
                                            router.push('/dashboard/catalog/inventory/products/search')
                                        }}
                                        variant="outline"
                                        className="bg-secondary text-white"
                                    >
                                        Search OpenFoodFacts
                                    </Button>
                                )}
                            </div>

                            <Form {...form}>
                                <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-6">
                                    <FormField
                                        control={form.control}
                                        name="title"
                                        render={({ field }) => (
                                            <FormItem>
                                                <FormLabel>Title</FormLabel>
                                                <FormControl>
                                                    <Input
                                                        data-cy="title-input"
                                                        placeholder="Enter product title" {...field}
                                                    />
                                                </FormControl>
                                                <FormMessage />
                                            </FormItem>
                                        )}
                                    />

                                    <FormField
                                        control={form.control}
                                        name="description"
                                        render={({ field }) => (
                                            <FormItem>
                                                <FormLabel>Description</FormLabel>
                                                <FormControl>
                                                    <Textarea
                                                        data-cy="description-input"
                                                        placeholder="Enter product description"
                                                        className="min-h-[100px]"
                                                        {...field}
                                                    />
                                                </FormControl>
                                                <FormMessage />
                                            </FormItem>
                                        )}
                                    />

                                    <FormField
                                        data-cy="price-input"
                                        control={form.control}
                                        name="price"
                                        render={({ field }) => (
                                            <FormItem>
                                                <FormLabel>Price</FormLabel>
                                                <FormControl>
                                                    <Input
                                                        data-cy="price-input"
                                                        type="number"
                                                        step="0.01"
                                                        placeholder="Enter price"
                                                        {...field}
                                                        onChange={(e) => {
                                                            let value = e.target.value;

                                                            if (value === '') {
                                                                field.onChange(0);
                                                            } else {
                                                                const numValue = parseFloat(value);

                                                                field.onChange(numValue < 0 ? 0 : numValue);
                                                            }
                                                        }}
                                                    />
                                                </FormControl>
                                                <FormMessage />
                                            </FormItem>
                                        )}
                                    />

                                    <FormField
                                        control={form.control}
                                        name="category"
                                        render={({ field }) => (
                                            <FormItem>
                                                <FormLabel>Category</FormLabel>
                                                <Select
                                                    onValueChange={(newValue) => {
                                                        if (newValue) {
                                                            field.onChange(newValue);
                                                        }
                                                    }}
                                                    value={field.value}
                                                >
                                                    <FormControl>
                                                        <SelectTrigger data-cy="category-trigger">
                                                            <SelectValue placeholder="Select a category" />
                                                        </SelectTrigger>
                                                    </FormControl>
                                                    <SelectContent>
                                                        {categories?.map((category) => (
                                                            <SelectItem
                                                                key={category.id}
                                                                value={category.id.toString()}
                                                            >
                                                                {category.title}
                                                            </SelectItem>
                                                        ))}
                                                    </SelectContent>
                                                </Select>
                                                <FormMessage />
                                            </FormItem>
                                        )}
                                    />

                                    <FormField
                                        control={form.control}
                                        name="brand"
                                        render={({ field }) => (
                                            <FormItem>
                                                <FormLabel>Brand</FormLabel>
                                                <Select
                                                    onValueChange={(newValue) => {
                                                        if (newValue) {
                                                            field.onChange(newValue);
                                                        }
                                                    }}
                                                    value={field.value}
                                                >
                                                    <FormControl>
                                                        <SelectTrigger data-cy="brand-trigger">
                                                            <SelectValue placeholder="Select a brand" />
                                                        </SelectTrigger>
                                                    </FormControl>
                                                    <SelectContent>
                                                        {brands?.map((brand) => (
                                                            <SelectItem
                                                                key={brand.id}
                                                                value={brand.id.toString()}
                                                            >
                                                                {brand.title}
                                                            </SelectItem>
                                                        ))}
                                                    </SelectContent>
                                                </Select>
                                                <FormMessage />
                                            </FormItem>
                                        )}
                                    />
                                    {isNewProduct && (
                                        <FormField
                                            control={form.control}
                                            name="quantity"
                                            render={({ field }) => (
                                                <FormItem>
                                                    <FormLabel>Quantity</FormLabel>
                                                    <FormControl>
                                                        <Input
                                                            data-cy="quantity-input"
                                                            type="number"
                                                            placeholder="Entrez une quantité"
                                                            {...field}
                                                            onChange={(e) => {
                                                                let value = e.target.value;

                                                                if (value === '') {
                                                                    field.onChange(0);
                                                                } else {
                                                                    const numValue = parseInt(value);

                                                                    field.onChange(numValue < 0 ? 0 : numValue);
                                                                }
                                                            }}
                                                        />
                                                    </FormControl>
                                                    <FormMessage />
                                                </FormItem>
                                            )}
                                        />
                                    )}
                                    <FormField
                                        control={form.control}
                                        name="tag"
                                        render={({ field }) => (
                                            <FormItem>
                                                <FormLabel>Tags</FormLabel>
                                                <FormControl>
                                                    <Popover>
                                                        <PopoverTrigger asChild>
                                                            <Button
                                                                data-cy="tags-select"
                                                                variant="outline"
                                                                role="combobox"
                                                                className="w-full justify-between"
                                                            >
                                                                <span className="truncate">
                                                                {field.value.length > 0
                                                                    ? field.value
                                                                        .map(tagId => tags?.find(t => t.id === tagId)?.title)
                                                                        .filter(Boolean)
                                                                        .join(", ")
                                                                    : "Select tags..."}
                                                                </span>
                                                                <ChevronsUpDown className="ml-2 h-4 w-4 shrink-0 opacity-50" />
                                                            </Button>
                                                        </PopoverTrigger>
                                                        <PopoverContent className="w-[300px] p-0">
                                                            <Command>
                                                                <CommandInput placeholder="Search tags..." />
                                                                <CommandList>
                                                                    <CommandEmpty>No tags found.</CommandEmpty>
                                                                    <CommandGroup>
                                                                        {tags?.map((tag) => (
                                                                            <CommandItem
                                                                                key={tag.id}
                                                                                onSelect={() => {
                                                                                    const newValue = field.value.includes(tag.id)
                                                                                    ? field.value.filter((id: number) => id !== tag.id)
                                                                                    : [...field.value, tag.id];

                                                                                    field.onChange(newValue);
                                                                                }}
                                                                            >
                                                                                <Check
                                                                                    className={cn(
                                                                                        "mr-2 h-4 w-4",
                                                                                        field.value.includes(tag.id) ? "opacity-100" : "opacity-0"
                                                                                    )}
                                                                                />
                                                                                {tag.title}
                                                                            </CommandItem>
                                                                        ))}
                                                                    </CommandGroup>
                                                                </CommandList>
                                                            </Command>
                                                        </PopoverContent>
                                                    </Popover>
                                                </FormControl>
                                                <FormMessage />
                                            </FormItem>
                                        )}
                                    />

                                    <div className="flex justify-center">
                                        <Button
                                            data-cy="submit-button"
                                            type="submit"
                                            className="w-1/2 bg-secondary text-white"
                                        >
                                            {isNewProduct ? 'Create product' : 'Update product'}
                                        </Button>
                                    </div>
                                </form>
                            </Form>
                        </motion.div>
                    </div>
                </div>
            ) : (
                <div className="w-full h-full flex justify-center items-center">
                    <Spinner color="success" labelColor="success" />
                </div>
            )}
        </div>
    );
}
