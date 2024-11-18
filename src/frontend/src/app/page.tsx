'use client';

import React, { useState } from 'react';
import SearchBar from '../components/SearchBar';
import { Product } from '@/types/product';

const productsData: Product[] = [
  {
    id: '1',
    brand: 'Garmin',
    model: 'Forerunner 55',
    type: 'Running Watch',
    sentiment: 4.5,
    reviewCount: 1250
  },
  {
    id: '2',
    brand: 'Garmin',
    model: 'Fenix 7',
    type: 'Multisport Watch',
    sentiment: 4.7,
    reviewCount: 890
  },
  {
    id: '3',
    brand: 'Fitbit',
    model: 'Sense 2',
    type: 'Fitness Watch',
    sentiment: 4.2,
    reviewCount: 1500
  },
  {
    id: '4',
    brand: 'Apple',
    model: 'Watch Series 9',
    type: 'Smartwatch',
    sentiment: 4.8,
    reviewCount: 3200
  },
  {
    id: '5',
    brand: 'Whoop',
    model: '4.0',
    type: 'Fitness Tracker',
    sentiment: 4.3,
    reviewCount: 750
  },
];

const Page = () => {
  const [searchResults, setSearchResults] = useState<Product[]>([]);
  const [isSearching, setIsSearching] = useState(false);

  const handleSearch = async (query: string) => {
    setIsSearching(true);
    // Simulating API call with setTimeout
    setTimeout(() => {
      const filtered = productsData.filter(product => {
        const searchLower = query.toLowerCase();
        const fullName = `${product.brand} ${product.model}`.toLowerCase();
        const type = product.type.toLowerCase();
        return fullName.includes(searchLower) || type.includes(searchLower);
      });
      setSearchResults(filtered);
      setIsSearching(false);
    }, 500);
  };

  const ProductGrid = ({ products, title }: { products: Product[], title: string }) => (
    <section className="py-12 px-4">
      <div className="max-w-6xl mx-auto">
        <div className="flex items-center mb-6">
          <svg xmlns="http://www.w3.org/2000/svg" className="w-6 h-6 text-blue-500 mr-2" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
            <polyline points="23 6 13.5 15.5 8.5 10.5 1 18" />
            <polyline points="17 6 23 6 23 12" />
          </svg>
          <h2 className="text-2xl font-semibold">{title}</h2>
        </div>

        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {products.map((product) => (
            <div key={product.id} className="bg-white rounded-lg border shadow-sm hover:shadow-lg transition-shadow p-6">
              <h3 className="text-lg font-semibold mb-2">{product.brand} {product.model}</h3>
              <div className="text-sm text-gray-600 mb-3">{product.type}</div>
              <div className="flex items-center justify-between text-sm text-gray-600">
                <div className="flex items-center">
                  <svg xmlns="http://www.w3.org/2000/svg" className="w-4 h-4 text-yellow-400 mr-1" viewBox="0 0 24 24" fill="currentColor">
                    <path d="M12 2l3.09 6.26L22 9.27l-5 4.87 1.18 6.88L12 17.77l-6.18 3.25L7 14.14 2 9.27l6.91-1.01L12 2z" />
                  </svg>
                  <span>{product.sentiment} sentiment</span>
                </div>
                <div>{product.reviewCount.toLocaleString()} reviews</div>
              </div>
              <div className="mt-4 pt-4 border-t">
                <button className="text-blue-500 hover:text-blue-600 font-medium">
                  View Analysis →
                </button>
              </div>
            </div>
          ))}
        </div>
      </div>
    </section>
  );

  return (
    <div className="min-h-screen bg-gradient-to-b from-white to-gray-50">
      {/* Hero Section */}
      <section className="pt-20 pb-16 px-4">
        <div className="max-w-4xl mx-auto text-center">
          <h1 className="text-4xl font-bold mb-6">
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