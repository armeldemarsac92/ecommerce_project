import {Loader} from "@/components/loader";

export default function Loading() {
    return (
        <div className="w-screen h-screen flex flex-col justify-center items-center">
            <Loader/>
        </div>
    );
}
