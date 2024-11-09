import {Ellipsis, Eye, Hammer, Search, Trash} from "lucide-react";
import {Input} from "@/components/shadcn/input";
import {Table, TableBody, TableCell, TableHead, TableHeader, TableRow} from "@/components/shadcn/table";
import {Chip} from "@nextui-org/chip";
import {
    DropdownMenu,
    DropdownMenuContent,
    DropdownMenuItem, DropdownMenuSeparator,
    DropdownMenuTrigger
} from "@/components/shadcn/dropdown-menu";
import {Avatar, AvatarImage} from "@/components/shadcn/avatar";
import * as React from "react";

export default function CustomersUsersPage() {

    return (
        <div className="w-full p-5 space-y-4">
            <div className="flex justify-between">
                <div className="w-2/6 relative">
                    <Search className="absolute left-2.5 top-1/2 h-4 w-4 -translate-y-1/2 text-gray-500 dark:text-gray-400"/>
                    <Input
                        type="search"
                        placeholder="Search..."
                        className="pl-8"

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
                        {fake_users_data.map((user) => (
                            <TableRow key={user.username}>
                                <TableCell>
                                    <Avatar className="w-8 h-8 rounded-lg object-contain" asChild>
                                        <AvatarImage src={user.avatar} alt={user.username} />
                                    </Avatar>
                                </TableCell>
                                <TableCell>{user.username}</TableCell>
                                <TableCell>{user.email}</TableCell>
                                <TableCell>{user.phone}</TableCell>
                                <TableCell>
                                    <Chip size="sm" color={`${user.email_verify ? 'success' : 'danger'}`} variant="flat">
                                        {user.email_verify ? 'Verified' : 'Not verified'}
                                    </Chip>
                                </TableCell>
                                <TableCell>{user.role}</TableCell>
                                <TableCell>
                                    <Chip size="sm" color={`${user.status === 'Active' ? 'success' : 'danger'}`} variant="flat">
                                        {user.status}
                                    </Chip>
                                </TableCell>
                                <TableCell>
                                    <DropdownMenu>
                                        <DropdownMenuTrigger asChild className="hover:opacity-50 hover:cursor-pointer">
                                            <div className={"py-2"}>
                                                <Ellipsis className="size-4" />
                                            </div>
                                        </DropdownMenuTrigger>
                                        <DropdownMenuContent side="right" align="start" className="w-56">
                                            <DropdownMenuItem className={"hover:cursor-pointer"}>
                                                <Eye className="size-4" />
                                                <span>View</span>
                                            </DropdownMenuItem>
                                            <DropdownMenuSeparator />
                                            <DropdownMenuItem className={"hover:cursor-pointer"}>
                                                <Hammer className="size-4" />
                                                <span>Ban</span>
                                            </DropdownMenuItem>
                                            <DropdownMenuSeparator />
                                            <DropdownMenuItem className={"hover:cursor-pointer"}>
                                                <Trash className="size-4" />
                                                <span>Delete</span>
                                            </DropdownMenuItem>
                                        </DropdownMenuContent>
                                    </DropdownMenu>
                                </TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </div>
        </div>
    );
}

const fake_users_data = [
    {
        avatar: "https://ui.shadcn.com/avatars/02.png",
        username: "Jesko",
        email: "developer@miamiam.com",
        phone: "0123456789",
        email_verify: true,
        status: "Active",
        role: "Developer",
    },
    {
        avatar: "https://ui.shadcn.com/avatars/03.png",
        username: "Drenyth",
        email: "drenyth@miammiam.com",
        phone: "0123456789",
        email_verify: false,
        status: "Active",
        role: "Developer",
    },
    {
        avatar: "https://ui.shadcn.com/avatars/04.png",
        username: "PasDinspi",
        email: "pasdinspi@miammiam.com",
        phone: "0123456789",
        email_verify: false,
        status: "Active",
        role: "Developer",
    },
    {
        avatar: "https://ui.shadcn.com/avatars/05.png",
        username: "Vidax",
        email: "vidax@miammiam.com",
        phone: "0123456789",
        email_verify: false,
        status: "Active",
        role: "Developer",
    },
    {
        avatar: "https://ui.shadcn.com/avatars/01.png",
        username: "Tommy",
        email: "tommy@gmail.com",
        phone: "0123456789",
        email_verify: true,
        status: "Blocked",
        role: "Customer",
    },
];
