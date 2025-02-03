"use client"

import {Chip, Pagination, SortDescriptor, Spinner, Table, TableBody, TableCell, TableColumn, TableHeader, TableRow, useDisclosure} from "@nextui-org/react";
import React, {useEffect, useMemo, useState, useCallback} from "react";
import {columns} from "@/app/(dashboard)/dashboard/(catalog)/catalog/inventory/_data/data";
import {SearchBar} from "@/components/ui/search-bar";
import {Button} from "@/components/shadcn/button";
import {ChevronLeft, ChevronRight, Edit, Ellipsis, Trash} from "lucide-react";
import {DropdownMenu, DropdownMenuContent, DropdownMenuItem, DropdownMenuSeparator, DropdownMenuTrigger} from "@/components/shadcn/dropdown-menu";
import {useProducts} from "@/hooks/swr/products/use-products";
import {useToast} from "@/hooks/use-toast";
import {deleteProduct} from "@/actions/product";
import {Modal, ModalBody, ModalContent, ModalFooter, ModalHeader} from "@nextui-org/modal";
import {useInventories} from "@/hooks/swr/inventories/use-inventories";
import {Input} from "@/components/shadcn/input";
import {increamentProduct, substractProduct} from "@/actions/inventory";

export const InventoryTableClient = () => {
    const [filterValue, setFilterValue] = useState("");
    const [rowsPerPage] = useState(10);
    const { toast } = useToast();
    const [sortDescriptor, setSortDescriptor] = useState<SortDescriptor>({
        column: "title",
        direction: "ascending",
    });
    const {isOpen, onOpen, onClose} = useDisclosure();
    const {isOpen: isQuantityModalOpen, onOpen: onQuantityModalOpen, onClose: onQuantityModalClose} = useDisclosure();
    const [productIdToDelete, setProductIdToDelete] = useState<number|null>(null);
    const [page, setPage] = useState(1);
    const { products, refreshProducts, errorSWRProducts, loadingSWRProducts } = useProducts();
    const { inventories, refreshInventories, errorSWRInventories, loadingSWRInventories } = useInventories();
    const [selectedInventory, setSelectedInventory] = useState<{
        productId: number;
        currentQuantity: number;
    } | null>(null);
    const [quantityToAdd, setQuantityToAdd] = useState(0);


    useEffect(() => {
      if (errorSWRProducts || errorSWRInventories) {
          toast({ variant: "destructive", title: "Error", description: "An error occurred"})
      }
    }, []);

    type Product = typeof products[0];

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

    const items = useMemo(() => {
        const start = (page - 1) * rowsPerPage;
        const end = start + rowsPerPage;

        return filteredItems.slice(start, end);
    }, [page, filteredItems, rowsPerPage]);

    const sortedItems = useMemo(() => {
        return [...items].sort((a: Product, b: Product) => {
            const first = a[sortDescriptor.column as keyof Product] as number;
            const second = b[sortDescriptor.column as keyof Product] as number;
            const cmp = first < second ? -1 : first > second ? 1 : 0;

            return sortDescriptor.direction === "descending" ? -cmp : cmp;
        });
    }, [sortDescriptor, items]);

    const deleteInventoryProduct =  () => {
        if (productIdToDelete === null) {
            toast({
                variant: "destructive",
                title: "Error",
                description: "Any products targeted"
            });
        } else {
            deleteProduct(productIdToDelete).then((response) => {
                if (response.status === 204) {
                    toast({
                        variant: "success",
                        title: "Success",
                        description: "Product successfully deleted"
                    });
                    refreshProducts().then(() => {
                        onClose()
                        setProductIdToDelete(null);
                    }).catch(() => {
                        toast({
                            variant: "destructive",
                            title: "Error",
                            description: "An error occurred"
                        });
                    })
                }
            })
            .catch(() => {
                toast({
                    variant: "destructive",
                    title: "Error",
                    description: "An error occurred"
                });
            })
        }
    };

    const updateProductQuantity = (quantity: number) => {
        if (selectedInventory) {
            if (quantity > 0) {
                increamentProduct(selectedInventory.productId, quantity).then(() => {
                    refreshInventories().then(() => {
                        onQuantityModalClose();
                        toast({
                            variant: "success",
                            title: "Success",
                            description: "Stock successfully updated"
                        });
                    })
                        .catch(() => {
                            toast({
                                variant: "destructive",
                                title: "Error",
                                description: "An error occurred"
                            });
                        })
                })
                    .catch(() => {
                        toast({
                            variant: "destructive",
                            title: "Error",
                            description: "An error occurred"
                        });
                    })
            }
            else if (quantity < 0) {
                substractProduct(selectedInventory.productId, Math.abs(quantity)).then(() => {
                    refreshInventories().then(() => {
                        onQuantityModalClose();
                        toast({
                            variant: "success",
                            title: "Success",
                            description: "Stock successfully updated"
                        });
                    })
                        .catch(() => {
                            toast({
                                variant: "destructive",
                                title: "Error",
                                description: "An error occurred"
                            });
                        })
                })
                    .catch(() => {
                        toast({
                            variant: "destructive",
                            title: "Error",
                            description: "An error occurred"
                        });
                    })
            }
            else {
                toast({
                    variant: "destructive",
                    title: "Error",
                    description: "An error occurred"
                });
            }
        }
    }

    const renderCell = useCallback((product: Product, columnKey: React.Key) => {
        const cellValue = product[columnKey as keyof Product];
        const productInventory = inventories.find(inv => inv.product_id === product.id);

        switch (columnKey) {
            case "sku":
                return productInventory?.sku || "-";
            case "stock":
                const quantity = productInventory?.quantity ?? 0;

                return (
                    <Chip size="sm" className={"rounded-sm"} color={`${quantity > 0 ? 'success' : 'danger'}`} variant="flat">
                        {quantity > 0 ? 'In stock' : 'Out stock'}
                    </Chip>
                );
            case "actions":
                return (
                    <div className="relative flex justify-center items-center gap-2">
                        <DropdownMenu>
                            <DropdownMenuTrigger asChild className="hover:opacity-50 hover:cursor-pointer">
                                <div className={"py-2"}>
                                    <Ellipsis className="size-4" />
                                </div>
                            </DropdownMenuTrigger>
                            <DropdownMenuContent side="right" align="start" className="w-56">
                                <DropdownMenuItem className={"hover:cursor-pointer"} onClick={() => {
                                    const productInventory = inventories.find(inv => inv.product_id === product.id);

                                    setSelectedInventory({
                                        productId: product.id,
                                        currentQuantity: productInventory?.quantity ?? 0,
                                    });
                                    setQuantityToAdd(0);
                                    onQuantityModalOpen();
                                }}>
                                    <Edit className="mr-2 size-4" />
                                    <span>Manage quantity</span>
                                </DropdownMenuItem>
                                <DropdownMenuSeparator />
                                <DropdownMenuItem className={"hover:cursor-pointer"}  onClick={() => {
                                    onOpen()
                                    setProductIdToDelete(product.id)
                                }}>
                                    <Trash className="mr-2 size-4" />
                                    <span>Delete</span>
                                </DropdownMenuItem>
                            </DropdownMenuContent>
                        </DropdownMenu>
                    </div>
                );
            default:
                return cellValue;
        }
    }, [inventories]);

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

    const onSearchChange = useCallback((value?: string) => {
        if (value) {
            setFilterValue(value);
            setPage(1);
        } else {
            setFilterValue("");
        }
    }, []);

    const onClear = useCallback(()=>{
        setFilterValue("")
        setPage(1)
    },[])

    const topContent = useMemo(() => {
        return (
            <div className="flex flex-col gap-4">
                <div className="flex justify-between gap-3 items-end">
                    <SearchBar onClear={onClear} onValueChange={onSearchChange} value={filterValue}/>
                </div>

                <div className="flex justify-between items-center">
                    <span className="text-default-400 text-small">Total {products.length} products</span>
                    <span className="flex items-center gap-x-1 text-default-400 text-small">
                        Rows per page: 10
                    </span>
                </div>
            </div>
        );
    }, [
        filterValue,
        onSearchChange,
        products.length,
        hasSearchFilter,
    ]);

    const bottomContent = React.useMemo(() => {
        return (
            <div className="py-2 px-2 flex justify-between items-center">
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
                    <Button variant={"expandIcon"} iconPlacement={"left"} Icon={<ChevronRight size={15}/>} disabled={pages === 1} size="sm" onClick={onPreviousPage}>
                        Previous
                    </Button>
                    <Button variant={"expandIcon" } iconPlacement={"right"} Icon={<ChevronRight size={15}/>} disabled={pages === 1} size="sm" onClick={onNextPage}>
                        Next
                    </Button>
                </div>
            </div>
        );
    }, [items.length, page, pages, hasSearchFilter]);

    return (
        <>
            <div className="h-full" data-cy="inventory-table">
                {!loadingSWRProducts && !loadingSWRInventories ? (
                <Table
                    aria-label="Table of products"
                    isHeaderSticky
                    bottomContent={bottomContent}
                    bottomContentPlacement="outside"
                    classNames={{
                        thead: "[&>tr]:first:rounded-sm",
                        wrapper: "p-0 max-h-[600px]",
                    }}
                    sortDescriptor={sortDescriptor}
                    topContent={topContent}
                    topContentPlacement="outside"
                    onSortChange={setSortDescriptor}
                    radius={"sm"}
                    shadow={"none"}
                >
                    <TableHeader columns={columns}>
                        {(column) => (
                            <TableColumn
                                key={column.uid}
                                align={column.uid === "actions" ? "center" : "start"}
                                allowsSorting={column.sortable}
                            >
                                {column.name}
                            </TableColumn>
                        )}
                    </TableHeader>
                    <TableBody emptyContent={"No products found"} items={sortedItems}>
                        {(item) => (
                            <TableRow key={item.id}>
                                {(columnKey) => <TableCell>{renderCell(item, columnKey)}</TableCell>}
                            </TableRow>
                        )}
                    </TableBody>
                </Table>
                ) : (
                    <div className="w-full h-full flex justify-center items-center">
                        <Spinner color="success" labelColor="success" />
                    </div>
                )}
            </div>
            <Modal isOpen={isOpen} onClose={onClose}>
                <ModalContent>
                    {(onClose) => (
                        <div>
                            <ModalHeader className="flex flex-col gap-1 text-left">Delete confirmation</ModalHeader>
                            <ModalBody>Are you sure to delete this product ?</ModalBody>
                            <ModalFooter>
                                <Button variant="outline" onClick={onClose}>
                                    Cancel
                                </Button>
                                <Button variant="destructive" onClick={deleteInventoryProduct}>
                                    Delete
                                </Button>
                            </ModalFooter>
                        </div>
                    )}
                </ModalContent>
            </Modal>
            <Modal isOpen={isQuantityModalOpen} onClose={onQuantityModalClose}>
                <ModalContent>
                    {(onQuantityModalClose) => (
                        <div>
                            <ModalHeader className="flex flex-col gap-1 text-left">Manage product quantity</ModalHeader>
                            <ModalBody>
                                <div className="space-y-4">
                                    <div className="flex items-center justify-between">
                                        <span>Current Quantity:</span>
                                        <span className="font-bold">
                                            {selectedInventory?.currentQuantity ?? 0}
                                        </span>
                                    </div>

                                    <div className="space-y-2">
                                        <Input
                                            type="number"
                                            value={quantityToAdd}
                                            onChange={(e) => {
                                                const value = parseInt(e.target.value);

                                                setQuantityToAdd(isNaN(value) ? 0 : value);
                                            }}
                                            placeholder="Enter quantity to add or remove"
                                        />
                                        <p className="text-sm text-gray-500">
                                            Use positive numbers to add, negative to remove
                                        </p>
                                    </div>

                                    <div className="flex items-center justify-between">
                                        <span>New Total:</span>
                                        <span className="font-bold">
                                            {(selectedInventory?.currentQuantity ?? 0) + quantityToAdd}
                                        </span>
                                    </div>
                                </div>
                            </ModalBody>
                            <ModalFooter>
                                <Button variant="outline" onClick={onQuantityModalClose}>
                                    Cancel
                                </Button>
                                <Button
                                    className="bg-secondary"
                                    onClick={() => updateProductQuantity(quantityToAdd)}
                                >
                                    Update
                                </Button>
                            </ModalFooter>
                        </div>
                    )}
                </ModalContent>
            </Modal>
        </>
    );
}