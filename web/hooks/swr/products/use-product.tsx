import { axiosSWRFetcher } from "@/utils/fetcher";
import { useEffect, useState } from "react";
import useSWR from "swr";

export const useProducts = (id: number) => {
    const { data, mutate, isLoading: loadingSWRFood, error: errorSWRFood } = useSWR(id ? `/food/${id}` : null, axiosSWRFetcher, {
        revalidateOnFocus: true,
        revalidateOnReconnect: true,
        refreshInterval: 60000
    });

    const [food, setFood] = useState<FoodType | null>(null);

    useEffect(() => {
        if (data) {
            setFood(data.data);
        }
    }, [data]);

    return {
        food,
        refreshUser: () => mutate(`/user/${id}`),
        loadingSWRFood,
        errorSWRFood,
        isError: !!errorSWRFood
    };
}