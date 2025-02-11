"use client"

import { ShoppingCart } from "lucide-react";
import NumberTicker from "@/components/shadcn/number-ticker";
import { Tab, Tabs } from "@nextui-org/react";
import { useState } from "react";

export function OrderAverageCard() {
    const [dateRange, setDateRange] = useState("daily");

  return (
    <div className="flex flex-col justify-between aspect-video rounded-xl bg-white p-5">
      <div>
        <div className={"flex justify-between items-center"}>
          <h3 className={"text-muted-foreground"}>Order Average</h3>

          <ShoppingCart className={"text-icon"} size={20} />
        </div>

        <div>
          <p className={"text-3xl font-bold"}>
              {dateRange === "monthly" ? (
              <>
                <NumberTicker value={1130} />€
              </>
            ) : (
              <>
                <NumberTicker value={30} />€
              </>
            )}
          </p>
          {/*<span className={`text-xs font-medium ${monthly ? "text-success" : "text-danger"}`}>
            {monthly ? "+" : "-"}
            {monthly ? <NumberTicker className={"text-success"} value={30} /> : <NumberTicker className={"text-danger"} value={2} />}%
            last 30 days
          </span>*/}
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
              <Tab key="daily" title="Today" />
              <Tab key="monthly" title="Monthly" />
          </Tabs>
      </div>
    </div>
  )
}
