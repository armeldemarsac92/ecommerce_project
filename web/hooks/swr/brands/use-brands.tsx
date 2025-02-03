import useSWR from "swr";
import {axiosSWRFetcher} from "@/utils/fetcher";
import {useEffect, useState} from "react";
import {Brand} from "@/types/brand/brand";

export const useBrands = () => {
    const { data, mutate, isLoading: loadingSWRBrands, error: errorSWRBrands } = useSWR('/brands', axiosSWRFetcher, {
        revalidateOnFocus: true,
        revalidateOnReconnect: true,
        refreshInterval: 60000
    });

    const [brands, setBrands] = useState<Brand[]>([]);

    useEffect(() => {
        if (data) {
            setBrands(data)
        }
    }, [data]);

    return {
        brands,
        refreshCategories: ()  => mutate('/brands'),
        loadingSWRBrands,
        errorSWRBrands,
        isError: !!errorSWRBrands
    }
}