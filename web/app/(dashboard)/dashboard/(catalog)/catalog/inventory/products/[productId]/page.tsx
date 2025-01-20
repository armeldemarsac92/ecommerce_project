"use client"

import { z } from "zod";
import { motion } from "framer-motion";
import AutoForm, { AutoFormSubmit } from "@/components/ui/auto-form";
import { useEffect, useState } from "react";
import {Image, Spinner} from "@nextui-org/react";
import * as React from "react";
import Product from "@/types/product";
import {getProduct} from "@/actions/products/products";
import {useToast} from "@/hooks/use-toast";

export default function InventoryProductIdPage({params}: {params: {productId: string}}) {
    const [isNewProduct, setIsNewProduct] = useState(false);
    const [loading, isLoading] = useState(true);
    const [product, setProduct] = useState<Product>({
        id: 0,
        title: "",
        description: "",
        price: 0,
        image: "",
        category: 0,
        brand: 0,
        stock: 0,
    });
    const { toast } = useToast()


    const getData = async () => {
        await getProduct(params.productId).then((response) => {
            setProduct(response)
        }).catch(() => {
            toast({ variant: "destructive", title: "Error", description: "An error occurred"})
        })
    }

    useEffect(() => {
        if(params.productId === "new") {
            setIsNewProduct(true);
            isLoading(false);
        } else {
            getData().then(() => {
                    isLoading(false)
                }
            )
        }
    }, [params]);

    const formSchema = z.object({
        image: z.string().default(product?.image),
        title: z
          .string({ required_error: 'Title is required' })
          .describe('Title')
          .min(2, {
            message: "Title must be at least 2 characters",
          })
          .default(product.title)
        ,
        description: z.string({
            required_error: 'Description is required'
        })
          .describe('Description')
          .default(product.description)
      ,
        price: z.number({})
            .min(0, { message: 'Price must be greater than 0'
        })
          .describe('Price')
          .default(product.price),
    });

    const onSubmit = (data: z.infer<typeof formSchema>) => {
       try {
           console.log(data);
       } catch (error) {
           console.error(error);
        }
    }

    return (
        <div>
            { !loading ? (
            <div className={`${isNewProduct ? "" : "grid m-5 grid-cols-1 lg:grid-cols-2"}`}>
            {!isNewProduct ? (
                <div className="flex justify-center mb-6 lg:mb-0 items-center">
                    <Image
                        width="100%"
                        alt={product.title}
                        src={product.image || '/images/product-placeholder.jpeg'}
                    />
                </div>
            ) : (
                <></>
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
            : (<div className="h-[30rem] w-full flex justify-center">
                    <Spinner label="Loading..." color="success" labelColor="success" size="lg"/>
                </div>
               )}
        </div>
    );
}
