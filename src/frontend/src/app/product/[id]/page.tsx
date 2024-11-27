// app/product/[id]/page.tsx
'use client';

import { useEffect, useState } from 'react';
import { productService } from '@/services/ProductService';
import { Product } from '@/types/Product';

export default function ProductAnalysis({ params }: { params: { id: string } }) {
    const [product, setProduct] = useState<Product | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const loadProduct = async () => {
            try {
                const data = await productService.getProduct(params.id);
                setProduct(data);
            } catch (err) {
                setError('Failed to load product details');
                console.error(err);
            } finally {
                setLoading(false);
            }
        };

        loadProduct();
    }, [params.id]);

    if (loading) return <div className="p-4">Loading...</div>;
    if (error) return <div className="text-red-600 p-4">{error}</div>;
    if (!product) return <div className="p-4">Product not found</div>;

    return (
        <div className="min-h-screen bg-gray-50 py-8">
            <div className="max-w-7xl mx-auto px-4">
                {/* Product Header */}
                <div className="bg-white rounded-lg shadow-sm p-6 mb-6">
                    <h1 className="text-3xl font-bold mb-2">{product.brand} {product.model}</h1>
                    <p className="text-gray-600">{product.type}</p>
                </div>

                {/* Analysis Grid */}
                <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                    {/* Sentiment Score Card */}
                    <div className="bg-white rounded-lg shadow-sm p-6">
                        <h2 className="text-xl font-semibold mb-4">Overall Sentiment</h2>
                        {/* Add sentiment visualization */}
                    </div>

                    {/* Key Topics Card */}
                    <div className="bg-white rounded-lg shadow-sm p-6">
                        <h2 className="text-xl font-semibold mb-4">Key Topics</h2>
                        {/* Add topics list/chart */}
                    </div>

                    {/* Rating Distribution Card */}
                    <div className="bg-white rounded-lg shadow-sm p-6">
                        <h2 className="text-xl font-semibold mb-4">Rating Distribution</h2>
                        {/* Add rating distribution chart */}
                    </div>
                </div>

                {/* Reviews Section */}
                <div className="mt-8 bg-white rounded-lg shadow-sm p-6">
                    <h2 className="text-2xl font-semibold mb-6">Recent Reviews</h2>
                    {/* Add reviews list */}
                </div>
            </div>
        </div>
    );
}