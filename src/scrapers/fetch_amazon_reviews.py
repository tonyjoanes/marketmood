from pymongo import ASCENDING, DESCENDING, MongoClient
import requests
from bs4 import BeautifulSoup
from typing import List, Dict, Optional
import time
import random
from dataclasses import asdict, dataclass
from datetime import datetime

@dataclass
class Review:
    product_id: str
    review_id: str
    rating: int
    title: str
    content: str
    author: str
    date: datetime
    verified_purchase: bool
    helpful_votes: int
    source: str

class ReviewRepository:
    def __init__(self, connection_string: str = "mongodb://localhost:27017"):
        self.client = MongoClient(connection_string)
        self.db = self.client.marketmood
        self.reviews = self.db.reviews
        self.product_metadata = self.db.product_metadata
        
        # Create indexes
        self.reviews.create_index([("review_id", ASCENDING)], unique=True)
        self.reviews.create_index([("product_id", ASCENDING), ("date", DESCENDING)])
        
    def save_reviews(self, reviews: List[Review]) -> Dict[str, int]:
        """
        Save reviews to MongoDB and return statistics about the operation
        """
        stats = {"attempted": 0, "inserted": 0, "duplicates": 0}
        
        for review in reviews:
            stats["attempted"] += 1
            try:
                self.reviews.insert_one(asdict(review))
                stats["inserted"] += 1
            except DuplicateKeyError:
                stats["duplicates"] += 1
                
        return stats
    
    def get_latest_review_date(self, product_id: str) -> Optional[datetime]:
        """
        Get the date of the most recent review for a product
        """
        latest_review = self.reviews.find_one(
            {"product_id": product_id},
            sort=[("date", DESCENDING)]
        )
        return latest_review["date"] if latest_review else None
    
    def update_product_metadata(self, product_id: str, last_scraped: datetime):
        """
        Update metadata about when the product was last scraped
        """
        self.product_metadata.update_one(
            {"product_id": product_id},
            {
                "$set": {
                    "last_scraped": last_scraped,
                    "updated_at": datetime.utcnow()
                }
            },
            upsert=True
        )

class AmazonScraper:
    def __init__(self):
        self.headers = {
            'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36'
        }
        self.base_url = "https://www.amazon.co.uk"
    
    def get_product_reviews(self, product_id: str, pages: int = 3, since_date: Optional[datetime] = None) -> List[Review]:
        reviews = []
        for page in range(1, pages + 1):
            url = f"{self.base_url}/dp/{product_id}/ref=cm_cr_getr_d_paging_btm_next_{page}?pageNumber={page}"
            
            try:
                response = requests.get(url, headers=self.headers)
                print(f"Fetched {response.status_code} reviews for product {product_id}")
                
                if response.status_code == 200:
                    page_reviews = self._parse_reviews_page(response.content, product_id)
                    print(page_reviews)
                    # If we have a since_date, only keep newer reviews
                    if since_date:
                        page_reviews = [r for r in page_reviews if r.date > since_date]
                        
                        # If all reviews on this page are old, we can stop paginating
                        if not page_reviews:
                            print(f"No new reviews found on page {page}, stopping pagination")
                            break
                            
                    reviews.extend(page_reviews)
                
                # Respect rate limits
                time.sleep(random.uniform(2, 5))
                
            except Exception as e:
                print(f"Error fetching page {page} for product {product_id}: {str(e)}")
                continue
                
        return reviews
    
    def _parse_reviews_page(self, html_content: bytes, product_id: str) -> List[Review]:
        soup = BeautifulSoup(html_content, 'html.parser')
        review_elements = soup.find_all('div', {'data-hook': 'review'})
        reviews = []
        print(soup.prettify())
        
        for element in review_elements:
            try:
                review = self._parse_review(element, product_id)
                if review:
                    reviews.append(review)
            except Exception as e:
                print(f"Error parsing review: {str(e)}")
                continue
                
        return reviews
    
    def _parse_review(self, review_element: BeautifulSoup, product_id: str) -> Optional[Review]:
        try:
            # Extract review ID from the element's ID attribute
            review_id = review_element.get('id', '')
            
            # Find rating
            rating_element = review_element.find('i', {'data-hook': 'review-star-rating'})
            rating = int(rating_element.text.split('.')[0]) if rating_element else 0
            
            # Find title
            title_element = review_element.find('a', {'data-hook': 'review-title'})
            title = title_element.text.strip() if title_element else ''
            
            # Find content
            content_element = review_element.find('span', {'data-hook': 'review-body'})
            content = content_element.text.strip() if content_element else ''
            
            # Find author
            author_element = review_element.find('span', {'class': 'a-profile-name'})
            author = author_element.text.strip() if author_element else ''
            
            # Find date
            date_element = review_element.find('span', {'data-hook': 'review-date'})
            date_str = date_element.text.split('on ')[-1] if date_element else ''
            date = datetime.strptime(date_str, '%d %B %Y')
            
            # Check if verified purchase
            verified_element = review_element.find('span', {'data-hook': 'avp-badge'})
            verified_purchase = bool(verified_element)
            
            # Find helpful votes
            helpful_element = review_element.find('span', {'data-hook': 'helpful-vote-statement'})
            helpful_votes = int(helpful_element.text.split()[0]) if helpful_element else 0
            
            return Review(
                product_id=product_id,
                review_id=review_id,
                rating=rating,
                title=title,
                content=content,
                author=author,
                date=date,
                verified_purchase=verified_purchase,
                helpful_votes=helpful_votes,
                source="amazon"
            )
            
        except Exception as e:
            print(f"Error parsing review details: {str(e)}")
            return None

