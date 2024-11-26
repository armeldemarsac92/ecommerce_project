"use client"

import React from 'react';
import dynamic from 'next/dynamic';

const DynamicPlayer = dynamic(() => import('@lottiefiles/react-lottie-player').then((mod) => mod.Player), {
    ssr: false,
});

export const Loader = () => {
    return (
        <DynamicPlayer
            autoplay
            loop
            src="/lotties/infinite-loader-animation.json"
            style={{ height: '250px', width: '250px' }}
            renderer="svg"
            speed={1.5}
        />
    );
};
