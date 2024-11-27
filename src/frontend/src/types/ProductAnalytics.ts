// types/ProductAnalytics.ts
export interface ProductAnalytics {
    sentimentScore: number;
    keyTopics: Array<{
        topic: string;
        count: number;
        sentiment: number;
    }>;
    ratingDistribution: {
        [key: number]: number;
    };
}