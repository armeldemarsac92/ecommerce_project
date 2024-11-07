"use client"

import { type LucideIcon } from "lucide-react"

import {
  SidebarGroup,
  SidebarGroupLabel,
  SidebarMenu,
  SidebarMenuButton,
  SidebarMenuItem,
} from "@/components/shadcn/sidebar"
import { usePathname, useRouter } from "next/navigation";

export function NavMain({
  titleNav,
  items,
}: {
  titleNav: string
  items: {
    title: string
    url: string
    icon?: LucideIcon
    items?: {
      title: string
      url: string
    }[]
  }[]
}) {

  const router = useRouter();
  const pathname = usePathname();

  return (
    <SidebarGroup>
      <SidebarGroupLabel>{titleNav}</SidebarGroupLabel>
      <SidebarMenu>
        {items.map((item) => (
          <SidebarMenuItem key={item.title}>
            <SidebarMenuButton onClick={() => router.push(item.url)} className={`transition-colors duration-200 hover:bg-secondary hover:text-white ${pathname !== item.url ? "text-muted-foreground" : "bg-secondary"}`} tooltip={item.title} isActive={pathname === item.url}>
              {item.icon && <item.icon className={`${pathname === item.url && "text-white"}`} />}
              <span className={`text-[13px] ${pathname === item.url && "text-white"}`}>{item.title}</span>
              {/*<Link className={`text-[13px] hover:text-for ${!item.isActive ? "text-muted-foreground/80" : "text-foreground"}`} href={item.url}>{item.title}</Link>*/}
            </SidebarMenuButton>
          </SidebarMenuItem>
        ))}
      </SidebarMenu>
    </SidebarGroup>
  )
}
