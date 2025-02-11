"use client"

import { Euro } from "lucide-react";
import NumberTicker from "@/components/shadcn/number-ticker";
import { Tab, Tabs } from "@nextui-org/react";
import {useState} from "react";

export function TotalRevenueCard() {
    const [monthly, setMonthly] = useState(false);

  return (
    <div className="flex flex-col justify-between aspect-video rounded-xl bg-white p-5">
      <div>
        <div className={"flex justify-between items-center"}>
          <h3 className={"text-muted-foreground"}>Total Revenue</h3>


          <Euro className={"text-icon"} size={20} />
        </div>
        <div>
          <p className={"text-3xl font-bold"}>
              {monthly ? (
                  <>
                      <NumberTicker value={11356}/>€
                  </>
              ) : (
                  <>
                    <NumberTicker value={453}/>€
                  </>
              )}
          </p>

          {/*<span className={"text-xs font-medium text-danger"}>-9% last 30 days</span>*/}
        </div>
      </div>

      <div>
        <Tabs
          variant={"underlined"}
          defaultSelectedKey={"today"}
          aria-label="Options"
          classNames={{
            tabList: "gap-6 w-full relative rounded-none p-0 border-b border-divider",
            cursor: "w-full bg-success",
            tab: "max-w-fit px-0 h-12",
            tabContent: "group-data-[selected=true]:text-success"
          }}
          onSelectionChange={(e) => e === "monthly" ? setMonthly(true) : setMonthly(false)}
        >
          <Tab key="today" title="Today"/>
          <Tab key="monthly" title="Monthly"/>
        </Tabs>
      </div>
    </div>
  )
}
