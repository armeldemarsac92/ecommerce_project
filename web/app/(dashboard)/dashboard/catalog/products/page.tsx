import {
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableHeader,
    TableRow,
} from "@/components/shadcn/table";
import { Input} from "@/components/shadcn/input";
import { Search, Plus, Trash, Edit, Ellipsis } from "lucide-react";
import { Button } from "@/components/shadcn/button";
import {
    DropdownMenu,
    DropdownMenuItem,
    DropdownMenuSeparator,
    DropdownMenuTrigger
} from "@/components/shadcn/dropdown-menu";
import { DropdownMenuContent } from "@/components/shadcn/dropdown-menu";
import { Chip } from "@nextui-org/chip";
import {Link} from "@nextui-org/link";

const products = [
    {
        logo: "",
        name: "Coca cola",
        sku: "15423145",
        price: "2€",
        stock: true,
        category: "Drink"
    },
    {
        logo: "",
        name: "Bacon",
        sku: "49846233145",
        price: "4€",
        stock: false,
        category: "Meat"
    },
    {
        logo: "",
        name: "Water 1L",
        sku: "154231454215",
        price: "1€",
        stock: true,
        category: "Drink"
    },
    {
        logo: "",
        name: "Pizza express margherita",
        sku: "1545223145",
        price: "5€",
        stock: true,
        category: "Pizzas pies and quiches"
    },
    {
        logo: "",
        name: "Dark chocolate 65% cocoa",
        sku: "15423145",
        price: "2€",
        stock: true,
        category: "Snacks"
    }
];

export default function Product() {

    const formatStock = (stock: boolean) => {
        if (stock)
            return 'In stock'
        else
            return 'Out stock'
    }

    return (
        <div className="w-full p-5 space-y-4">
            <div className="flex justify-between">
                <div className="w-1/6 relative">
                    <Search className="absolute left-2.5 top-1/2 h-4 w-4 -translate-y-1/2 text-gray-500 dark:text-gray-400"/>
                    <Input
                        type="search"
                        placeholder="Search..."
                        className="pl-8"
                    />
                </div>
                <div className="relative">
                    <Link href="/dashboard/catalog/products/new">
                        <Button>
                            <Plus className="mr-2 h-4 w-4" />
                            Add product
                        </Button>
                    </Link>
                </div>
            </div>
            <div className="border-1 rounded-md">
                <Table>
                    <TableHeader>
                        <TableRow>
                            <TableHead></TableHead>
                            <TableHead>Name</TableHead>
                            <TableHead>SKU</TableHead>
                            <TableHead>Price</TableHead>
                                <TableHead>Stock</TableHead>
                                <TableHead>Category</TableHead>
                                <TableHead>Actions</TableHead>
                        </TableRow>
                    </TableHeader>
                    <TableBody>
                        {products.map((product) => (
                            <TableRow key={product.sku}>
                                <TableCell>{product.logo}</TableCell>
                                <TableCell>{product.name}</TableCell>
                                <TableCell>{product.sku}</TableCell>
                                <TableCell>{product.price}</TableCell>
                                <TableCell>
                                    <Chip size="sm" color={`${product.stock ? 'success' : 'danger'}`} variant="flat">
                                        {formatStock(product.stock)}
                                    </Chip>
                                </TableCell>
                                <TableCell>{product.category}</TableCell>
                                <TableCell>
                                    <DropdownMenu>
                                        <DropdownMenuTrigger asChild className="hover:opacity-50 hover:cursor-pointer my-2">
                                                <Ellipsis className="size-4" />
                                        </DropdownMenuTrigger>
                                        <DropdownMenuContent side="right" align="start" className="w-56">
                                            <DropdownMenuItem>
                                                <Edit className="mr-2 size-4" />
                                                <span>Modify</span>
                                            </DropdownMenuItem>
                                            <DropdownMenuSeparator />
                                            <DropdownMenuItem>
                                                <Trash className="mr-2 size-4" />
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
    )
}