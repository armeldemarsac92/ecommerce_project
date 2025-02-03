import useSWR from 'swr'
import { axiosSWRFetcher } from "@/utils/fetcher";
import { useEffect, useState } from "react";
import {Product} from "@/types/product/product";

export const useProduct = (id: number) => {
    const { data, mutate, isLoading: loadingSWRProduct, error: errorSWRProduct } = useSWR(id ? `/products/${id}` : null, axiosSWRFetcher, {
        revalidateOnFocus: true,
        revalidateOnReconnect: true,
        refreshInterval: 60000
    });

    const [product, setProduct] = useState<Product | null>(null);



    useEffect(() => {
        if (data) {
            setProduct(data);
        }
    }, [data]);

    return {
        product,
        refreshProduct: () => mutate(`/product/${id}`),
        loadingSWRProduct,
        errorSWRProduct,
        isError: !!errorSWRProduct
    };
}