import { FormLabel } from "@/components/shadcn/form";
import { cn } from "@/lib/utils";

function AutoFormLabel({
  label,
  isRequired,
  className,
}: {
  label: string;
  isRequired: boolean;
  className?: string;
}) {
  return (
    <>
      <FormLabel className={cn(className) + "text-xs"}>
        {label}
        {isRequired && <span className="text-red-500"> *</span>}
      </FormLabel>
    </>
  );
}

export default AutoFormLabel;
