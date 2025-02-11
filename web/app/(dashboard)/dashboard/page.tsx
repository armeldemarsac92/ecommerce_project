import { TotalRevenueCard } from "@/app/(dashboard)/dashboard/_components/statistics/total-revenue-card";
import { OrderAverageCard } from "@/app/(dashboard)/dashboard/_components/statistics/order-average-card";
import { UsersNumberCard } from "@/app/(dashboard)/dashboard/_components/statistics/users-number-card";
import LastOrders from "@/app/(dashboard)/dashboard/_components/statistics/last-orders";
import { NumberSalesChart } from "@/app/(dashboard)/dashboard/_components/statistics/number-sales-chart";

export default function Page() {
  return (
    <div className="flex flex-1 flex-col gap-4 p-4 bg-[#F7F7F7]">
      <div className="grid auto-rows-min gap-4 md:grid-cols-3">
        <TotalRevenueCard/>
        <OrderAverageCard/>
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
