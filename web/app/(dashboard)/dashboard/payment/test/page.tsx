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
        if (!stripe || !elements) return;

        setLoading(true);
        setError(null);

        try {
            const { error: paymentError, paymentIntent } = await stripe.confirmCardPayment(clientSecret, {
                payment_method: {
                    card: elements.getElement(CardElement)!,
                    billing_details: {
                        name: 'Test User',
                    },
                },
            });

            if (paymentError) {
                setError(paymentError.message || 'An error occurred');
                setSucceeded(false);
            } else if (paymentIntent.status === 'succeeded') {
                setSucceeded(true);
            }
        } catch (err) {
            setError('Payment failed');
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
    const [orderId, setOrderId] = useState<number>(6);
    const [serverError, setServerError] = useState<string>('');

    const handleOrderSubmit = async (id: number) => {
        try {
            const response = await fetch(`https://localhost:7143/api/v1/orders/${id}/payment`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    "payment_method_id": null
                })
            });

            if (!response.ok) {
                const errorData = await response.json();
                setServerError(errorData.message || 'Failed to initialize payment');
                return;
            }

            const data: PaymentIntentResponse = await response.json();
            setClientSecret(data.clientSecret);
            setAmount(data.amount);
            setServerError('');
        } catch (err) {
            setServerError('Payment initialization failed');
        }
    };

    return (
        <div className="min-h-screen bg-gray-50 flex items-center justify-center p-4">
            <div className="w-full max-w-md bg-white p-6 rounded-lg shadow-md">
                <div className="mb-6">
                    <label className="block text-sm font-medium text-gray-700">Order Number</label>
                    <input
                        type="number"
                        value={orderId}
                        onChange={(e) => setOrderId(Number(e.target.value))}
                        className="mt-1 block w-full rounded-md border-gray-300 shadow-sm"
                    />
                    <button
                        onClick={() => handleOrderSubmit(orderId)}
                        className="mt-2 w-full py-2 px-4 bg-blue-600 text-white rounded"
                    >
                        Load Order
                    </button>
                </div>

                {serverError && (
                    <div className="mb-4 p-3 text-sm text-red-600 bg-red-50 rounded">
                        {serverError}
                    </div>
                )}

                {clientSecret ? (
                    <Elements
                        stripe={stripePromise}
                        options={{/*...*/}}
                    >
                        <PaymentForm orderId={orderId} amount={amount/100} clientSecret={clientSecret} />
                    </Elements>
                ) : (
                    <div className="text-center p-4">Enter order number and click Load Order</div>
                )}
            </div>
        </div>
    );
};

export default CheckoutWrapper;