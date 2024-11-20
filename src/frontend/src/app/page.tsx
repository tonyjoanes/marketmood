// app/page.tsx
'use client';

import React, { useState, useEffect } from 'react';
import SearchBar from '@/components/SearchBar';
import ProductCard from '@/components/ProductCard';
import { Product } from '@/types/Product';
import { productsData } from '@/data/Products';
import { productService } from '@/services/ProductService';

// Separate ProductGrid component
const ProductGrid = ({ products, title }: { products: Product[], title: string }) => (
  <section className="py-12 px-4">
    <div className="max-w-6xl mx-auto">
      <div className="flex items-center mb-6">
        <svg xmlns="http://www.w3.org/2000/svg" className="w-6 h-6 text-blue-500 mr-2" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
          <polyline points="23 6 13.5 15.5 8.5 10.5 1 18" />
          <polyline points="17 6 23 6 23 12" />
        </svg>
        <h2 className="text-2xl font-bold text-gray-900">{title}</h2>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        {products.map((product) => (
          <ProductCard key={product.id} product={product} />
        ))}
      </div>
    </div>
  </section>
);

const Page = () => {
  const [products, setProducts] = useState<Product[]>([]);
  const [searchResults, setSearchResults] = useState<Product[]>([]);
  const [isSearching, setIsSearching] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    loadProducts();
  }, []);

  const loadProducts = async () => {
    try {
      const data = await productService.getProducts();
      setProducts(data);
    } catch (err) {
      setError('Failed to load products');
      console.error(err);
    }
  };

  const handleSearch = (query: string) => {
    setIsSearching(true);
    const filtered = products.filter(product => {
      const searchLower = query.toLowerCase();
      const fullName = `${product.brand} ${product.model}`.toLowerCase();
      return fullName.includes(searchLower) ||
        product.type.toLowerCase().includes(searchLower);
    });
    setSearchResults(filtered);
    setIsSearching(false);
  };

  if (error) {
    return <div className="text-red-600 p-4">{error}</div>;
  }

  return (
    <div className="min-h-screen bg-gradient-to-b from-white to-gray-50">
      {/* Hero Section */}
      <section className="pt-20 pb-16 px-4">
        <div className="max-w-4xl mx-auto text-center">
          <h1 className="text-4xl font-bold mb-6 text-gray-900">
            Discover Product Insights That Matter
          </h1>
          <p className="text-lg text-gray-600 mb-8">
            Analyze thousands of reviews instantly. Make informed decisions with AI-powered insights.
          </p>

          <SearchBar onSearch={handleSearch} products={productsData} />

          {isSearching && (
            <div className="mt-4 text-gray-600">
              Searching...
            </div>
          )}
        </div>
      </section>

      {/* Search Results */}
      {searchResults.length > 0 && (
        <ProductGrid products={searchResults} title="Search Results" />
      )}

      {/* Show Trending Products only if no search results */}
      {searchResults.length === 0 && !isSearching && (
        <>
          <ProductGrid products={productsData} title="Trending Products" />

          {/* Market Insights Section */}
          <section className="py-12 px-4 bg-white">
            <div className="max-w-6xl mx-auto">
              <div className="flex items-center mb-6">
                <svg xmlns="http://www.w3.org/2000/svg" className="w-6 h-6 text-blue-500 mr-2" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                  <rect x="2" y="2" width="20" height="20" rx="2" />
                  <line x1="6" y1="6" x2="6" y2="18" />
                  <line x1="10" y1="10" x2="10" y2="18" />
                  <line x1="14" y1="14" x2="14" y2="18" />
                  <line x1="18" y1="8" x2="18" y2="18" />
                </svg>
                <h2 className="text-2xl font-semibold">Latest Market Insights</h2>
              </div>

              <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                <div className="bg-white rounded-lg border shadow-sm p-6">
                  <h3 className="font-semibold mb-2">Rising Categories</h3>
                  <p className="text-gray-600">
                    Premium multisport watches showing increased positive sentiment
                  </p>
                </div>
                <div className="bg-white rounded-lg border shadow-sm p-6">
                  <h3 className="font-semibold mb-2">Customer Priorities</h3>
                  <p className="text-gray-600">
                    Battery life and GPS accuracy are top concerns in recent reviews
                  </p>
                </div>
              </div>
            </div>
          </section>
        </>
      )}
    </div>
  );
};

export default Page;