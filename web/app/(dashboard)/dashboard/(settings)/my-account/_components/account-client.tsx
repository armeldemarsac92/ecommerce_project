"use client"

import { Avatar, AvatarImage } from "@/components/shadcn/avatar";
import * as React from "react";
import { useIsSSR } from "@react-aria/ssr";
import { Spinner } from "@nextui-org/react";
import { Button } from "@/components/shadcn/button";
import { Pencil } from "lucide-react";
import { ContactInformationAccountForm } from "@/app/(dashboard)/dashboard/(settings)/my-account/_components/contact-information-account-form";
import { Separator } from "@/components/shadcn/separator";
import {
  PersonalInformationAccountForm
} from "@/app/(dashboard)/dashboard/(settings)/my-account/_components/personal-information-account-form";
import {
  RoleInformationAccountForm
} from "@/app/(dashboard)/dashboard/(settings)/my-account/_components/role-information-account-form";

export const AccountClient = () => {
  const isNotReady = useIsSSR();

  const user = {
    name: "Jesko",
    email: "developer@miammiam.com",
    avatar: "https://ui.shadcn.com/avatars/02.png",
    created_at: "05-11-2024",
  }

  return (
    <div className="flex flex-1 flex-col gap-4 py-8 px-14 bg-[#F7F7F7]">
      {!isNotReady ? (
        <div className={"flex flex-col gap-y-10"}>
          <div className={"w-full flex justify-between items-center"}>
            <div className={"flex items-center gap-x-5"}>
              <Avatar className="size-20" asChild>
                <AvatarImage src={user.avatar} alt={user.name} />
              </Avatar>

              <div>
                <h1 className={"text-xl font-bold"}>{user.name}</h1>
                <p className={"text-muted-foreground text-sm"}>{user.email}</p>
                <span className={"text-muted-foreground text-xs font-light"}>Created at <span className={"font-medium underline"}>{user.created_at}</span></span>
              </div>
            </div>

            <Button className={"text-xs"} size={"sm"} variant="expandIcon" Icon={Pencil} iconPlacement="right">
              {/*<Edit size={15} />*/}
              Modify
            </Button>
          </div>

          <Separator orientation={"horizontal"}/>

          <PersonalInformationAccountForm/>

          <Separator orientation={"horizontal"}/>

          <ContactInformationAccountForm/>

          <Separator orientation={"horizontal"}/>

          <RoleInformationAccountForm/>
        </div>
      ) : (
        <div className={"w-full h-full flex justify-center items-center"}>
          <Spinner />
        </div>
      )}
    </div>
  )
}
