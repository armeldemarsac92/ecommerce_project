"use client";

import * as React from "react";
import { Card, CardContent } from "@/components/shadcn/card";
import {
    Carousel,
    CarouselContent,
    CarouselItem,
    CarouselNext,
    CarouselPrevious,
} from "@/components/shadcn/carousel";
import {Image} from "@nextui-org/react";
import { z } from "zod";
import {motion} from "framer-motion";
import AutoForm, {AutoFormSubmit} from "@/components/ui/auto-form";


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

export const DetailsProductComponent = () => {
    const [isHovered, setIsHovered] = React.useState(false)

    const product = {
        id: 1,
        title: 'Coca Cola',
        price: '1.50',
        stock: 10,
        image: ['/images/coca-cola.jpg',
                '/images/fanta.jpeg',
                '/images/banana.jpeg',
        ],
        description: 'Coca-Cola, or Coke, is a carbonated soft drink manufactured by The Coca-Cola Company. Originally marketed as a temperance drink and intended as a patent medicine, it was invented in the late 19th century by John Stith Pemberton and was bought out by businessman Asa Griggs Candler, whose marketing tactics led Coca-Cola to its dominance of the world soft-drink market throughout the 20th century.',
        category: 'Drink',
        brand: 'Coca cola',
    }

    const formSchema = z.object({
        image: z.string().describe('Image'),
        title: z.string(({ required_error: 'Title is required' }))
            .describe('Title')
            .min(2, { message: 'Title must be at least 2 characters' })
            .default(product.title),
        description: z.string({ required_error: 'Description is required' })
            .describe('Description')
            .default(product.description),
        price: z.number({})
            .min(0, { message: 'Price must be greater than 0' })
            .describe('Price')
            .default(parseFloat(product.price)),
        category_id: z.nativeEnum(Category, { required_error: 'Category is required' })
            .describe('Category')
            .default(product.category as Category),
        brand_id: z.nativeEnum(Brand, { required_error: 'Brand is required' })
            .describe('Brand')
            .default(product.brand as Brand),
    });

    const onSubmit = (data: z.infer<typeof formSchema>) => {
        try {
            console.log(data);
        } catch (error) {
            console.error(error);
        }
    }

    return (
        <div className="grid m-5 grid-cols-1 lg:grid-cols-2">
            <div className="flex justify-center mb-6 lg:mb-0 items-center">
                <Carousel
                    className="w-full max-w-xs relative"
                    onMouseEnter={() => setIsHovered(true)}
                    onMouseLeave={() => setIsHovered(false)}
                >
                    <CarouselContent>
                        {product.image.map((image, index) => (
                            <CarouselItem key={index}>
                                <div className="p-1">
                                    <Card>
                                        <CardContent className="flex aspect-square items-center justify-center p-6">
                                            <Image
                                                src={image}
                                                fallbackSrc={'/images/product-placeholder.jpeg'}
                                                className="h-[400px] object-contain"
                                                alt={product.title}
                                                width="100%"
                                            />
                                        </CardContent>
                                    </Card>
                                </div>
                            </CarouselItem>
                        ))}
                    </CarouselContent>
                    <div
                        className={`absolute top-1/2 -translate-y-1/2 left-2 transition-opacity duration-300 ${isHovered ? 'opacity-100' : 'opacity-0'}`}>
                        <CarouselPrevious/>
                    </div>
                    <div
                        className={`absolute top-1/2 -translate-y-1/2 right-2 transition-opacity duration-300 ${isHovered ? 'opacity-100' : 'opacity-0'}`}>
                        <CarouselNext/>
                    </div>
                </Carousel>
            </div>
            <div className="w-full flex items-center justify-center">
                <motion.div
                    className="w-4/5 max-w-2xl space-y-6"
                    initial={{opacity: 0}}
                    animate={{opacity: 1}}
                    transition={{duration: 0.5}}
                >
                    <div className="w-full flex justify-center">
                        <AutoForm
                            className="w-full space-y-6"
                            onSubmit={onSubmit}
                            formSchema={formSchema}
                            fieldConfig={{
                                image: {
                                    inputProps: {
                                        type: 'file',
                                    }
                                },
                                title: {
                                    inputProps: {
                                        placeholder: 'Enter product title'
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
                                <AutoFormSubmit className="w-1/2 bg-[#23aa49] hover:opacity-80">
                                    Save
                                </AutoFormSubmit>
                            </div>
                        </AutoForm>
                    </div>
                </motion.div>
            </div>
        </div>
    )
}
