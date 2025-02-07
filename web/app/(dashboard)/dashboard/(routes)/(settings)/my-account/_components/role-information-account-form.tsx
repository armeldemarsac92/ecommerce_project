import { Input } from "@/components/shadcn/input";

export const RoleInformationAccountForm = () => {
  return (
    <div className={"flex justify-between"}>
      <div className={"w-4/6"}>
        <h3 className={"text-sm font-semibold"}>Role information</h3>
        <p className={"text-xs text-muted-foreground"}>
          Role information for your account.
        </p>
      </div>

      <Input value={"Admin"} disabled/>
    </div>
  )
}
