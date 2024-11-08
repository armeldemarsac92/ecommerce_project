"use client"
import { z } from "zod";
import {motion} from "framer-motion";
import AutoForm, {AutoFormSubmit} from "@/components/ui/auto-form";

enum Category {
    Drink = 'Drink',
    Meat = 'Meat',
    PizzasPiesAndQuiches = 'Pizzas pies and quiches',
    Snacks = 'Snacks'
}

enum Brand {
    CocaCola = "Coca cola",
    Bacon = "Bacon",
    Water1L = "Water 1L",
    PizzaExpressMargherita = "Pizza express margherita",
    DarkChocolate65Cocoa = "Dark chocolate 65% cocoa"
}

export default function ProductIdPage() {
    const formSchema = z.object({
        title: z.string({
            required_error: 'Title is required'
        }),
        description: z.string({
            required_error: 'Description is required'
        }),
        price: z.number({})
            .min(0, {
                message: 'Price must be greater than 0'
        }),
        category_id: z.nativeEnum(Category, {
            required_error: 'Category is required'
        }),
        brand_id: z.nativeEnum(Brand, {
            required_error: 'Brand is required'
        }),
    });

    const onSubmit = (data: z.infer<typeof formSchema>) => {
       try {
           console.log(data);
       } catch (error) {
           console.error(error);
        }
    }

    return (
        <div className="w-full flex items-center justify-center">
            <motion.div
                className="w-4/5 max-w-2xl space-y-6"
                initial={{ opacity: 0 }}
                animate={{ opacity: 1 }}
                transition={{ duration: 0.5 }}
            >
                <h1 className="text-2xl font-bold text-center">
                    Add product
                </h1>
                <div className="w-full flex justify-center">
                    <AutoForm
                        className="w-full space-y-6"
                        onSubmit={onSubmit}
                        formSchema={formSchema}
                        fieldConfig={{
                            title: {
                                inputProps: {
                                    placeholder: 'Enter the title'
                                }
                            },
                            description: {
                                fieldType: 'textarea',
                                inputProps: {
                                    placeholder: 'Enter product description'
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
                        <div className="flex justify-center">
                            <AutoFormSubmit className="w-1/2">
                                Add product
                            </AutoFormSubmit>
                        </div>
                    </AutoForm>
                </div>
            </motion.div>
        </div>
    );
}