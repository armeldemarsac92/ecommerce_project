import {authAxiosInstance} from "@/utils/instance/axios-instance";

async function signinWithOAuth(provider: string) {
    const { data } = await authAxiosInstance.get(`/auth/external-login/${provider}`);

    if(data) {
        window.location.href = data;
    }

    console.log(data)
}

export {
    signinWithOAuth
}