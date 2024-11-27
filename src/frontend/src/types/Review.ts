// types/Review.ts
export interface Review {
    id: string;
    rating: number;
    title: string;
    content: string;
    author: string;
    date: string;
    verifiedPurchase: boolean;
    helpfulVotes: number;
}