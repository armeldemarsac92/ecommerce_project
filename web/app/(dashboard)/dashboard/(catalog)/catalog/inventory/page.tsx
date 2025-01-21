import {InventoryTable} from "@/app/(dashboard)/dashboard/(catalog)/catalog/inventory/_components/inventory-table";

export default function InventoryPage() {
    return (
        <div className="w-full h-full p-4 space-y-4">
            <div>
                <InventoryTable/>
            </div>
        </div>
    )
}