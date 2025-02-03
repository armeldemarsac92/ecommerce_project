import {authAxiosInstance} from "@/utils/instance/axios-instance";

type ISimpleLoginRequest = {
    email: string,
    two_factor_code: string
}

type ISimpleLoginResponse = {
    requiresTwoFactor: boolean,
    provider: string
}

type IVerifyOTPRequest = {
    verification_code: string;
    email: string;
}

type IVerifyOTPResponse = {
    accessToken: string,
    expiresIn: number,
    refreshToken: string
}

/*async function signinWithOAuth(provider: string) {
    const { data } = await authAxiosInstance.get(`/auth/external-login/${provider}`);

    if(data) {
        window.location.href = data;
    }

    console.log(data)
}*/

async function simpleLoginWithEmail(data: ISimpleLoginRequest): Promise<ISimpleLoginResponse> {
    return await authAxiosInstance.post(`/auth/simple-login`, data);
}

async function verifyOTPCode(data: IVerifyOTPRequest): Promise<IVerifyOTPResponse> {
    return await authAxiosInstance.post(`/auth/simple-login`, data);
}

export {
    simpleLoginWithEmail,
    verifyOTPCode
}