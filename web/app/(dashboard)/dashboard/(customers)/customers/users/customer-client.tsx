"use client"

import {ChevronLeft, ChevronRight, Ellipsis, Eye, Hammer, Trash} from "lucide-react";
import React, {useEffect, useMemo, useState, useCallback} from "react";
import {Pagination, SortDescriptor, Spinner, Table, TableBody, TableCell, TableColumn, TableHeader, TableRow} from "@nextui-org/react";
import {Chip} from "@nextui-org/chip";
import {DropdownMenu, DropdownMenuContent, DropdownMenuItem, DropdownMenuSeparator, DropdownMenuTrigger} from "@/components/shadcn/dropdown-menu";
import {Avatar, AvatarImage} from "@/components/shadcn/avatar";
import {useToast} from "@/hooks/use-toast";
import {useCustomers} from "@/hooks/swr/customers/use-customers";
import {SearchBar} from "@/components/ui/search-bar";
import {Button} from "@/components/shadcn/button";
import{columns} from "@/app/(dashboard)/dashboard/(customers)/customers/users/_data/data";

export const CustomersUsersClient = () => {
    const [filterValue, setFilterValue] = useState("");
    const [rowsPerPage] = useState(10);
    const {toast} = useToast();
    const [page, setPage] = useState(1);
    const [sortDescriptor, setSortDescriptor] = useState<SortDescriptor>({
        column: "user_name",
        direction: "ascending"
    });
    const {customers, loadingSWRCustomers, errorSWRCustomers} = useCustomers();

    const avatarUrls = [
        "https://ui.shadcn.com/avatars/02.png",
        "https://ui.shadcn.com/avatars/03.png",
        "https://ui.shadcn.com/avatars/04.png"
    ];

    useEffect(() => {
        if (errorSWRCustomers) {
            toast({variant: "destructive", title: "Error", description: "An error occurred"})
        }
    }, [errorSWRCustomers]);

    const hasSearchFilter = Boolean(filterValue);

    const getAvatarByUsername = (username: string) => {
        const sum = username.split('').reduce((acc, char) => acc + char.charCodeAt(0), 0);
        const index = sum % avatarUrls.length;

        return avatarUrls[index];
    };

    const filteredItems = useMemo(() => {
        let filteredCustomers = [...customers];

        if (hasSearchFilter) {
            filteredCustomers = filteredCustomers.filter((customer) =>
                customer.user_name.toLowerCase().includes(filterValue.toLowerCase()),
            );
        }

        return filteredCustomers;
    }, [customers, filterValue]);

    const pages = Math.ceil(filteredItems.length / rowsPerPage);

    const items = useMemo(() => {
        const start = (page - 1) * rowsPerPage;
        const end = start + rowsPerPage;

        return filteredItems.slice(start, end);
    }, [page, filteredItems, rowsPerPage]);

    type Customer = typeof customers[0];

    const sortedItems = useMemo(() => {
        return [...items].sort((a: Customer, b: Customer) => {
            const first = a[sortDescriptor.column as keyof Customer];
            const second = b[sortDescriptor.column as keyof Customer];
            const cmp = first < second ? -1 : first > second ? 1 : 0;

            return sortDescriptor.direction === "descending" ? -cmp : cmp;
        });
    }, [sortDescriptor, items]);

    const renderCell = useCallback((customer: Customer, columnKey: React.Key) => {
        switch (columnKey) {
            case "avatar":
                return (
                    <Avatar className="w-8 h-8 rounded-lg object-contain" asChild>
                        <AvatarImage src={getAvatarByUsername(customer.user_name)} alt={customer.user_name}/>
                    </Avatar>
                );
            case "email_confirmed":
                return (
                    <Chip
                        size="sm"
                        color={customer.email_confirmed ? 'success' : 'danger'}
                        variant="flat"
                    >
                        {customer.email_confirmed ? 'Verified' : 'Not verified'}
                    </Chip>
                );
            case "actions":
                return (
                    <div className="relative flex justify-center items-center gap-2">
                        <DropdownMenu>
                            <DropdownMenuTrigger asChild className="hover:opacity-50 hover:cursor-pointer">
                                <div className="py-2">
                                    <Ellipsis className="size-4"/>
                                </div>
                            </DropdownMenuTrigger>
                            <DropdownMenuContent side="right" align="start" className="w-56">
                                <DropdownMenuItem className="hover:cursor-pointer">
                                    <Eye className="size-4"/>
                                    <span>View</span>
                                </DropdownMenuItem>
                                <DropdownMenuSeparator/>
                                <DropdownMenuItem className="hover:cursor-pointer">
                                    <Hammer className="size-4"/>
                                    <span>Ban</span>
                                </DropdownMenuItem>
                                <DropdownMenuSeparator/>
                                <DropdownMenuItem className="hover:cursor-pointer">
                                    <Trash className="size-4"/>
                                    <span>Delete</span>
                                </DropdownMenuItem>
                            </DropdownMenuContent>
                        </DropdownMenu>
                    </div>
                );
            default:
                return customer[columnKey as keyof Customer] || "-";
        }
    }, []);

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

    const onClear = useCallback(() => {
        setFilterValue("");
        setPage(1);
    }, []);

    const topContent = useMemo(() => {
        return (
            <div className="flex flex-col gap-4">
                <div className="flex justify-between gap-3 items-end">
                    <SearchBar onClear={onClear} onValueChange={onSearchChange} value={filterValue}/>
                </div>

                <div className="flex justify-between items-center">
                    <span className="text-default-400 text-small">Total {customers.length} customers</span>
                    <span className="flex items-center gap-x-1 text-default-400 text-small">
                        Rows per page: {rowsPerPage}
                    </span>
                </div>
            </div>
        );
    }, [filterValue, onSearchChange, customers.length]);

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
        );
    }, [page, pages]);

    return (
        <div className="h-full">
            {loadingSWRCustomers ? (
                <div className="w-full h-full flex justify-center items-center">
                    <Spinner color="success" labelColor="success"/>
                </div>
            ) : (
                <div className="w-full p-5">
                    <Table
                        aria-label="Table of customers"
                        isHeaderSticky
                        bottomContent={bottomContent}
                        bottomContentPlacement="outside"
                        classNames={{
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
                        <TableBody emptyContent={"No customers found"} items={sortedItems}>
                            {(item) => (
                                <TableRow key={item.id}>
                                    {(columnKey) => <TableCell>{renderCell(item, columnKey)}</TableCell>}
                                </TableRow>
                            )}
                        </TableBody>
                    </Table>
                </div>
            )}
        </div>
    );
};