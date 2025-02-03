import {Card, CardContent, CardDescription, CardHeader, CardTitle} from "@/components/shadcn/card";
import {Button} from "@/components/shadcn/button";
import {Check} from "lucide-react";
import {InputOTP, InputOTPGroup, InputOTPSlot} from "@/components/shadcn/input-otp";

export default function VerifySignInPage() {
  return (
      <div className="flex h-screen w-full items-center justify-center px-4">
          <Card className="mx-auto w-[40dvh] shadow-none border-none">
              <CardHeader className={"flex justify-center items-center space-y-0.5"}>
                  <CardTitle className="text-2xl font-bold">Verify your account</CardTitle>
                  <CardDescription className={"text-xs"}>
                      Enter your SSO Code send in your mail
                  </CardDescription>
              </CardHeader>
              <CardContent className={"flex justify-center mt-5"}>
                  <div className="grid gap-4 gap-y-5">
                      <div className="grid gap-2">
                          <InputOTP maxLength={6}>
                              <InputOTPGroup>
                                  <InputOTPSlot index={0} />
                                  <InputOTPSlot index={1} />
                                  <InputOTPSlot index={2} />
                                  <InputOTPSlot index={3} />
                                  <InputOTPSlot index={4} />
                                  <InputOTPSlot index={5} />
                              </InputOTPGroup>
                          </InputOTP>
                      </div>


                      <Button  variant={"expandIcon"} iconPlacement={"right"} Icon={Check} type="submit" className="w-full">
                          Verify
                      </Button>
                  </div>
              </CardContent>
          </Card>
      </div>
  )
}
