"use client"

import {
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableHeader,
    TableRow,
} from "@/components/shadcn/table"
import * as React from "react";
import {Accordion, AccordionItem} from "@nextui-org/react";
import {Chip} from "@nextui-org/chip";
import {Avatar, AvatarImage} from "@/components/shadcn/avatar";
import {Link} from "@nextui-org/link";

type Product = {
    id: number
    name: string
    quantity: number
    price: number
}

type Order = {
    id: number
    date: string
    customer: {
        username: string
        avatar: string
    }
    products: Product[]
    total: number
    status: "In progress" | "Shipped" | "Delivered"
}

const orders: Order[] = [
    {
        id: 1,
        date: "2023-11-09",
        customer: {
            username: "Alice Dupont",
            avatar: "https://ui.shadcn.com/avatars/02.png",
        },
        products: [
            { id: 1, name: "Pomme de terre", quantity: 2, price: 19.99 },
            { id: 2, name: "Red bull", quantity: 1, price: 49.99 },
        ],
        total: 89.97,
        status: "In progress",
    },
    {
        id: 2,
        date: "2023-11-08",
        customer: {
            username: "Bob Martin",
            avatar: "https://ui.shadcn.com/avatars/03.png",
        },
        products: [
            { id: 3, name: "Pain au raisin", quantity: 1, price: 79.99 },
        ],
        total: 79.99,
        status: "Shipped",
    },
    {
        id: 3,
        date: "2023-11-07",
        customer: {
            username: "Claire Dubois",
            avatar: "https://ui.shadcn.com/avatars/04.png",
        },
        products: [
            { id: 4, name: "Café", quantity: 1, price: 99.99 },
            { id: 5, name: "Gingembre", quantity: 2, price: 14.99 },
        ],
        total: 129.97,
        status: "Delivered",
    },
]

export default function OrdersPage() {
    return (
        <div className="w-full p-5 space-y-4">
            <Accordion className="w-full border rounded-md p-1 px-4">
                {orders.map((order) => (
                    <AccordionItem
                        key={order.id}
                        value={`order-${order.id}`}
                        title={
                            <div className={"grid grid-cols-6 w-full text-sm"}>
                                <span>Order #{order.id}</span>
                                <span>{order.date}</span>
                                <Link href={"/dashboard"} size={"sm"} className={"w-fit group flex items-center gap-x-2"}>
                                    <Avatar className="w-8 h-8 rounded-lg object-contain" asChild>
                                        <AvatarImage src={order.customer.avatar} alt={order.customer.username} />
                                    </Avatar>
                                    <span className={"group-hover:underline"}>{order.customer.username}</span>
                                </Link>
                                <span className="flex justify-end">
                                    <Chip size="sm"
                                          color={`${order.status === 'In progress' ? 'default' : order.status === 'Shipped' ? 'warning' : 'success'}`}
                                          variant="flat">
                                        {order.status}
                                    </Chip>
                                </span>
                                <span className="text-right">{order.total.toFixed(2)}€</span>
                            </div>
                        }
                    >
                        <Table>
                            <TableHeader>
                                <TableRow>
                                    <TableHead>Product</TableHead>
                                    <TableHead>Quantity</TableHead>
                                    <TableHead>Unit price</TableHead>
                                    <TableHead className="text-right">Total</TableHead>
                                </TableRow>
                            </TableHeader>
                            <TableBody>
                                {order.products.map((product) => (
                                    <TableRow key={product.id}>
                                        <TableCell>{product.name}</TableCell>
                                        <TableCell>{product.quantity}</TableCell>
                                        <TableCell>{product.price.toFixed(2)}€</TableCell>
                                        <TableCell className="text-right">
                                            {(product.quantity * product.price).toFixed(2)}€
                                        </TableCell>
                                    </TableRow>
                                ))}
                                <TableRow>
                                    <TableCell colSpan={3} className="font-bold">Order total</TableCell>
                                    <TableCell className="text-right font-bold">{order.total.toFixed(2)}€</TableCell>
                                </TableRow>
                            </TableBody>
                        </Table>
                    </AccordionItem>
                ))}
            </Accordion>
        </div>
    )
}