// store/product-store.ts
import { create } from 'zustand'

interface ProductState {
    productToUpdate: string | null;
    setProductToUpdate: (id: string | null) => void;
    clearProductToUpdate: () => void;
}

export const useProductStore = create<ProductState>((set) => ({
    productToUpdate: null,
    setProductToUpdate: (id) => set({ productToUpdate: id }),
    clearProductToUpdate: () => set({ productToUpdate: null }),
}))