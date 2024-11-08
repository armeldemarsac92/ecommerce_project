"use client"

import * as React from "react";
import { ChartNoAxesCombined, CreditCard, Grid3x3, Newspaper, PackageSearch, PanelsTopLeft, UsersRound } from "lucide-react";
import { NavMain } from "@/components/ui/sidebar/nav-main";
import { NavUser } from "@/components/ui/sidebar/nav-user";
import { Sidebar, SidebarContent, SidebarFooter, SidebarHeader, SidebarRail } from "@/components/shadcn-ui/sidebar";
import { ThemeSwitch } from "@/components/theme-switch";

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
      url: "/dashboard/overview",
      icon: PanelsTopLeft,
      isActive: true,
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
      isActive: true,
    },
    {
      title: "Inventory",
      url: "/dashboard/catalog/Inventory",
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
      isActive: true,
    },
    {
      title: "Invoices",
      url: "/dashboard/sales/invoices",
      icon: Newspaper,
    }
  ],
};

export function AppSidebar({ ...props }: React.ComponentProps<typeof Sidebar>) {
  return (
    <Sidebar collapsible="icon" {...props}>
      <SidebarHeader/>

      <SidebarContent>
        <NavMain titleNav={"Dashboard"} items={data.dashboard_nav} />
        <NavMain titleNav={"Catalog"} items={data.catalog_nav} />
        <NavMain titleNav={"Customers"} items={data.customers_nav} />
        <NavMain titleNav={"Sales"} items={data.sales_nav} />
      </SidebarContent>

      <SidebarFooter>
        <NavUser user={data.user} />
      </SidebarFooter>
      <SidebarRail />
    </Sidebar>
  );
}
