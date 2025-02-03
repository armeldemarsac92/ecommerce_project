'use client'

import {InputOTP, InputOTPGroup, InputOTPSlot} from "@/components/shadcn/input-otp";
import {Button} from "@/components/shadcn/button";
import {Check} from "lucide-react";
import {verifyOTPCode} from "@/actions/auth";
import {useState} from "react";

export const OTPForm = () => {
    const [otpCode, setOtpCode] = useState()
    
    const handleVerify = async () => {
        /*const response = await verifyOTPCode()*/
    }

    return (
        <>
            <div className="grid gap-2">
                <InputOTP maxLength={6}>
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


            <Button onClick={} variant={"expandIcon"} iconPlacement={"right"} Icon={<Check size={15}/>} type="submit"
                    className="w-full">
                Verify
            </Button>
        </>
    )
}