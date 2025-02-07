import { z } from "zod";
import AutoForm from "@/components/ui/auto-form";

export const ContactInformationAccountForm = () => {
  const formSchema = z.object({
    email: z
      .string(),

    phone: z
      .string()
  });

  return (
    <div className={"flex justify-between"}>
      <div className={"w-4/6"}>
        <h3 className={"text-sm font-semibold"}>Contact information</h3>
        <p className={"text-xs text-muted-foreground"}>
          Contact information for your account.
        </p>
      </div>

      <AutoForm
        formSchema={formSchema}
        fieldConfig={{
          email: {
            inputProps: {
              showLabel: false,
              defaultValue: "developer@miammiam.com",
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

          phone: {
            inputProps: {
              showLabel: false,
              defaultValue: "0781081234",
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
