import { useState } from "react"
import { Check, ChevronsUpDown } from "lucide-react"
import { cn } from "@/lib/utils"
import { Button } from "@/components/shadcn/button"
import { Command, CommandEmpty, CommandGroup, CommandInput, CommandItem, CommandList } from "@/components/shadcn/command"
import { Popover, PopoverContent, PopoverTrigger } from "@/components/shadcn/popover"

interface Tag {
    id: number;
    title: string;
}

interface TagComboboxProps {
    tags: Tag[];
    value: number[];
    onChange: (value: number[]) => void;
}

export function TagCombobox({ tags, value, onChange }: TagComboboxProps) {
    const [open, setOpen] = useState(false)

    const getSelectedLabels = () => {
        return value
            .map((id) => tags.find((t) => t.id === id)?.title || '')
            .filter(title => title !== '')
            .join(", ")
    }

    return (
        <Popover open={open} onOpenChange={setOpen}>
            <PopoverTrigger asChild>
                <Button
                    variant="outline"
                    role="combobox"
                    aria-expanded={open}
                    className="w-full justify-between"
                >
          <span className="truncate">
            {value.length > 0 ? getSelectedLabels() : "Sélectionner des tags"}
          </span>
                    <ChevronsUpDown className="ml-2 h-4 w-4 shrink-0 opacity-50" />
                </Button>
            </PopoverTrigger>
            <PopoverContent className="w-full p-0">
                <Command>
                    <CommandInput placeholder="Rechercher un tag..." />
                    <CommandList>
                        <CommandEmpty>Aucun tag trouvé.</CommandEmpty>
                        <CommandGroup>
                            {tags.map((tag) => (
                                <CommandItem
                                    key={tag.id}
                                    onSelect={() => {
                                        onChange(
                                            value.includes(tag.id)
                                                ? value.filter((id) => id !== tag.id)
                                                : [...value, tag.id]
                                        )
                                    }}
                                >
                                    <Check
                                        className={cn(
                                            "mr-2 h-4 w-4",
                                            value.includes(tag.id) ? "opacity-100" : "opacity-0"
                                        )}
                                    />
                                    {tag.title}
                                </CommandItem>
                            ))}
                        </CommandGroup>
                    </CommandList>
                </Command>
            </PopoverContent>
        </Popover>
    )
}