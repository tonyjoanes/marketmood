// src/components/ProductImage.tsx
import Image from 'next/image';
import { useState } from 'react';

interface ProductImageProps {
    src: string;
    alt: string;
    className?: string;
}

const ProductImage = ({ src, alt, className = '' }: ProductImageProps) => {
    const [error, setError] = useState(false);

    return (
        <div className={`relative w-full h-48 bg-gray-50 ${className}`}>
            {!error ? (
                <Image
                    src={src}
                    alt={alt}
                    fill
                    className="object-contain p-4"
                    sizes="(max-width: 768px) 100vw, (max-width: 1200px) 50vw, 33vw"
                    onError={() => setError(true)}
                    priority={false}
                />
            ) : (
                <div className="absolute inset-0 flex items-center justify-center text-gray-400">
                    <svg
                        className="w-16 h-16"
                        fill="none"
                        stroke="currentColor"
                        viewBox="0 0 24 24"
                    >
                        <path
                            strokeLinecap="round"
                            strokeLinejoin="round"
                            strokeWidth={2}
                            d="M4 16l4.586-4.586a2 2 0 012.828 0L16 16m-2-2l1.586-1.586a2 2 0 012.828 0L20 14m-6-6h.01M6 20h12a2 2 0 002-2V6a2 2 0 00-2-2H6a2 2 0 00-2 2v12a2 2 0 002 2z"
                        />
                    </svg>
                </div>
            )}
        </div>
    );
};

export default ProductImage;