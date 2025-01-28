import useSWR from "swr";
import {axiosSWRFetcher} from "@/utils/fetcher";
import {useEffect, useState} from "react";
import {Category} from "@/types/category/category";

export const useCategories = ()=> {
    const { data, mutate, isLoading: loadingSWRCategories, error: errorSWRCategories } = useSWR('/categories', axiosSWRFetcher, {
        revalidateOnFocus: true,
        revalidateOnReconnect: true,
        refreshInterval: 60000
    });

    const [categories, setCategories] = useState<Category[]>([]);

    useEffect(() => {
        if (data) {
            setCategories(data);
        }
    }, [data]);

    return {
        categories,
        refreshCategories: ()  => mutate(),
        loadingSWRCategories,
        errorSWRCategories,
        isError: !!errorSWRCategories
    }
}