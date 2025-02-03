import useSWR from "swr";
import {axiosSWRFetcher} from "@/utils/fetcher";
import {useEffect, useState} from "react";
import {Tag} from "@/types/tag/tag";

export const useTags= () => {
    const { data, mutate, isLoading: loadingSWRTags, error: errorSWRTags } = useSWR('/products_tags', axiosSWRFetcher, {
        revalidateOnFocus: true,
        revalidateOnReconnect: true,
        refreshInterval: 60000
    })

    const [tags, setTags] = useState<Tag[]>([]);

    useEffect(() => {
        if (data) {
            setTags(data);
        }
    }, [data]);

    return {
        tags,
        refreshTags: () => mutate(),
        loadingSWRTags,
        errorSWRTags,
        isError: !!errorSWRTags
    }
}