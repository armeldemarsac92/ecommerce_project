import useSWR from "swr";
import {axiosSWRFetcher} from "@/utils/fetcher";
import {useEffect, useState} from "react";
import {Customer} from "@/types/customer/customer";

export const useCustomers = () => {
    const { data, mutate, isLoading: loadingSWRCustomers, error: errorSWRCustomers } = useSWR('/customers', axiosSWRFetcher, {
        revalidateOnFocus: true,
        revalidateOnReconnect: true,
        refreshInterval: 60000
    });

    const [customers, setCustomers] = useState<Customer[]>([]);

    useEffect(() => {
        if (data) {
            setCustomers(data);
        }
    }, [data]);

    return {
        customers,
        refreshCustomers: () => mutate(),
        loadingSWRCustomers,
        errorSWRCustomers,
        isError: !!errorSWRCustomers,
    }
}