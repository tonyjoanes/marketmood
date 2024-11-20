// src/services/productService.ts
import { Product } from '../types/Product';

const API_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5000';

export const productService = {
    async getProducts(): Promise<Product[]> {
        const response = await fetch(`${API_URL}/api/products`);
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        return response.json();
    },

    async getProduct(id: string): Promise<Product> {
        const response = await fetch(`${API_URL}/api/products/${id}`);
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        return response.json();
    },

    async createProduct(data: CreateProductRequest): Promise<Product> {
        const response = await fetch(`${API_URL}/api/products`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(data),
        });
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        return response.json();
    }
};

interface CreateProductRequest {
    name: string;
    description: string;
    brand: string;
    categories: string[];
}