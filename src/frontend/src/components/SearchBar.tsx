'use client';

import React, { useState } from 'react';
import { Search } from 'lucide-react';

const SearchBar = () => {
    const [searchQuery, setSearchQuery] = useState('');

    return (
        <div className="relative max-w-2xl mx-auto">
            <input
                type="text"
                value={searchQuery}
                onChange={(e) => setSearchQuery(e.target.value)}
                placeholder="Search for any product..."
                className="w-full px-6 py-4 rounded-full border shadow-sm focus:ring-2 focus:ring-blue-500 focus:border-transparent"
            />
            <button className="absolute right-3 top-1/2 -translate-y-1/2 p-2 bg-blue-500 text-white rounded-full hover:bg-blue-600">
                <Search className="w-5 h-5" />
            </button>
        </div>
    );
};

export default SearchBar;