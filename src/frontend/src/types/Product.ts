// src/types/product.ts
export interface Product {
    id: string;
    brand: string;
    model: string;
    type: string;
    sentiment: number;
    reviewCount: number;
    imageUrl?: string;
}
