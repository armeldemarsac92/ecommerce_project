"use client";

import React, { useEffect, useState } from 'react';
import { loadStripe } from '@stripe/stripe-js';
import {
    Elements,
    CardElement,
    useStripe,
    useElements
} from '@stripe/react-stripe-js';

// Define types for our component props and API responses
interface PaymentFormProps {
    orderId: number;
    amount: number;
    clientSecret: string;
}

interface PaymentConfirmationResponse {
    success: boolean;
}

interface PaymentIntentResponse {
    clientSecret: string;
    amount: number;
}

// Initialize Stripe with your publishable key
const stripePromise = loadStripe('pk_test_51Qj3NQLx56XjlpN6w9erBverpJAb1Gl3MEaNlKNwUQJCG1kS1IkTE0b1h1jsnbMVBRAEncZiPrWfTiMV1od8zkZ2008LSooULv');

const PaymentForm: React.FC<PaymentFormProps> = ({ orderId, amount, clientSecret }) => {
    const stripe = useStripe();
    const elements = useElements();
    const [error, setError] = useState<string | null>(null);
    const [loading, setLoading] = useState<boolean>(false);
    const [succeeded, setSucceeded] = useState<boolean>(false);

    const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();

        if (!stripe || !elements) {
            return;
        }

        setLoading(true);
        setError(null);

        try {
            const { error: paymentError, paymentIntent } = await stripe.confirmCardPayment(clientSecret, {
                payment_method: {
                    card: elements.getElement(CardElement)!,
                    billing_details: {
                        name: 'Test User', // You might want to make this dynamic
                    },
                },
            });

            if (paymentError) {
                throw paymentError;
            }

            // Payment successful, confirm with backend
            const response = await fetch('http://localhost:5001/api/orders/payment/confirm', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    orderId,
                    paymentIntentId: paymentIntent!.id,
                }),
            });

            if (!response.ok) {
                throw new Error('Failed to confirm payment with server');
            }

            const confirmationData: PaymentConfirmationResponse = await response.json();
            console.log('Payment confirmation:', confirmationData);
            setSucceeded(true);
        } catch (err) {
            setError((err as Error).message || 'An error occurred during payment');
        } finally {
            setLoading(false);
        }
    };

    return (
        <form onSubmit={handleSubmit} className="w-full max-w-md">
            <div className="mb-6 p-3 border rounded">
                <CardElement
                    options={{
                        style: {
                            base: {
                                fontSize: '16px',
                                color: '#424770',
                                '::placeholder': {
                                    color: '#aab7c4',
                                },
                            },
                            invalid: {
                                color: '#9e2146',
                            },
                        },
                    }}
                />
            </div>

            {error && (
                <div className="mb-4 p-3 text-sm text-red-600 bg-red-50 rounded">
                    {error}
                </div>
            )}

            {succeeded ? (
                <div className="p-3 text-sm text-green-600 bg-green-50 rounded">
                    Payment successful! Your order has been confirmed.
                </div>
            ) : (
                <button
                    type="submit"
                    disabled={!stripe || loading}
                    className="w-full py-2 px-4 bg-blue-600 text-white rounded hover:bg-blue-700 disabled:bg-gray-400 disabled:cursor-not-allowed"
                >
                    {loading ? 'Processing...' : `Pay $${amount}`}
                </button>
            )}
        </form>
    );
};

const CheckoutWrapper: React.FC = () => {
    const [clientSecret, setClientSecret] = useState<string>('');
    const [amount, setAmount] = useState<number>(0);
    const orderId = 6; // For testing purposes

    useEffect(() => {
        const initPayment = async () => {
            try {
                const response = await fetch(`http://localhost:5001/api/orders/${orderId}/payment/intent`, {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                });

                if (!response.ok) {
                    throw new Error('Failed to initialize payment');
                }

                const data: PaymentIntentResponse = await response.json();
                setClientSecret(data.clientSecret);
                setAmount(data.amount);
            } catch (err) {
                console.log('Payment initialization failed:', err);
            }
        };

        initPayment();
    }, [orderId]);

    if (!clientSecret) {
        return <div className="text-center p-4">Loading payment form...</div>;
    }

    return (
        <div className="min-h-screen bg-gray-50 flex items-center justify-center p-4">
            <div className="w-full max-w-md bg-white p-6 rounded-lg shadow-md">
                <h2 className="text-2xl font-bold mb-6">Complete Your Payment</h2>
                <div className="mb-4">
                    <p className="text-gray-600">Order #{orderId}</p>
                    <p className="text-lg font-semibold">${amount}</p>
                </div>

                <Elements
                    stripe={stripePromise}
                    options={{
                        clientSecret,
                        appearance: {
                            theme: 'stripe',
                            variables: {
                                colorPrimary: '#2563eb',
                                colorBackground: '#ffffff',
                                colorText: '#1f2937',
                            },
                        },
                    }}
                >
                    <PaymentForm orderId={orderId} amount={amount} clientSecret={clientSecret} />
                </Elements>
            </div>
        </div>
    );
};

export default CheckoutWrapper;