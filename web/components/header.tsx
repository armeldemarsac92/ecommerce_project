"use client"

import { SidebarTrigger } from "@/components/shadcn/sidebar";
import { Separator } from "@/components/shadcn/separator";
import {
  Breadcrumb,
  BreadcrumbItem,
  BreadcrumbList, BreadcrumbPage,
  BreadcrumbSeparator
} from "@/components/shadcn/breadcrumb";
import * as React from "react";
import { Button } from "@/components/shadcn/button";
import { Bell } from "lucide-react";
import { Popover, PopoverContent, PopoverTrigger } from "@nextui-org/react";
import NotificationModal from "@/components/ui/notification/notification-modal";
import { usePathname } from "next/navigation";
import { Link } from "@nextui-org/link";

export const Header = () => {
  const pathname = usePathname();
  const pathSegments = pathname.split("/").filter(segment => segment !== "");

  return (
    <header className="flex h-16 shrink-0 items-center gap-2 transition-[width,height] ease-linear group-has-[[data-collapsible=icon]]/sidebar-wrapper:h-12">
      <div className="w-full flex justify-between items-center px-4">
        <div className={"flex items-center gap-2"}>
          <SidebarTrigger className="-ml-1" />
          <Separator orientation="vertical" className="mr-2 h-4" />
          <Breadcrumb>
            <BreadcrumbList>
              {pathSegments.map((segment, index) => {
                const href = `/${pathSegments.slice(0, index + 1).join("/")}`;

                return (
                  <React.Fragment key={index}>
                    {index !== 0 && <BreadcrumbSeparator className="hidden md:block" />}
                    {index === pathSegments.length - 1 ? (
                      <BreadcrumbItem className="hidden md:block">
                        <BreadcrumbPage className={"text-xs"}>
                          {segment.charAt(0).toUpperCase() + segment.slice(1).replace(/-/g, " ")}
                        </BreadcrumbPage>
                      </BreadcrumbItem>
                    ) : (
                      <BreadcrumbItem className="hidden md:block">
                        <Link className={"text-xs text-muted-foreground duration-200 transition-colors hover:text-foreground"} href={href}>
                          {segment.charAt(0).toUpperCase() + segment.slice(1).replace(/-/g, " ")}
                        </Link>
                      </BreadcrumbItem>
                    )}
                  </React.Fragment>
                );
              })}
            </BreadcrumbList>
          </Breadcrumb>
        </div>


        <div className={"inline-flex gap-x-4"}>
          <Popover offset={12} placement="bottom-end">
            <PopoverTrigger>
              <div className={"relative"}>
                <Button variant="ghost" size="icon">
                  <Bell size={16} />
                </Button>

                <div
                  className={"flex justify-center items-center absolute top-0 right-0 bg-secondary rounded-full min-w-4 min-h-4"}>
                  <span className={"text-[8px] text-white font-medium"}>3</span>
                </div>
              </div>
            </PopoverTrigger>
            <PopoverContent className="min-w-[50vh] max-w-[90vw] p-0 sm:max-w-[380px]">
              <NotificationModal className="w-full shadow-none" />
            </PopoverContent>
          </Popover>
        </div>
      </div>
    </header>
  )
}
