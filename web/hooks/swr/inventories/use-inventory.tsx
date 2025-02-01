import useSWR from "swr";
import {axiosSWRFetcher} from "@/utils/fetcher";
import {useEffect, useState} from "react";
import {Inventory} from "@/types/inventory/inventory";

export const useInventory = (id: number) => {
    const { data, mutate, isLoading: loadingSWRInventory, error: errorSWRInventory } = useSWR(id ? `/inventories/${id}` : null, axiosSWRFetcher, {
        revalidateOnFocus: true,
        revalidateOnReconnect: true,
        refreshInterval: 60000
    });

    const [inventory, setInventory] = useState<Inventory | null>(null);

    useEffect(() => {
        if (data) {
            setInventory(data)
        }
    }, [data]);

    return {
        inventory,
        refreshInventory: () => mutate(),
        loadingSWRInventory,
        errorSWRInventory,
        isError: !!errorSWRInventory
    };
}