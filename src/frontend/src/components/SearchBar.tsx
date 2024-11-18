// src/components/SearchBar.tsx
'use client';

import React, { useState } from 'react';
import { Product } from '../types/Product'

interface SearchBarProps {
    onSearch: (query: string) => void;
    products: Product[];
}

const SearchBar = ({ onSearch, products }: SearchBarProps) => {
    const [searchQuery, setSearchQuery] = useState('');
    const [showSuggestions, setShowSuggestions] = useState(false);

    const filteredSuggestions = products.filter(item => {
        const searchLower = searchQuery.toLowerCase();
        const fullName = `${item.brand} ${item.model}`.toLowerCase();
        return fullName.includes(searchLower) ||
            item.type.toLowerCase().includes(searchLower);
    });

    const handleSearch = (e: React.FormEvent) => {
        e.preventDefault();
        onSearch(searchQuery);
        setShowSuggestions(false);
    };

    const handleSuggestionClick = (product: Product) => {
        const query = `${product.brand} ${product.model}`;
        setSearchQuery(query);
        setShowSuggestions(false);
        onSearch(query);
    };

    return (
        <div className="relative max-w-2xl mx-auto">
            <form onSubmit={handleSearch}>
                <div className="relative">
                    <input
                        type="text"
                        value={searchQuery}
                        onChange={(e) => setSearchQuery(e.target.value)}
                        onFocus={() => setShowSuggestions(true)}
                        placeholder="Search for wearable devices... (e.g., Garmin Forerunner 55)"
                        className="w-full px-6 py-4 rounded-full border shadow-sm focus:ring-2 focus:ring-blue-500 focus:border-transparent outline-none
                       text-gray-800 placeholder-gray-500 font-medium text-lg"
                    />
                    <button
                        type="submit"
                        className="absolute right-3 top-1/2 -translate-y-1/2 p-2 bg-blue-500 text-white rounded-full hover:bg-blue-600"
                    >
                        <svg
                            xmlns="http://www.w3.org/2000/svg"
                            className="h-5 w-5"
                            fill="none"
                            viewBox="0 0 24 24"
                            stroke="currentColor"
                        >
                            <path
                                strokeLinecap="round"
                                strokeLinejoin="round"
                                strokeWidth={2}
                                d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z"
                            />
                        </svg>
                    </button>
                </div>
            </form>

            {showSuggestions && searchQuery.length > 0 && (
                <div
                    className="absolute z-10 w-full mt-2 bg-white rounded-lg shadow-lg border"
                    onMouseLeave={() => setShowSuggestions(false)}
                >
                    {filteredSuggestions.length > 0 ? (
                        <ul className="py-2">
                            {filteredSuggestions.map((product) => (
                                <li
                                    key={product.id}
                                    className="px-4 py-3 hover:bg-blue-50 cursor-pointer transition-colors duration-150"
                                    onClick={() => handleSuggestionClick(product)}
                                >
                                    <div className="flex justify-between items-center">
                                        <div className="text-gray-800">
                                            <span className="font-semibold">{product.brand} </span>
                                            <span className="font-medium">{product.model}</span>
                                        </div>
                                        <span className="text-sm font-medium text-gray-600">{product.type}</span>
                                    </div>
                                </li>
                            ))}
                        </ul>
                    ) : (
                        <div className="px-4 py-3 text-gray-700 font-medium">
                            No matching devices found
                        </div>
                    )}
                </div>
            )}
        </div>
    );
};

export default SearchBar;