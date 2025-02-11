"use client"

import { Users } from "lucide-react";
import NumberTicker from "@/components/shadcn/number-ticker";
import { Tab, Tabs } from "@nextui-org/react";
import { useState } from "react";
import useSWR from "swr";
import { axiosSWRFetcher } from "@/utils/fetcher";
import { Spinner } from "@nextui-org/spinner";

export function UsersNumberCard() {
    const [dateRange, setDateRange] = useState("daily");

    const { data: statUser, isLoading } = useSWR(
        `/stats/new_users/${dateRange}`,
        axiosSWRFetcher,
        {
            revalidateOnFocus: true,
            revalidateOnReconnect: true,
            refreshInterval: 60000
        }
    );

    return (
        <div className="flex flex-col justify-between aspect-video rounded-xl bg-white p-5">
            <div>
                <div className="flex justify-between items-center">
                    <h3 className="text-muted-foreground">New Users</h3>
                    <Users className="text-icon" size={20}/>
                </div>
                <div className="text-3xl font-bold">
                    {isLoading ? (
                        <Spinner size="sm"/>
                    ) : (
                        typeof statUser === 'number' ? (
                            statUser === 0 ? (
                                "0"
                            ) : (
                                <NumberTicker value={statUser}/>
                            )
                        ) : (
                            <Spinner size="sm"/>
                        )
                    )}
                </div>
            </div>

            <div>
                <Tabs
                    variant="underlined"
                    defaultSelectedKey="daily"
                    aria-label="Options"
                    classNames={{
                        tabList: "gap-6 w-full relative rounded-none p-0 border-b border-divider",
                        cursor: "w-full bg-success",
                        tab: "max-w-fit px-0 h-12",
                        tabContent: "group-data-[selected=true]:text-success"
                    }}
                    onSelectionChange={(key) => setDateRange(key.toString())}
                >
                    <Tab key="daily" title="Today"/>
                    <Tab key="monthly" title="Monthly"/>
                </Tabs>
            </div>
        </div>
    );
}