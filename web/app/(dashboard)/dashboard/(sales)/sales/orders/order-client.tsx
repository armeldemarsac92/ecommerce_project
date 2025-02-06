"use client"

import {Table, TableBody, TableCell, TableHead, TableHeader, TableRow,} from "@/components/shadcn/table"
import * as React from "react";
import {Accordion, AccordionItem, Pagination, Spinner} from "@nextui-org/react";
import {Chip} from "@nextui-org/chip";
import {Avatar, AvatarImage} from "@/components/shadcn/avatar";
import {Link} from "@nextui-org/link";
import {SearchBar} from "@/components/ui/search-bar";
import {useToast} from "@/hooks/use-toast";
import {useCallback, useEffect, useMemo, useState} from "react";
import {useOrders} from "@/hooks/swr/orders/use-orders";
import {useCustomers} from "@/hooks/swr/customers/use-customers";
import {Button} from "@/components/shadcn/button";
import {ChevronLeft, ChevronRight} from "lucide-react";

type PaymentStatus = 'processing' | 'pending' | 'succeeded' | 'failed' | 'canceled';

export const OrdersClient= () => {
    const [filterValue, setFilterValue] = useState("");
    const [page, setPage] = useState(1);
    const [rowsPerPage] = useState(10);
    const { toast } = useToast();
    const { orders, loadingSWROrders, errorSWROrders } = useOrders();
    const { customers, loadingSWRCustomers, errorSWRCustomers } = useCustomers();
    const paymentStatusColors = {
        processing: 'warning',
        pending: 'default',
        succeeded: 'success',
        failed: 'danger',
        canceled: 'default'
    } as const;

    const avatarUrls = [
        "https://ui.shadcn.com/avatars/02.png",
        "https://ui.shadcn.com/avatars/03.png",
        "https://ui.shadcn.com/avatars/04.png"
    ];

    useEffect(() => {
        if (errorSWRCustomers || errorSWROrders) {
            toast({ variant: "destructive", title: "Error", description: "An error occurred"})
        }
    }, [errorSWRCustomers, errorSWROrders]);

    const hasSearchFilter = Boolean(filterValue);

    const getAvatarByUsername = (username: string) => {
        const sum = username.split('').reduce((acc, char) => acc + char.charCodeAt(0), 0);
        const index = sum % avatarUrls.length;

        return avatarUrls[index];
    };

    const filteredItems = useMemo(() => {
        let filteredOrders = [...orders].map(order => {
            const matchingCustomer = customers.find(customer => customer.id === order.user_id)

            return {
                ...order,
                customer: {
                    username: matchingCustomer?.user_name || '-'
                }
            };
        });

        if (hasSearchFilter) {
            filteredOrders = filteredOrders.filter((order) =>
                order.customer.username.toLowerCase().startsWith(filterValue.toLowerCase())
            );
        }

        return filteredOrders;
    }, [orders, customers, filterValue]);

    const pages = Math.ceil(filteredItems.length / rowsPerPage);

    const paginatedItems = useMemo(() => {
        const start = (page - 1) * rowsPerPage;
        const end = start + rowsPerPage;

        return filteredItems.slice(start, end);
    }, [page, rowsPerPage, filteredItems]);

    const onNextPage = useCallback(() => {
        if (page < pages) {
            setPage(page + 1);
        }
    }, [page, pages]);

    const onPreviousPage = useCallback(() => {
        if (page > 1) {
            setPage(page - 1);
        }
    }, [page]);


    const onSearchChange = useCallback((value: string) => {
        if (value) {
            setFilterValue(value);
            setPage(1);
        } else {
            setFilterValue("");
        }
    }, []);

    const onClear = useCallback(() => {
        setFilterValue("");
        setPage(1);
    }, []);

    return (
        <>
            {loadingSWROrders || loadingSWRCustomers ? (
                <div className="h-full w-full flex justify-center">
                    <Spinner color="success" labelColor="success" />
                </div>) : (
            <div className="w-full p-5 space-y-4">
                <div>
                    <div className="flex justify-between gap-3 items-end">
                        <SearchBar onClear={onClear} onValueChange={onSearchChange} value={filterValue} />
                    </div>
                </div>
                <Accordion className="w-full border rounded-md p-1 px-4">
                    {paginatedItems.map((order) => (
                        <AccordionItem
                            key={order.order_id}
                            textValue={`order-${order.order_id}`}
                            title={
                                <div className={"grid grid-cols-6 w-full text-sm"}>
                                    <span>Order #{order.order_id}</span>
                                    <span>{order.created_at ? new Date(order.created_at).toISOString().split('T')[0] : "-"}</span>
                                    <div className="flex space-x-14">
                                        <Link href={"/dashboard"} size={"sm"} className={"w-fit group flex items-center gap-x-2"}>
                                            <Avatar className="w-8 h-8 rounded-lg object-contain" asChild>
                                                <AvatarImage
                                                    src={getAvatarByUsername(order.customer.username)}
                                                    alt={order.customer.username}
                                                />
                                            </Avatar>
                                            <span className="text-xs">{order.customer.username}</span>
                                        </Link>
                                        <span className="flex justify-end items-center">
                                            <Chip size="sm"
                                                  color={paymentStatusColors[order.stripe_payment_status as PaymentStatus] || 'default'}
                                                  className="text-xs"
                                                  variant="flat"
                                            >
                                            {order.stripe_payment_status!= null ? order.stripe_payment_status.toUpperCase() : 'DRAFT'}
                                            </Chip>
                                        </span>
                                        <span className="flex items-center">{order.total_amount.toFixed(2)}€</span>
                                    </div>
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
                                    {order.order_items.map((product) => (
                                        <TableRow key={product.product_id}>
                                            <TableCell>{product.title}</TableCell>
                                            <TableCell>{product.quantity ? (product.quantity) : "-"}</TableCell>
                                            <TableCell>{product.unit_price ? product.unit_price + "€" : "-"}</TableCell>
                                            <TableCell className="text-right">
                                                {(product.quantity * product.unit_price).toFixed(2)}€
                                            </TableCell>
                                        </TableRow>
                                    ))}
                                    <TableRow>
                                        <TableCell
                                            colSpan={3}
                                            className="font-bold"
                                        >
                                            Order total
                                        </TableCell>
                                        <TableCell className="text-right font-bold">{order.total_amount.toFixed(2)}€</TableCell>
                                    </TableRow>
                                </TableBody>
                            </Table>
                        </AccordionItem>
                    ))}
                </Accordion>

                <div className="py-2 px-2 flex justify-between items-center m-2">
                    <Pagination
                        classNames={{
                            cursor: "bg-secondary rounded-sm",
                            prev: "rounded-sm",
                            next: "rounded-sm",
                        }}
                        radius={"sm"}
                        isCompact
                        showControls
                        showShadow
                        page={page}
                        total={pages}
                        onChange={setPage}
                    />
                    <div className="hidden sm:flex w-[30%] justify-end gap-2">
                        <Button
                            variant={"expandIcon"}
                            iconPlacement={"left"}
                            Icon={<ChevronLeft size={15}/>}
                            disabled={page === 1}
                            size="sm"
                            onClick={onPreviousPage}
                        >
                            Previous
                        </Button>
                        <Button
                            variant={"expandIcon"}
                            iconPlacement={"right"}
                            Icon={<ChevronRight size={15}/>}
                            disabled={page === pages}
                            size="sm"
                            onClick={onNextPage}
                        >
                            Next
                        </Button>
                    </div>
                </div>
            </div>
            )}
        </>
    )
}