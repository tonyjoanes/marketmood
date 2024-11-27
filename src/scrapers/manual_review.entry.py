import requests
from typing import List, Dict, Optional
from dataclasses import dataclass
from datetime import datetime
import json

@dataclass
class CreateReviewRequest:
    productId: str
    reviewId: str
    rating: int
    title: str
    content: str
    author: str
    date: datetime
    verifiedPurchase: bool
    helpfulVotes: int
    source: str  # Added source field

class ReviewApiClient:
    def __init__(self, base_url: str = "http://localhost:5000"):
        self.base_url = base_url
        self.session = requests.Session()
        self.session.headers.update({
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        })

    def create_single_review(self, review: CreateReviewRequest) -> Dict:
        """Create a single review via the API"""
        url = f"{self.base_url}/api/productreviews"
        
        # Convert datetime to ISO format string for JSON serialization
        review_dict = {
            "productId": review.productId,
            "reviewId": review.reviewId,
            "rating": review.rating,
            "title": review.title,
            "content": review.content,
            "author": review.author,
            "date": review.date.isoformat(),
            "verifiedPurchase": review.verifiedPurchase,
            "helpfulVotes": review.helpfulVotes,
            "source": review.source
        }
        
        try:
            response = self.session.post(url, json=review_dict)
            response.raise_for_status()
            return response.json()
        except requests.exceptions.RequestException as e:
            print(f"Error creating review: {str(e)}")
            if hasattr(e.response, 'text'):
                print(f"Response: {e.response.text}")
            raise

    def create_batch_reviews(self, reviews: List[CreateReviewRequest]) -> Dict:
        """Create multiple reviews in a batch via the API"""
        url = f"{self.base_url}/api/productreviews/batch"
        
        # Convert reviews to dictionary format
        reviews_dict = [
            {
                "productId": r.productId,
                "reviewId": r.reviewId,
                "rating": r.rating,
                "title": r.title,
                "content": r.content,
                "author": r.author,
                "date": r.date.isoformat(),
                "verifiedPurchase": r.verifiedPurchase,
                "helpfulVotes": r.helpfulVotes,
                "source": r.source
            }
            for r in reviews
        ]
        
        try:
            response = self.session.post(url, json=reviews_dict)
            response.raise_for_status()
            return response.json()
        except requests.exceptions.RequestException as e:
            print(f"Error creating batch reviews: {str(e)}")
            if hasattr(e.response, 'text'):
                print(f"Response: {e.response.text}")
            raise

def main():
    # Initialize API client
    api_client = ReviewApiClient()
    
    # Example of manually collected Argos reviews
    manual_reviews = [
        CreateReviewRequest(
            productId="9453421",
            reviewId="argos_9453421_1",
            rating=5,
            title="Great bike computer",
            content="Really happy with this purchase. The navigation features are excellent and battery life is impressive.",
            author="John Smith",
            date=datetime(2024, 1, 15),
            verifiedPurchase=True,
            helpfulVotes=3,
            source="argos"
        ),
        CreateReviewRequest(
            productId="9453421",
            reviewId="argos_9453421_2",
            rating=4,
            title="Good but complex",
            content="Good device overall but takes some time to learn all the features.",
            author="Sarah Jones",
            date=datetime(2024, 1, 16),
            verifiedPurchase=True,
            helpfulVotes=1,
            source="argos"
        )
    ]
    
    print(f"Attempting to import {len(manual_reviews)} reviews...")
    
    try:
        # Try batch import first
        print("\nAttempting batch import...")
        result = api_client.create_batch_reviews(manual_reviews)
        print("Batch import result:", json.dumps(result, indent=2))
        
    except Exception as e:
        print(f"\nBatch import failed: {str(e)}")
        print("Falling back to individual imports...")
        
        # Fall back to individual imports if batch fails
        success_count = 0
        for review in manual_reviews:
            try:
                result = api_client.create_single_review(review)
                success_count += 1
                print(f"Successfully imported review: {review.reviewId}")
            except Exception as e:
                print(f"Failed to import review {review.reviewId}: {str(e)}")
        
        print(f"\nImport complete. Successfully imported {success_count} out of {len(manual_reviews)} reviews.")

if __name__ == "__main__":
    main()