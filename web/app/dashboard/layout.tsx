import "@/styles/globals.css";
import {SidebarProvider} from "@/components/shadcn-ui/sidebar";
import {AppSidebar} from "@/components/ui/sidebar/app-sidebar";

export default function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
  <SidebarProvider>
    <AppSidebar />
    {children}
  </SidebarProvider>
  );
}
