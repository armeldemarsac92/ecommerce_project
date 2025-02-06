"use client"

import { ChartNoAxesCombined, CreditCard, Grid3x3, PackageSearch, PanelsTopLeft, UsersRound } from "lucide-react";
import { NavMain } from "@/components/ui/sidebar/nav-main";
import { NavUser } from "@/components/ui/sidebar/nav-user";
import {
  Sidebar,
  SidebarContent,
  SidebarFooter,
  SidebarHeader,
  SidebarRail
} from "@/components/shadcn/sidebar";
import { Logo } from "@/components/icons";
import { Link } from "@nextui-org/link";

// This is sample data.
const data = {
  user: {
    name: "Jesko",
    email: "developer@miammiam.com",
    avatar: "https://ui.shadcn.com/avatars/02.png",
  },

  dashboard_nav: [
    {
      title: "Overview",
      url: "/dashboard",
      icon: PanelsTopLeft,
    },
    {
      title: "Statistics",
      url: "/dashboard/statistics",
      icon: ChartNoAxesCombined,
    },
  ],

  catalog_nav: [
    {
      title: "Products",
      url: "/dashboard/catalog/products",
      icon: Grid3x3,
    },
    {
      title: "Inventory",
      url: "/dashboard/catalog/inventory",
      icon: PackageSearch,
    }
  ],

  customers_nav: [
    {
      title: "Users",
      url: "/dashboard/customers/users",
      icon: UsersRound,
    }
  ],

  sales_nav: [
    {
      title: "Orders",
      url: "/dashboard/sales/orders",
      icon: CreditCard,
    }
  ],
};

export function AppSidebar({ ...props }: React.ComponentProps<typeof Sidebar>) {
  return (
    <Sidebar collapsible="icon" {...props}>
      <SidebarHeader className={"flex-row items-center gap-x-3"} data-cy="navbar-header">
        <Link href={"/dashboard"}>
          <Logo size={30}/>
        </Link>
        <div className={"group-data-[collapsible=icon]:hidden"}>
          <h1 className={"font-semibold"}>MiamMiam</h1>
          <p className={"text-xs text-gray-500 font-light"}>Admin Dashboard</p>
        </div>
      </SidebarHeader>

      <SidebarContent data-cy="navbar-content">
        <NavMain titleNav={"Dashboard"} items={data.dashboard_nav} />
        <NavMain titleNav={"Catalog"} items={data.catalog_nav} />
        <NavMain titleNav={"Customers"} items={data.customers_nav} />
        <NavMain titleNav={"Sales"} items={data.sales_nav} />
      </SidebarContent>

      <SidebarFooter data-cy="navbar-footer">
        <NavUser user={data.user} />
      </SidebarFooter>
      <SidebarRail />
    </Sidebar>
  );
}
