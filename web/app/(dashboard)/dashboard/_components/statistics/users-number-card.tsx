"use client"

import { Users } from "lucide-react";
import NumberTicker from "@/components/shadcn/number-ticker";
import { Tab, Tabs } from "@nextui-org/react";
import {useEffect} from "react";
import {useAppContext} from "@/contexts/app-context";

export function UsersNumberCard() {
  return (
    <div className="flex flex-col justify-between aspect-video rounded-xl bg-white p-5">
      <div>
        <div className={"flex justify-between items-center"}>
          <h3 className={"text-muted-foreground"}>New Users</h3>

          <Users className={"text-icon"} size={20}/>
        </div>
        <p className={"text-3xl font-bold"}>
          <NumberTicker value={983}/>
        </p>
      </div>

      <div>
        <Tabs
          variant={"underlined"}
          defaultSelectedKey={'today'}
          aria-label="Options"
          classNames={{
            tabList: "gap-6 w-full relative rounded-none p-0 border-b border-divider",
            cursor: "w-full bg-success",
            tab: "max-w-fit px-0 h-12",
            tabContent: "group-data-[selected=true]:text-success"
          }}
        >
          <Tab key="today" title="Today"/>
          <Tab key="monthly" title="Monthly"/>
        </Tabs>
      </div>
    </div>
  )
}
