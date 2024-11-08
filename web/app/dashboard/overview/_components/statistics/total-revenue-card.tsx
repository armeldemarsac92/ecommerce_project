"use client"

import { Tabs, TabsList, TabsTrigger } from "@/components/shadcn-ui/tabs";
import { Euro } from "lucide-react";
import NumberTicker from "@/components/shadcn-ui/number-ticker";

export function TotalRevenueCard() {
  return (
    <div className="flex flex-col justify-between aspect-video rounded-xl bg-white p-5">
      <div className={"space-y-1.5"}>
        <div className={"flex justify-between items-center"}>
          <h3 className={"text-muted-foreground"}>Total Revenue</h3>

          <Euro className={"text-muted-foreground"} size={20}/>
        </div>
        <p className={"text-3xl font-bold"}>
          <NumberTicker value={11356}/>€
        </p>
      </div>

      <div>
        <Tabs defaultValue="today">
          <TabsList className="grid w-full grid-cols-2">
            <TabsTrigger className={"text-xs"} value="today">Today</TabsTrigger>
            <TabsTrigger className={"text-xs"} value="month">Month</TabsTrigger>
          </TabsList>
        </Tabs>
      </div>
    </div>
  )
}
