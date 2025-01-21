"use client"

import { z } from "zod";
import { motion } from "framer-motion";
import AutoForm, { AutoFormSubmit } from "@/components/ui/auto-form";
import { useEffect, useState } from "react";
import {Image, Spinner} from "@nextui-org/react";
import * as React from "react";
import {useToast} from "@/hooks/use-toast";
import {useProduct} from "@/hooks/swr/products/use-product";
import {useCategories} from "@/hooks/swr/categories/use-categories";
import {useBrands} from "@/hooks/swr/brands/use-brands";
import {Select, SelectContent, SelectItem, SelectTrigger, SelectValue} from "@/components/shadcn/select";
import {updateProduct} from "@/actions/product";
import {UpdateProductForm} from "@/types/product/updateProduct"
import {Product} from "@/types/product/product";

export default function ProductClient({params}: {params: {productId: string}}) {
    const [isNewProduct, setIsNewProduct] = useState(false);
    const { toast } = useToast()
    const { product, loadingSWRProduct, errorSWRProduct } = useProduct(parseInt(params.productId));
    const { categories, loadingSWRCategories, errorSWRCategories } = useCategories();
    const { brands, loadingSWRBrands, errorSWRBrands } = useBrands();

    useEffect(() => {
        if(params.productId === "new") {
            setIsNewProduct(true);
        }

        if (errorSWRProduct || errorSWRCategories || errorSWRBrands) {
            toast({ variant: "destructive", title: "Error", description: "An error occurred"})
        }
    }, [params]);

    const formSchema = z.object({
        image: z.string(),
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
        price: z.coerce.number({})
            .min(0, { message: 'Price must be greater than 0'
            })
            .describe('Price'),
        category: z.string().describe('Category'),
        brand: z.string().describe('Brand'),
    });

    const onSubmit = (formData: UpdateProductForm) => {
        const completedFormData = {
            ...formData,
            id: parseInt(params.productId),
            brandId: parseInt(formData.brand),
            categoryId: parseInt(formData.category),
        }

        updateProduct(completedFormData).then((response)=>  {
        if (response.status === 200) {

            toast({ variant: "success", title: "Success", description: "Product successfully updated"})
        }})
        .catch(() => {
            toast({ variant: "destructive", title: "Error", description: "An error occurred"})
         })
    }

    return (
        <div className={"h-full"}>
            {!loadingSWRProduct && !loadingSWRCategories && !loadingSWRBrands ? (
                    <div className={`${isNewProduct ? "" : "grid m-5 grid-cols-1 lg:grid-cols-2"}`}>
                        {!isNewProduct && (
                            <div className="flex justify-center mb-6 lg:mb-0 items-center">
                                <Image
                                    width="100%"
                                    alt={product?.title ?? "No image"}
                                    src={product?.image || 'https://placehold.co/300x300'}
                                />
                            </div>
                        )}

                        <div className="w-11/12 flex mx-auto my-4 justify-center">
                            <motion.div
                                className="w-full max-w-2xl space-y-6"
                                initial={{opacity: 0}}
                                animate={{opacity: 1}}
                                transition={{duration: 0.5}}
                            >
                                <h1 className="text-2xl font-bold text-center">
                                    {params.productId !== "new" ? 'Modify product' : 'Add product'}
                                </h1>

                                <div className="w-full flex">
                                    <AutoForm
                                        values={{
                                            image: product?.image ?? "",
                                            title: product?.title ?? "",
                                            description: product?.description ?? "",
                                            price: product?.price ?? 0,
                                            category: product?.category_id.toString() ?? "0",
                                            brand: product?.brand_id.toString() ?? "0",
                                        }}
                                        className="w-full space-y-6"
                                        onSubmit={onSubmit}
                                        formSchema={formSchema}
                                        fieldConfig={{
                                            image: {
                                                fieldType: 'file',
                                            },
                                            title: {
                                                inputProps: {
                                                    placeholder: 'Enter product title',
                                                }
                                            },
                                            description: {
                                                fieldType: 'textarea',
                                                inputProps: {
                                                    placeholder: 'Enter product description',
                                                }
                                            },
                                            price: {
                                                inputProps: {
                                                    placeholder: 'Enter price',
                                                    type: 'number',
                                                }
                                            },
                                            category: {
                                                fieldType: ({ field }) => (
                                                    <Select
                                                        onValueChange={field.onChange}
                                                        defaultValue={field.value}
                                                    >
                                                        <SelectTrigger>
                                                            <SelectValue placeholder="Select a category" />
                                                        </SelectTrigger>
                                                        <SelectContent>
                                                            {categories.map((category) => (
                                                                <SelectItem key={category.id} value={category.id.toString()}>
                                                                    {category.title}
                                                                </SelectItem>
                                                            ))}
                                                        </SelectContent>
                                                    </Select>
                                                ),
                                            },
                                            brand: {
                                                fieldType: ({ field }) => (
                                                    <Select
                                                        onValueChange={field.onChange}
                                                        defaultValue={field.value}
                                                    >
                                                        <SelectTrigger>
                                                            <SelectValue placeholder="Select a brand" />
                                                        </SelectTrigger>
                                                        <SelectContent>
                                                            {brands.map((brand) => (
                                                                <SelectItem key={brand.id} value={brand.id.toString()}>
                                                                    {brand.title}
                                                                </SelectItem>
                                                            ))}
                                                        </SelectContent>
                                                    </Select>
                                                )
                                            }
                                        }}
                                    >
                                        <div className="flex justify-center">
                                            <AutoFormSubmit className="w-1/2 bg-secondary text-white">
                                                {isNewProduct ? 'Create product' : 'Update product'}
                                            </AutoFormSubmit>
                                        </div>
                                    </AutoForm>
                                </div>
                            </motion.div>
                        </div>
                    </div>)
                : (<div className="w-full h-full flex justify-center items-center">
                        <Spinner color="success" labelColor="success" />
                    </div>
                )}
        </div>
    );
}
