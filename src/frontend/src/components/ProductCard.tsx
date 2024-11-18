// src/components/ProductCard.tsx
import ProductImage from './ProductImage';
import { Product } from '../types/Product'

interface ProductCardProps {
    product: Product;
}

const ProductCard = ({ product }: ProductCardProps) => (
    <div className="bg-white rounded-lg border shadow-sm hover:shadow-lg transition-shadow">
        <ProductImage
            src={product.imageUrl}
            alt={`${product.brand} ${product.model}`}
        />
        <div className="p-6">
            <div className="text-lg font-semibold mb-2">
                <span className="text-gray-900">{product.brand} </span>
                <span className="text-gray-800">{product.model}</span>
            </div>
            <div className="text-sm text-gray-700 font-medium mb-3">{product.type}</div>
            <div className="flex items-center justify-between text-sm">
                <div className="flex items-center">
                    <svg xmlns="http://www.w3.org/2000/svg" className="w-4 h-4 text-yellow-400 mr-1" viewBox="0 0 24 24" fill="currentColor">
                        <path d="M12 2l3.09 6.26L22 9.27l-5 4.87 1.18 6.88L12 17.77l-6.18 3.25L7 14.14 2 9.27l6.91-1.01L12 2z" />
                    </svg>
                    <span className="text-gray-700 font-medium">{product.sentiment} sentiment</span>
                </div>
                <div className="text-gray-700 font-medium">{product.reviewCount.toLocaleString()} reviews</div>
            </div>
            <div className="mt-4 pt-4 border-t">
                <button className="text-blue-600 hover:text-blue-700 font-semibold">
                    View Analysis →
                </button>
            </div>
        </div>
    </div>
);

export default ProductCard;