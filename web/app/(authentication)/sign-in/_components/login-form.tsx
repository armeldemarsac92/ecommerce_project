"use client"

import { Button } from "@/components/shadcn/button"
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/shadcn/card"
import { Input } from "@/components/shadcn/input"
import { Label } from "@/components/shadcn/label"
import { ArrowRight } from "lucide-react";
import { useState } from "react";
import { Spinner } from "@nextui-org/spinner";
import { useRouter } from "next/navigation";
import {signinWithOAuth} from "@/actions/auth";
import {FcGoogle} from "react-icons/fc";

export const LoginForm = () => {
  const router = useRouter();
  const [buttonIsLoading, setButtonIsLoading] = useState(false);


  const handleSubmit = () => {
    setButtonIsLoading(true);
    router.push("/sign-in/verify");
  }

  const handleOAuthLogin = async (provider: string) => {
    const data = await signinWithOAuth(provider);

    console.log(data);
  }

  return (
    <Card className="mx-auto w-[40dvh]">
      <CardHeader className={"flex justify-center items-center space-y-0.5"}>
        <CardTitle className="text-2xl font-bold">MiamMiam</CardTitle>
        <CardDescription className={"text-xs"}>
          Admin - Dashboard
        </CardDescription>
      </CardHeader>
      <CardContent>
        <div className="grid gap-4 gap-y-8">
          <Button onClick={() => handleOAuthLogin('google')} variant={"outline"}>
            <FcGoogle className={"mr-2"} size={24}/>
            Connexion avec Google
          </Button>
          <div className="grid gap-2">
            <Label className={"text-xs"} htmlFor="email">Email</Label>
            <Input
              id="email"
              type="email"
              placeholder="m@example.com"
              required
            />
          </div>
          {/*<div className="grid gap-1">
            <div className="flex justify-between items-center">
              <Label className={"text-xs"} htmlFor="password">Password</Label>
              <Button className={"text-xs p-0 hover:text-secondary"} variant={"linkHover2"}>
                Forgot your password?
              </Button>
            </div>
            <Input id="password" type="password" placeholder={"*********"} required />
          </div>*/}
          <Button onClick={handleSubmit} variant={"expandIcon"} iconPlacement={"right"} Icon={<ArrowRight size={15} />} type="submit" className="w-full" disabled={buttonIsLoading}>
            {buttonIsLoading ? <Spinner color={"success"} size={"sm"}/> : "Login"}
          </Button>
        </div>
      </CardContent>
    </Card>
  )
}
