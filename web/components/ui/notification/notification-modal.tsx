"use client";

import type {CardProps} from "@nextui-org/react";

import React from "react";
import {
  Card,
  CardBody,
  CardHeader,
  Chip,
  Tabs,
  Tab,
  ScrollShadow,
  CardFooter,
} from "@nextui-org/react";
import {Icon} from "@iconify/react";
import { NotificationItem } from "@/components/ui/notification/notification-item";
import { Button } from "@/components/shadcn/button";

type Notification = {
  id: string;
  isRead?: boolean;
  avatar: string;
  description: string;
  name: string;
  time: string;
  type?: "default" | "request" | "file";
};

enum NotificationTabs {
  All = "all",
}

const notifications: Record<NotificationTabs, Notification[]> = {
  all: [
    {
      id: "6",
      isRead: true,
      avatar: "https://i.pravatar.cc/150?u=a04458a24a2d826712d",
      description: "commented on your post.",
      name: "Amelie Dawson",
      time: "4 days ago",
    },
  ],
};

export default function NotificationModal(props: CardProps) {
  const [activeTab, setActiveTab] = React.useState<NotificationTabs>(NotificationTabs.All);

  const activeNotifications = notifications[activeTab];

  return (
    <Card className="w-full min-w-[50vh] max-w-[50vh]" {...props}>
      <CardHeader className="flex flex-col px-0 pb-0">
        <div className="flex w-full items-center justify-between px-5 py-2">
          <div className="inline-flex items-center gap-1">
            <h4 className="inline-block align-middle text-large font-medium">Notifications</h4>
            <Chip size="sm" variant="flat">
              12
            </Chip>
          </div>
          <Button className={"text-sm font-normal"} variant={"ghost"}>
            Delete all
          </Button>
        </div>
        <Tabs
          aria-label="Notifications"
          classNames={{
            base: "w-full",
            tabList: "gap-6 px-6 py-0 w-full relative rounded-none border-b border-divider",
            cursor: "w-full",
            tab: "max-w-fit px-2 h-12",
          }}
          color="primary"
          selectedKey={activeTab}
          variant="underlined"
          onSelectionChange={(selected) => setActiveTab(selected as NotificationTabs)}
        >
          <Tab
            key="all"
            title={
              <div className="flex items-center space-x-2">
                <span>All</span>
              </div>
            }
          />
        </Tabs>
      </CardHeader>
      <CardBody className="w-full gap-0 p-0">
        <ScrollShadow className="h-[500px] w-full">
          {activeNotifications?.length > 0 ? (
            activeNotifications.map((notification) => (
              <NotificationItem key={notification.id} {...notification} />
            ))
          ) : (
            <div className="flex h-full w-full flex-col items-center justify-center gap-2">
              <Icon className="text-default-400" icon="solar:bell-off-linear" width={40} />
              <p className="text-small text-default-400">No notifications yet.</p>
            </div>
          )}
        </ScrollShadow>
      </CardBody>
    </Card>
  );
}
