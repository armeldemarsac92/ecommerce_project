import {axiosSWRFetcher} from "@/utils/fetcher";
import {useEffect, useState} from "react";
import {Product} from "@/types/product/product";
import useSWR from "swr";

export const useProducts = () => {
    const { data, mutate, isLoading: loadingSWRProducts, error: errorSWRProducts } = useSWR('/products', axiosSWRFetcher, {
        revalidateOnFocus: true,
        revalidateOnReconnect: true,
        refreshInterval: 60000
    });

    const [products, setProducts] = useState<Product[]>([]);

    useEffect(() => {
        if (data) {
            setProducts(data);
        }
    }, [data]);

    return {
        products,
        refreshProducts: () => mutate('/products'),
        loadingSWRProducts,
        errorSWRProducts,
        isError: !!errorSWRProducts
    }
}
