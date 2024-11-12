import { Resend } from 'resend';
import {EmailTemplate} from "@/components/resend/email-template";

const resend = new Resend(process.env.RESEND_API_KEY);

export async function POST() {
    try {
        const { data, error } = await resend.emails.send({
            from: 'MiamMiam <noreply@resend.dev>',
            to: ['reverssecole@gmail.com'],
            subject: 'Hello world',
            react: EmailTemplate({ username: 'Jesko', validationCode: "123456" }),
        });

        if (error) {
            return Response.json({ error }, { status: 500 });
        }

        return Response.json(data);
    } catch (error) {
        return Response.json({ error }, { status: 500 });
    }
}