# def main():
#     # Example Garmin product IDs
#     garmin_products = [
#         'B091ZXYQXF',  # Garmin Venu 2
#         'DJ9WB3PIOUHB',  # Garmin Forerunner 55
#     ]
    
#     scraper = AmazonScraper()
#     repo = ReviewRepository()
    
#     for product_id in garmin_products:
#         print(f"\nProcessing product: {product_id}")
        
#         # Get the latest review date we have for this product
#         latest_review_date = repo.get_latest_review_date(product_id)
#         if latest_review_date:
#             print(f"Found existing reviews, fetching reviews since {latest_review_date}")
        
#         # Fetch reviews, passing the latest_review_date if we have one
#         reviews = scraper.get_product_reviews(product_id, since_date=latest_review_date)
#         print(f"Fetched {len(reviews)} new reviews")
        
#         # Save the reviews and get statistics
#         if reviews:
#             # stats = repo.save_reviews(reviews)
#             print("Storage statistics:")
#             print(f"- Attempted to save: {stats['attempted']}")
#             print(f"- Successfully saved: {stats['inserted']}")
#             print(f"- Duplicates skipped: {stats['duplicates']}")
            
#             # Update metadata about when we last scraped this product
#             repo.update_product_metadata(product_id, datetime.utcnow())
        
#         # Print sample of new reviews
#         print("\nSample of new reviews:")
#         for i, review in enumerate(reviews[:2], 1):
#             print(f"\nReview #{i}")
#             print(f"Title: {review.title}")
#             print(f"Rating: {'⭐' * review.rating}")
#             print(f"Date: {review.date.strftime('%d %B %Y')}")
#             print(f"Content: {review.content[:100]}...")

def main():
    # Example Garmin product IDs
    garmin_products = [
        'B091ZXYQXF',  # Garmin Venu 2
        'DJ9WB3PIOUHB',  # Garmin Forerunner 55
    ]
    
    scraper = AmazonScraper()
    
    for product_id in garmin_products:
        print(f"\nProcessing product: {product_id}")
        
        # Fetch reviews
        reviews = scraper.get_product_reviews(product_id)
        print(f"Fetched {len(reviews)} reviews")
        
        # Print all reviews
        print("\nSample of reviews:")
        for i, review in enumerate(reviews, 1):
            print(f"\nReview #{i}")
            print(f"Title: {review.title}")
            print(f"Rating: {'⭐' * review.rating}")
            print(f"Date: {review.date.strftime('%d %B %Y')}")
            print(f"Content: {review.content}")
            print(f"Author: {review.author}")
            print(f"Verified Purchase: {'Yes' if review.verified_purchase else 'No'}")
            print(f"Helpful Votes: {review.helpful_votes}")


if __name__ == "__main__":
    main()