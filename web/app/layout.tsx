import "@/styles/globals.css";
import { Metadata, Viewport } from "next";
import clsx from "clsx";

import { Providers } from "./providers";

import { siteConfig } from "@/config/site";
import { supreme } from "@/config/fonts";
import * as React from "react";
import {Toaster} from "@/components/shadcn/toaster";
import {Suspense} from "react";
import Loading from "@/app/loading";
import {AppProvider} from "@/providers/app-provider";

export const metadata: Metadata = {
  title: {
    default: siteConfig.name,
    template: `%s - ${siteConfig.name}`,
  },
  description: siteConfig.description,
  icons: {
    icon: "/favicon.ico",
  },
};

export const viewport: Viewport = {
  themeColor: [
    { media: "(prefers-color-scheme: light)", color: "white" },
    { media: "(prefers-color-scheme: dark)", color: "black" },
  ],
};

export default function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <html suppressHydrationWarning lang="en">
      <head />
      <body
        className={clsx(
          "min-h-screen bg-background font-sans antialiased",
          supreme.className,
        )}
      >
        <Providers themeProps={{ attribute: "class", defaultTheme: "dark" }}>
          <AppProvider>
            <Suspense fallback={<Loading/>}>
              {children}
            </Suspense>

            <Toaster />
          </AppProvider>
        </Providers>
      </body>
    </html>
  );
}
