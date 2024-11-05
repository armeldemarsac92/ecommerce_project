"use client"

import { Tabs, TabsList, TabsTrigger } from "@/components/shadcn-ui/tabs";
import { ShoppingCart } from "lucide-react";
import NumberTicker from "@/components/shadcn-ui/number-ticker";

export function CartAverageCard() {
  return (
    <div className="flex flex-col justify-between aspect-video rounded-xl bg-white p-5">
      <div className={"space-y-2"}>
        <div className={"flex justify-between items-center"}>
          <h3 className={"text-muted-foreground"}>Cart Average</h3>

          <ShoppingCart className={"text-muted-foreground"} size={20} />
        </div>

        <div>
          <p className={"text-3xl font-bold"}>
            <NumberTicker value={4123}/>
          </p>
          <span className={"text-xs font-medium text-green-600/80"}>+29% last 30 days</span>
        </div>
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
