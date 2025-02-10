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
import {signinWithOAuth, simpleLoginWithEmail} from "@/actions/auth";
import {FcGoogle} from "react-icons/fc";
import {FaAws} from "react-icons/fa";
import {useAuthContext} from "@/contexts/auth-context";
import {useToast} from "@/hooks/use-toast";

export const LoginForm = () => {
  const router = useRouter();

  const {toast} = useToast();
  const {updateAuthData, updateContextLoading, contextLoading} = useAuthContext();


  const [email, setEmail] = useState("");

  const handleOAuthLogin = async (provider: string) => {
    await signinWithOAuth(provider);
  }

  const handleSimpleLoginWithEmail = async (email: string) => {
    updateContextLoading(true)
    updateAuthData({
      current_email: email
    })

    const response = await simpleLoginWithEmail({email});

    updateContextLoading(false)

    if(response.status === 200) {
      router.push("/sign-in/verify")
    }else {
      toast({ variant: "destructive", title: "An error was occurred", description: "Bad OTP code" })
    }
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

          <Button onClick={() => handleOAuthLogin('aws')} variant={"outline"}>
            <FaAws className={"mr-2"} size={24}/>
            Connexion avec AWS
          </Button>
          
          <div className="grid gap-2">
            <Label className={"text-xs"} htmlFor="email">Email</Label>
            <Input
              id="email"
              type="email"
              placeholder="m@example.com"
              required
              value={email}
              onChange={(e) => setEmail(e.target.value)}
            />
          </div>

          <Button
              data-cy="login-button"
              onClick={() => {handleSimpleLoginWithEmail(email)}} variant={"expandIcon"} iconPlacement={"right"} Icon={<ArrowRight size={15} />} type="submit" className="w-full" disabled={contextLoading}
          >
            {contextLoading ? <Spinner color={"success"} size={"sm"}/> : "Login"}
          </Button>
        </div>
      </CardContent>
    </Card>
  )
}
