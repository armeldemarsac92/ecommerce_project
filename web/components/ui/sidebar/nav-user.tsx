"use client"

import {
  BadgeCheck, Bell,
  ChevronsUpDown,
  LogOut,
  Sparkles
} from "lucide-react";

import {
  Avatar,
  AvatarFallback,
  AvatarImage,
} from "@/components/shadcn/avatar"
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuGroup,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/components/shadcn/dropdown-menu"
import {
  SidebarMenu,
  SidebarMenuButton,
  SidebarMenuItem,
  useSidebar,
} from "@/components/shadcn/sidebar"
import * as React from "react";
import { useRouter } from "next/navigation";
import axios from "axios";
import {AuthenticatedUser} from "@/contexts/app-context";

export function NavUser({
  user,
}: {
  user: AuthenticatedUser
}) {
  const { isMobile } = useSidebar();

  const router = useRouter();

  const sendEmail = async () => {
    const res = await axios.post("/api/resend/send-email", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
    });

    if (res.status === 200) {
      alert("Email sent successfully");
    } else {
      alert("Error sending email");
    }
  };

  return (
    <SidebarMenu>
      <SidebarMenuItem>
        <DropdownMenu>
          <DropdownMenuTrigger asChild>
            <SidebarMenuButton size="lg">
              <div className={"relative inline-block"}>
                  <Avatar className="w-8 h-8 rounded-lg object-contain" asChild>
                    <AvatarImage src={"https://ui.shadcn.com/avatars/02.png"} alt={"Avatar user"} />
                    {/*<AvatarFallback className="rounded-full">CN</AvatarFallback>*/}
                  </Avatar>
              </div>
              <div className="grid flex-1 text-left text-sm leading-tight group-data-[collapsible=icon]:hidden">
                <span className="truncate font-semibold">{user.given_name}</span>
                <span className="truncate text-xs">{user.email}</span>
              </div>
              <ChevronsUpDown className="group-data-[collapsible=icon]:hidden ml-auto mt-0.5" />
            </SidebarMenuButton>
          </DropdownMenuTrigger>
          <DropdownMenuContent
            className="w-[--radix-dropdown-menu-trigger-width] min-w-56 rounded-lg"
            side={isMobile ? "bottom" : "right"}
            align="end"
            sideOffset={4}
          >
            <DropdownMenuLabel className="p-0 font-normal">
              <div className="flex items-center gap-2 px-1 py-1.5 text-left text-sm">
                <Avatar className="h-8 w-8 rounded-lg">
                  <AvatarImage src={"https://ui.shadcn.com/avatars/02.png"} alt={"Avatar user"} />
                  <AvatarFallback className="rounded-lg">CN</AvatarFallback>
                </Avatar>
                <div className="grid flex-1 text-left text-sm leading-tight">
                  <span className="truncate font-semibold">{user.given_name}</span>
                  <span className="truncate text-xs">{user.email}</span>
                </div>
              </div>
            </DropdownMenuLabel>
            <DropdownMenuSeparator />
            <DropdownMenuGroup>
              <DropdownMenuItem onClick={sendEmail} className={"hover:cursor-pointer"}>
                <Sparkles />
                Upgrade to Pro
              </DropdownMenuItem>
            </DropdownMenuGroup>
            <DropdownMenuSeparator />
            <DropdownMenuGroup>
              <DropdownMenuItem className={"hover:cursor-pointer"} onClick={() => {router.push("/dashboard/my-account")}}>
                <BadgeCheck />
                Account
              </DropdownMenuItem>
              <DropdownMenuItem className={"hover:cursor-pointer"}>
                <Bell />
                Notifications
              </DropdownMenuItem>
            </DropdownMenuGroup>
            <DropdownMenuSeparator />
            <DropdownMenuItem className={"hover:cursor-pointer"}>
              <LogOut />
              Log out
            </DropdownMenuItem>
          </DropdownMenuContent>
        </DropdownMenu>
      </SidebarMenuItem>
    </SidebarMenu>
  )
}
