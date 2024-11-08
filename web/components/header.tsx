import { SidebarTrigger } from "@/components/shadcn-ui/sidebar";
import { Separator } from "@/components/shadcn-ui/separator";
import {
  Breadcrumb,
  BreadcrumbItem,
  BreadcrumbLink,
  BreadcrumbList, BreadcrumbPage,
  BreadcrumbSeparator
} from "@/components/shadcn-ui/breadcrumb";
import { ThemeSwitch } from "@/components/theme-switch";
import * as React from "react";
import { Button } from "@/components/shadcn-ui/button";
import { BellIcon } from "@radix-ui/react-icons";
import { Bell } from "lucide-react";

export const Header = () => {
  return (
    <header
      className="flex h-16 shrink-0 items-center gap-2 transition-[width,height] ease-linear group-has-[[data-collapsible=icon]]/sidebar-wrapper:h-12">
      <div className="w-full flex justify-between items-center px-4">
        <div className={"flex items-center gap-2"}>
          <SidebarTrigger className="-ml-1" />
          <Separator orientation="vertical" className="mr-2 h-4" />
          <Breadcrumb>
            <BreadcrumbList>
              <BreadcrumbItem className="hidden md:block">
                <BreadcrumbLink href="#">
                  Building Your Application
                </BreadcrumbLink>
              </BreadcrumbItem>
              <BreadcrumbSeparator className="hidden md:block" />
              <BreadcrumbItem>
                <BreadcrumbPage>Data Fetching</BreadcrumbPage>
              </BreadcrumbItem>
            </BreadcrumbList>
          </Breadcrumb>
        </div>


        <div className={"inline-flex gap-x-2"}>
          <div className={"relative"}>
            <Button variant="ghost" size="icon" className="h-8 w-8">
              <Bell />
            </Button>

            <div className={"flex justify-center items-center absolute -top-1 -right-1 bg-danger rounded-full min-w-4 min-h-4"}>
              <span className={"text-[8px] text-white font-medium"}>3</span>
            </div>
          </div>

          <ThemeSwitch />
        </div>
      </div>
    </header>
  )
}
