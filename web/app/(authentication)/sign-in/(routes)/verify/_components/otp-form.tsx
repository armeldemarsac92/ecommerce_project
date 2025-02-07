'use client'

import {InputOTP, InputOTPGroup, InputOTPSlot} from "@/components/shadcn/input-otp";
import {Button} from "@/components/shadcn/button";
import {Check} from "lucide-react";
import {verifyOTPCode} from "@/actions/auth";
import {useEffect, useState} from "react";
import {useAuthContext} from "@/contexts/auth-context";
import {Spinner} from "@nextui-org/spinner";
import {useRouter} from "next/navigation";
import {useToast} from "@/hooks/use-toast";
import {useAppContext} from "@/contexts/app-context";
import {format} from "date-fns";
import {fr} from "date-fns/locale";

export const OTPForm = () => {
    const router = useRouter();
    const {toast} = useToast();

    const {isLoading} = useAppContext();
    const {authData, updateContextLoading, contextLoading} = useAuthContext();
    const {storeTokens} = useAppContext();

    const [otpCode, setOtpCode] = useState("")

    const handleSubmit = async () => {
        updateContextLoading(true)

        if(authData) {
            if(authData.current_email) {
                const response = await verifyOTPCode({
                    email: authData.current_email,
                    verification_code: otpCode
                })

                updateContextLoading(false)

                if(response.status === 200) {
                    storeTokens(response.data)
                }else {
                    toast({ variant: "destructive", title: "An error was occurred", description: "Bad credentials" })
                }
            }
        }
    }

    return (
        <section className={"flex flex-col gap-y-6"}>
            <div className="relative w-[40dvh] flex flex-col justify-center items-center gap-y-4">
                <InputOTP
                    value={otpCode}
                    onChange={(value) => setOtpCode(value)}
                    maxLength={6}
                >
                    <InputOTPGroup>
                        <InputOTPSlot index={0}/>
                        <InputOTPSlot index={1}/>
                        <InputOTPSlot index={2}/>
                        <InputOTPSlot index={3}/>
                        <InputOTPSlot index={4}/>
                        <InputOTPSlot index={5}/>
                    </InputOTPGroup>
                </InputOTP>

                <Button onClick={handleSubmit} disabled={otpCode.length < 6 || contextLoading} variant={"expandIcon"}
                        iconPlacement={"right"} Icon={<Check size={15}/>} type="submit" className="w-11/12">
                    {contextLoading ? <Spinner color={"success"} size={"sm"}/> : "Verify"}
                </Button>
            </div>


            <div className={"flex flex-col justify-center items-center"}>
                <p className={"text-center text-sm text-muted-foreground"}>An email has been sent at <span className={"text-black font-medium"}>{authData?.current_email}</span></p>
                <Button className={"text-secondary"} variant={"link"} value={"Click here to resend"}>Click here to resend</Button>
            </div>
        </section>
    )
}