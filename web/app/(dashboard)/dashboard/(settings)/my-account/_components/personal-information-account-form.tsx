import { z } from "zod";
import AutoForm from "@/components/ui/auto-form";

export const PersonalInformationAccountForm = () => {
  const formSchema = z.object({
    first_name: z
      .string(),

    last_name: z
      .string()
  });

  return (
    <div className={"flex justify-between"}>
      <div className={"w-4/6"}>
        <h3 className={"text-sm font-semibold"}>Personal information</h3>
        <p className={"text-xs text-muted-foreground"}>
          Personal information for your account.
        </p>
      </div>

      <AutoForm
        formSchema={formSchema}
        fieldConfig={{
          first_name: {
            inputProps: {
              showLabel: false,
              defaultValue: "John",
              disabled: true,
            },
            renderParent: ({ children }) => (
              <div className="w-4/5">
                <div className="flex-1">
                  {children}
                </div>
              </div>
            ),
          },

          last_name: {
            inputProps: {
              showLabel: false,
              defaultValue: "Doe",
              disabled: true,
            },
            renderParent: ({ children }) => (
              <div className="w-4/5">
                <div className="flex-1">
                  {children}
                </div>
              </div>
            ),
          },
        }}
      />
    </div>
  )
}
