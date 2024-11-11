"use client"

import { useParams } from "next/navigation";
import { FormProductComponent } from "./_components/form";
import { DetailsProductComponent } from "./_components/details";

export default function InventoryProductIdPage() {
    const params = useParams();

    return (
        params.productId === 'new' ?
        (
            <FormProductComponent />
        )
        :
        (
            <DetailsProductComponent />
        )
    );
}
