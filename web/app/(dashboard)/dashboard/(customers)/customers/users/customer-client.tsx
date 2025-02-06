"use client"

import {ChevronLeft, ChevronRight, Ellipsis, Eye, Hammer, Trash} from "lucide-react";
import React, {useEffect, useMemo, useState, useCallback} from "react";
import {Table, TableBody, TableCell, TableHead, TableHeader, TableRow} from "@/components/shadcn/table";
import {Chip} from "@nextui-org/chip";
import {DropdownMenu, DropdownMenuContent, DropdownMenuItem, DropdownMenuSeparator, DropdownMenuTrigger} from "@/components/shadcn/dropdown-menu";
import {Avatar, AvatarImage} from "@/components/shadcn/avatar";
import {useToast} from "@/hooks/use-toast";
import {useCustomers} from "@/hooks/swr/customers/use-customers";
import {Pagination, Spinner} from "@nextui-org/react";
import {SearchBar} from "@/components/ui/search-bar";
import {Button} from "@/components/shadcn/button";

export const CustomersUsersClient = ()=> {
    const [filterValue, setFilterValue] = useState("");
    const [rowsPerPage] = useState(10);
    const {toast} = useToast();
    const [page, setPage] = useState(1);
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
        let filteredCustomers = [...customers].map(customer => {
            return {
                ...customer,
            }
        })

        if (hasSearchFilter) {
            filteredCustomers = filteredCustomers.filter(customer => {
                customer.user_name.toLowerCase().includes(filterValue.toLowerCase());
            })
        }

        return filteredCustomers
    }, [customers, filterValue])

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
        <div className="h-full">
            <div className="w-full p-5 space-y-4">
                {loadingSWRCustomers ? (
                    <div className="h-full w-full flex justify-center">
                        <Spinner color="success" labelColor="success"/>
                    </div>
                ) : (
                    <>
                        <div className="flex justify-between">
                            <div className="w-full">
                                <SearchBar
                                    onClear={onClear}
                                    onValueChange={onSearchChange}
                                    value={filterValue}
                                />
                            </div>
                        </div>
                        <div className="border-1 rounded-md">
                            <Table>
                                <TableHeader>
                                    <TableRow>
                                        <TableHead>Avatar</TableHead>
                                        <TableHead>Username</TableHead>
                                        <TableHead>Email</TableHead>
                                        <TableHead>Phone</TableHead>
                                        <TableHead>Email Verified</TableHead>
                                        <TableHead>Role</TableHead>
                                        <TableHead>Status</TableHead>
                                        <TableHead>Actions</TableHead>
                                    </TableRow>
                                </TableHeader>
                                <TableBody>
                                    {paginatedItems.map((user) => (
                                        <TableRow key={user.user_name}>
                                            <TableCell>
                                                <Avatar className="w-8 h-8 rounded-lg object-contain" asChild>
                                                    <AvatarImage src={getAvatarByUsername(user.user_name)} alt={user.user_name} />
                                                </Avatar>
                                            </TableCell>
                                            <TableCell>{user.user_name ? user.user_name : "-"}</TableCell>
                                            <TableCell>{user.email ? user.phone_number : "-"}</TableCell>
                                            <TableCell>{user.phone_number ? user.phone_number : "-"}</TableCell>
                                            <TableCell>
                                                <Chip
                                                    size="sm"
                                                    color={user.email_confirmed ? 'success' : 'danger'}
                                                    variant="flat"
                                                >
                                                    {user.email_confirmed ? 'Verified' : 'Not verified'}
                                                </Chip>
                                            </TableCell>
                                            <TableCell>{user.role}</TableCell>
                                            {/*<TableCell>*/}
                                            {/*    <Chip*/}
                                            {/*        size="sm"*/}
                                            {/*        color={user.status === 'Active' ? 'success' : 'danger'}*/}
                                            {/*        variant="flat"*/}
                                            {/*    >*/}
                                            {/*        {user.status}*/}
                                            {/*    </Chip>*/}
                                            {/*</TableCell>*/}
                                            <TableCell>
                                                <DropdownMenu>
                                                    <DropdownMenuTrigger asChild
                                                                         className="hover:opacity-50 hover:cursor-pointer">
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
                                            </TableCell>
                                        </TableRow>
                                    ))}
                                </TableBody>
                            </Table>
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
                    </>
                )}
            </div>
        </div>
    );
}