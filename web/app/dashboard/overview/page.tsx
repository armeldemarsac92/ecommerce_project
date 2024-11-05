import { AppSidebar } from "@/components/ui/sidebar/app-sidebar"
import {
  Breadcrumb,
  BreadcrumbItem,
  BreadcrumbLink,
  BreadcrumbList,
  BreadcrumbPage,
  BreadcrumbSeparator,
} from "@/components/shadcn-ui/breadcrumb"
import { Separator } from "@/components/shadcn-ui/separator"
import {
  SidebarInset,
  SidebarProvider,
  SidebarTrigger,
} from "@/components/shadcn-ui/sidebar"
import { ThemeSwitch } from "@/components/theme-switch";
import * as React from "react";
import { TotalRevenueCard } from "@/app/dashboard/overview/_components/statistics/total-revenue-card";
import { CartAverageCard } from "@/app/dashboard/overview/_components/statistics/cart-average-card";
import { UsersNumberCard } from "@/app/dashboard/overview/_components/statistics/users-number-card";
import LastOrders from "@/app/dashboard/overview/_components/statistics/last-orders";
import { NumberSalesChart } from "@/app/dashboard/overview/_components/statistics/number-sales-chart";

export default function Page() {
  return (
      <div className="flex flex-1 flex-col gap-4 p-4 bg-[#F7F7F7]">
        <div className="grid auto-rows-min gap-4 md:grid-cols-3">
          <TotalRevenueCard/>
          <CartAverageCard/>
          <UsersNumberCard/>
        </div>

        <div className={"w-full h-4/5"}>
          <div className="flex w-full h-full rounded-xl gap-x-4">
            <div className={"w-4/6 h-full bg-white rounded-xl"}>
              <NumberSalesChart/>
            </div>
            <LastOrders/>
          </div>
        </div>
      </div>
  )
}
