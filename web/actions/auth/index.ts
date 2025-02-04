import {authAxiosInstance} from "@/utils/instance/axios-instance";

type ISimpleLoginRequest = {
    email?: string,
    two_factor_code?: string
}

type IVerifyOTPRequest = {
    verification_code: string;
    email: string;
}

type IVerifyOTPResponse = {
    data: {
        accessToken: string,
        expiresIn: number,
        refreshToken: string
    }
    status: number
}

async function signinWithOAuth(provider: string) {
    const { data } = await authAxiosInstance.get(`/auth/external-login/${provider}`);

    if(data) {
        window.location.href = data;
    }

    console.log(data)
}

async function simpleLoginWithEmail(data: ISimpleLoginRequest) {
    return await authAxiosInstance.post(`/auth/simple-login`, data);
}

async function verifyOTPCode(data: IVerifyOTPRequest): Promise<IVerifyOTPResponse> {
    return await authAxiosInstance.post(`/auth/verify-2fa`, data);
}

export {
    signinWithOAuth,
    simpleLoginWithEmail,
    verifyOTPCode
}