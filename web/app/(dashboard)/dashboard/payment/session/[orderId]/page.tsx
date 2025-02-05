"use client";

import { loadStripe } from '@stripe/stripe-js';
import { useEffect, useState } from 'react';
import { useRouter } from 'next/navigation';

const stripePromise = loadStripe('pk_test_51Qj3NQLx56XjlpN6w9erBverpJAb1Gl3MEaNlKNwUQJCG1kS1IkTE0b1h1jsnbMVBRAEncZiPrWfTiMV1od8zkZ2008LSooULv');

export default function CheckoutPage({ params }: { params: { orderId: string } }) {
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');

    useEffect(() => {
        const initCheckout = async () => {
            try {
                const response = await fetch(`https://localhost:7143/api/v1/orders/${params.orderId}/session`, {
                    method: 'POST',
                });

                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }

                const data = await response.json();
                console.log('Session data:', data); // Debug log

                const stripe = await stripePromise;
                if (!stripe) throw new Error("Stripe not initialized");

                await stripe.redirectToCheckout({
                    sessionId: data.url.split('pay/')[1].split('#')[0]
                });
            } catch (err : any) {
                console.error('Error:', err);
                setError(err.message || 'Checkout failed');
            } finally {
                setLoading(false);
            }
        };

        initCheckout();
    }, [params.orderId]);

    if (loading) return <div>Loading...</div>;
    if (error) return <div>{error}</div>;
}