import { SidebarInset, SidebarProvider } from "@/components/shadcn-ui/sidebar";
import { AppSidebar } from "@/components/ui/sidebar/app-sidebar";
import { Header } from "@/components/header";

export default function DashboardLayout({ children }: { children: React.ReactNode }) {
  return (
    <SidebarProvider>
      <AppSidebar />

      <SidebarInset>
        <Header/>

        {children}
      </SidebarInset>
    </SidebarProvider>
  )
}
