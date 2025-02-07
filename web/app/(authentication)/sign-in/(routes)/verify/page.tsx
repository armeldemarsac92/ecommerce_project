import {Card, CardContent, CardDescription, CardHeader, CardTitle} from "@/components/shadcn/card";
import {OTPForm} from "@/app/(authentication)/sign-in/(routes)/verify/_components/otp-form";

export default function VerifySignInPage() {
  return (
      <div className="flex h-screen w-full items-center justify-center px-4">
          <Card className="mx-auto w-fit shadow-none border-none">
              <CardHeader className={"flex justify-center items-center space-y-0.5"}>
                  <CardTitle className="text-2xl font-bold">Verify your account</CardTitle>
                  <CardDescription className={"text-xs"}>
                      Enter your OTP Code sended in your mail
                  </CardDescription>
              </CardHeader>
              <CardContent className={"flex justify-center items-center mt-2"}>
                  <div className="grid gap-4 gap-y-5">
                      <OTPForm/>
                  </div>
              </CardContent>
          </Card>
      </div>
  )
}
