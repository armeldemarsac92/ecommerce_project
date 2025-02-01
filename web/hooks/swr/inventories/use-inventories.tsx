import useSWR from "swr";
import {axiosSWRFetcher} from "@/utils/fetcher";
import {Inventory} from "@/types/inventory/inventory";
import {useEffect, useState} from "react";

export const useInventories = () => {
    const { data, mutate, isLoading: loadingSWRInventories, error: errorSWRInventories } = useSWR('/inventories', axiosSWRFetcher, {
        revalidateOnFocus: true,
        revalidateOnReconnect: true,
        refreshInterval: 60000
    });

    const [inventories, setInventories] = useState<Inventory[]>([]);

    useEffect(() => {
        if (data) {
            setInventories(data);
        }
    }, [data]);

    return {
        inventories,
        refreshInventories: () => mutate(),
        loadingSWRInventories,
        errorSWRInventories,
        isError: !!errorSWRInventories,
    }
}