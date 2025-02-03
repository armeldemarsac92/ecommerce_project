import { create } from 'zustand'

interface OpenFoodFactsState {
    selectedProduct: {
        code: string;
        image_url: string;
    } | null;
    setSelectedProduct: (product: { code: string; image_url: string; } | null) => void;
    clearSelectedProduct: () => void;
}

export const useOpenFoodFactsStore = create<OpenFoodFactsState>((set) => ({
    selectedProduct: null,
    setSelectedProduct: (product) => set({ selectedProduct: product }),
    clearSelectedProduct: () => set({ selectedProduct: null }),
}))