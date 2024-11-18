import { Product } from "@/types/Product";

// src/data/products.ts
export const productsData: Product[] = [
    {
        id: '1',
        brand: 'Garmin',
        model: 'Forerunner 55',
        type: 'Running Watch',
        sentiment: 4.5,
        reviewCount: 1250,
        imageUrl: '/images/products/garmin-forerunner-55.png'
    },
    {
        id: '2',
        brand: 'Garmin',
        model: 'Fenix 7',
        type: 'Multisport Watch',
        sentiment: 4.7,
        reviewCount: 890,
        imageUrl: '/images/products/garmin-fenix-7.jpg'
    },
    {
        id: '3',
        brand: 'Fitbit',
        model: 'Sense 2',
        type: 'Fitness Watch',
        sentiment: 4.2,
        reviewCount: 1500,
        imageUrl: '/images/products/fitbit-sense-2.jpg'
    },
    {
        id: '4',
        brand: 'Apple',
        model: 'Watch Series 9',
        type: 'Smartwatch',
        sentiment: 4.8,
        reviewCount: 3200,
        imageUrl: '/images/products/apple-watch-s9.jpg'
    },
    {
        id: '5',
        brand: 'Whoop',
        model: '4.0',
        type: 'Fitness Tracker',
        sentiment: 4.3,
        reviewCount: 750,
        imageUrl: '/images/products/whoop-4.jpg'
    }
];