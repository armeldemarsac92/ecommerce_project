"use client"

import { z } from "zod";
import {motion} from "framer-motion";
import AutoForm, {AutoFormSubmit} from "@/components/ui/auto-form";
import { useParams } from "next/navigation";
import {useEffect, useState} from "react";

enum Category {
    Drink = 'Drink',
    Meat = 'Meat',
    Vegetables = 'Vegetables',
    Snacks = 'Snacks'
}

enum Brand {
    CocaCola = "Coca cola",
    Volvic = "Volvic",
    Freedent = "Freedent",
    Danone = "Danone",
    Sigis = "Sigis"
}

export default function InventoryProductIdPage() {
    const params = useParams();
    const [isNewProduct, setIsNewProduct] = useState(false);

    useEffect(() => {
        if(params.productId === "new") {
            setIsNewProduct(true);
        }
    }, [params]);

    const formSchema = z.object({
        title: z
          .string({ required_error: 'Title is required' })
          .describe('Title'),
          /*.min(2, {
            message: "Title must be at least 2 characters",
          }),*/
        description: z.string({
            required_error: 'Description is required'
        })
          .describe('Description')
      ,
        price: z.number({})
            .min(0, { message: 'Price must be greater than 0'
        })
          .describe('Price')
      ,
        category_id: z.nativeEnum(Category, {
            required_error: 'Category is required'
        })
          .describe('Category')
      ,
        brand_id: z.nativeEnum(Brand, {
            required_error: 'Brand is required'
        })
          .describe('Brand')
      ,
    });

    const onSubmit = (data: z.infer<typeof formSchema>) => {
       try {
           console.log(data);
       } catch (error) {
           console.error(error);
        }
    }

    return (
        <div className="w-11/12 flex mx-auto my-4">
            <motion.div
                className="w-full max-w-2xl space-y-6"
                initial={{ opacity: 0 }}
                animate={{ opacity: 1 }}
                transition={{ duration: 0.5 }}
            >
                <h1 className="text-2xl font-bold">
                    {params.productId !== "new" ? 'Modify product' : 'Add product'}
                </h1>

                <div className="w-full flex">
                    <AutoForm
                        className="w-full space-y-6"
                        onSubmit={onSubmit}
                        formSchema={formSchema}
                        fieldConfig={{
                            title: {
                                inputProps: {
                                    placeholder: 'Enter product title',
                                    defaultValue: isNewProduct ? "Caca" : params.productId
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
                            category_id: {
                                fieldType: 'select',
                                inputProps: {
                                    placeholder: 'Select category'
                                }
                            },
                            brand_id: {
                                fieldType: 'select',
                                inputProps: {
                                    placeholder: 'Select brand',
                                }
                            },
                        }}
                    >
                        <div className="flex">
                            <AutoFormSubmit className="w-1/2 bg-secondary text-white">
                                {isNewProduct ? 'Create product' : 'Update product'}
                            </AutoFormSubmit>
                        </div>
                    </AutoForm>
                </div>
            </motion.div>
        </div>
    );
}
