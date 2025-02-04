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

    useEffect(() => {
        console.log(authData?.current_email)
    }, [authData]);

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
                    toast({ variant: "success", title: "Logged successfully", description: format(new Date(), 'dd/MM/yyyy:HH:mm', { locale: fr })})
                    router.push("/dashboard")
                }else {
                    toast({ variant: "destructive", title: "An error was occurred", description: "Bad credentials" })
                }
            }
        }
    }

    return (
        <>
            <div className="grid gap-2">
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
            </div>


            <Button onClick={handleSubmit}  disabled={otpCode.length < 6 || contextLoading} variant={"expandIcon"} iconPlacement={"right"} Icon={<Check size={15}/>} type="submit" className="w-full">
                {contextLoading ? <Spinner color={"success"} size={"sm"}/> : "Verify"}
            </Button>
        </>
    )
}