import useSWR from "swr";
import {axiosSWRFetcher} from "@/utils/fetcher";
import {useEffect, useState} from "react";
import {Order} from "@/types/order/order";

export const useOrders = () => {
    const { data , mutate, isLoading: loadingSWROrders, error: errorSWROrders } = useSWR('/orders', axiosSWRFetcher, {
        revalidateOnFocus: true,
        revalidateOnReconnect: true,
        refreshInterval: 60000
    });

    const [orders, setOrders] = useState<Order[]>([]);

    useEffect(() => {
        if (data) {
            setOrders(data);
        }
    }, [data]);

    return {
        orders,
        refreshOrders: () => mutate(),
        loadingSWROrders,
        errorSWROrders,
        isError: !!errorSWROrders
    }
}