"use client";

import {Card, CardBody, CardFooter, Image, Pagination} from "@nextui-org/react"
import { Chip } from "@nextui-org/chip";
import {SearchBar} from "@/components/ui/search-bar";
import React, {useEffect, useState, useCallback, useMemo} from "react";
import {Button} from "@/components/shadcn/button";
import {ChevronLeft, ChevronRight} from "lucide-react";
import {Spinner} from "@nextui-org/react";
import {Link} from "@nextui-org/link";
import {useToast} from "@/hooks/use-toast";
import {useProducts} from "@/hooks/swr/products/use-products";


export default function CatalogClient() {
    const [filterValue, setFilterValue] = useState("");
    const [page, setPage] = useState(1);
    const [rowsPerPage] = useState(10);
    const { toast } = useToast()

    const { products, errorSWRProducts, loadingSWRProducts } = useProducts();


    useEffect(() => {
        if (errorSWRProducts) {
            toast({ variant: "destructive", title: "Error", description: "An error occurred"})
        }
    }, []);

    const hasSearchFilter = Boolean(filterValue);

    const filteredItems = useMemo(() => {
        let filteredUsers = [...products];

        if (hasSearchFilter) {
            filteredUsers = filteredUsers.filter((product) =>
                product.title.toLowerCase().includes(filterValue.toLowerCase()),
            );
        }

        return filteredUsers;
    }, [products, filterValue]);

    const pages = Math.ceil(filteredItems.length / rowsPerPage);

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
        <div className="h-full">
            <div className="h-full flex flex-col">
                {loadingSWRProducts ? (
                    <div className="h-full w-full flex justify-center">
                        <Spinner color="success" labelColor="success" />
                    </div>
                ) : (
                    <>
                        <div className="m-4">
                            <SearchBar onClear={onClear} onValueChange={onSearchChange} value={filterValue}/>
                        </div>
                        <div className="gap-5 grid grid-cols-2 md:grid-cols-3 lg:grid-cols-5 p-4">
                            {filteredItems.map((product, index) => (
                                <Card
                                    shadow="sm"
                                    key={index}
                                    as={Link}
                                    href={`/dashboard/catalog/inventory/products/${product.id}`}
                                >
                                    <CardBody className="overflow-visible p-0">
                                        <Image
                                            shadow="sm"
                                            radius="lg"
                                            width="100%"
                                            alt={product.title}
                                            className="h-full object-contain"
                                            src={product.image || 'https://placehold.co/300x300'}
                                        />
                                    </CardBody>
                                    <CardFooter className="text-small justify-between">
                                        <span className="flex-1 text-left">{product.title}</span>
                                        <span className="text-default-500 font-bold">{product.price}€</span>
                                    </CardFooter>
                                    <Chip
                                        size="sm"
                                        color={`${product.stock > 0 ? 'success' : 'danger'}`}
                                        variant="flat"
                                        className="m-5 p-2"
                                    >
                                        {product.stock > 0 ? 'In stock' : 'Out stock'}
                                    </Chip>
                                </Card>
                            ))}
                        </div>
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
                                <Button variant={"expandIcon"} iconPlacement={"left"} Icon={ChevronLeft} disabled={pages === 1} size="sm"
                                        onClick={onPreviousPage}>
                                    Previous
                                </Button>
                                <Button variant={"expandIcon"} iconPlacement={"right"} Icon={ChevronRight} disabled={pages === 1} size="sm"
                                        onClick={onNextPage}>
                                    Next
                                </Button>
                            </div>
                        </div>
                    </>
                )}
            </div>
        </div>
    )
}
