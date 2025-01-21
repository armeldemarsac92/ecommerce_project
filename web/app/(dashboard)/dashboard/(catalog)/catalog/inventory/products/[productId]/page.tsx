"use client"

import { z } from "zod";
import { motion } from "framer-motion";
import AutoForm, { AutoFormSubmit } from "@/components/ui/auto-form";
import { useEffect, useState } from "react";
import {Image, Spinner} from "@nextui-org/react";
import * as React from "react";
import {useToast} from "@/hooks/use-toast";
import {Product} from "@/types/product";
import {useProduct} from "@/hooks/swr/products/use-product";
import {useForm} from "react-hook-form";
import {zodResolver} from "@hookform/resolvers/zod";

export default function InventoryProductIdPage({params}: {params: {productId: string}}) {
    const [isNewProduct, setIsNewProduct] = useState(false);
    const [loading, isLoading] = useState(true);
    /*const [product, setProduct] = useState<Product>()*/

    const { toast } = useToast()


    /*const getData = async () => {
        await getProduct(params.productId).then((response) => {
            setProduct(response)
        }).catch(() => {
            toast({ variant: "destructive", title: "Error", description: "An error occurred"})
        })
    }*/

    const { product, loadingSWRProduct, errorSWRProduct } = useProduct(13);

    useEffect(() => {
        if(params.productId === "new") {
            setIsNewProduct(true);
            isLoading(false);
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
        price: z.number({})
            .min(0, { message: 'Price must be greater than 0'
            })
            .describe('Price'),
    });

    const onSubmit = (data: z.infer<typeof formSchema>) => {
        try {
            console.log(data);
        } catch (error) {
            console.error(error);
        }
    }

    return (
        <div className={"h-full"}>
            {!loadingSWRProduct ? (
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
                                            price: product?.price ?? 0
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
                        <Spinner label="Loading..." color="success" labelColor="success" size="lg"/>
                    </div>
                )}
        </div>
    );
}
