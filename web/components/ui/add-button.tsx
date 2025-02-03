"use client"

import {Link} from "@nextui-org/link";
import {Button} from "@/components/shadcn/button";
import {Plus} from "lucide-react";

export const AddButton = () => {
    return (
        <div className="relative">
            <Link href="/dashboard/catalog/inventory/products/search">
                <Button variant={"expandIcon"} iconPlacement={"left"} Icon={Plus} size={"sm"}>
                    Add product
                </Button>
            </Link>
        </div>
    )
}