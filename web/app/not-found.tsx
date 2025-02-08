'use client';

import Link from 'next/link';
import { useRouter } from 'next/navigation';
import { HomeIcon } from 'lucide-react';
import {Button} from "@/components/shadcn/button";

export default function NotFound() {
    const router = useRouter();

    return (
        <div className="min-h-screen bg-background flex items-center justify-center p-5">
            <div className="max-w-md w-full space-y-8">
                <div className="text-center">
                    <h1 className="text-4xl font-bold text-primary mb-2">404</h1>
                    <h2 className="text-2xl font-semibold text-foreground mb-4">Page not found</h2>
                    <p className="text-muted-foreground mb-8">
                        Sorry, we couldn&apos;t find the page you&apos;re looking for.
                    </p>
                </div>

                <div className="flex flex-col sm:flex-row gap-4 justify-center">
                    <Button
                        variant="outline"
                        onClick={() => router.back()}
                        className="flex items-center gap-2"
                    >
                        Go Back
                    </Button>

                    <Button
                        asChild
                        variant={"secondary"}
                        className="flex items-center gap-2"
                    >
                        <Link href="/dashboard">
                            <HomeIcon className="w-4 h-4" />
                            Return to Home
                        </Link>
                    </Button>
                </div>
            </div>
        </div>
    );
}