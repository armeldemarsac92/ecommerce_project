"use client"

import {SearchIcon} from "lucide-react";
import React, {FC} from "react";
import {Input} from "@nextui-org/input";

interface SearchBarProps {
    value?: string | (readonly string[] & string) | undefined
    onValueChange?: (value: string) => void | undefined,
    onClear?: () => void | undefined
}

export const SearchBar: FC<SearchBarProps> = ({
    value,
    onValueChange,
    onClear
}) => {
    return (
        <Input
            isClearable
            className="sm:max-w-[44%]"
            classNames={{
                inputWrapper: "rounded-md border bg-transparent"
            }}
            placeholder="Search by name..."
            startContent={<SearchIcon className={"mr-1"} size={18} />}
            value={value}
            onClear={() => onClear}
            onValueChange={onValueChange}
        />
    )
}