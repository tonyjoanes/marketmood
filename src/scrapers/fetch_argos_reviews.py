from pymongo import ASCENDING, DESCENDING, MongoClient
import requests
from bs4 import BeautifulSoup
from typing import List, Dict, Optional
import time
import random
from dataclasses import dataclass, asdict
from datetime import datetime
from pymongo.errors import DuplicateKeyError

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

class ArgosScraper:
    def __init__(self):
        self.headers = {
            'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36',
            'Accept': 'text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8',
            'Accept-Language': 'en-GB,en-US;q=0.9,en;q=0.8'
        }
        self.session = requests.Session()
        self.session.headers.update(self.headers)
        self.base_url = "https://www.argos.co.uk"
    
    def get_product_reviews(self, product_id: str, pages: int = 3, since_date: Optional[datetime] = None) -> List[Review]:
        """
        Fetch reviews for a product, optionally limiting to those after a specific date
        """
        reviews = []
        current_page = 1
        
        while current_page <= pages:
            try:
                # Construct the URL for the current page
                url = f"{self.base_url}/product/{product_id}/reviews"
                if current_page > 1:
                    url += f"?page={current_page}"
                
                print(f"Fetching page {current_page} from {url}")
                
                response = self.session.get(url)
                if response.status_code != 200:
                    print(f"Failed to fetch page {current_page}: Status code {response.status_code}")
                    break
                
                soup = BeautifulSoup(response.content, 'html.parser')
                page_reviews = self._parse_reviews_page(soup, product_id)
                
                if not page_reviews:
                    print(f"No reviews found on page {current_page}")
                    break
                
                # If we have a since_date, filter out older reviews
                if since_date:
                    page_reviews = [r for r in page_reviews if r.date > since_date]
                    if not page_reviews:
                        print(f"No new reviews found on page {current_page}")
                        break
                
                reviews.extend(page_reviews)
                
                # Check if there's a next page
                next_page = soup.find('a', {'aria-label': 'Next page'})
                if not next_page:
                    print("No more pages available")
                    break
                
                # Add random delay between requests
                time.sleep(random.uniform(2, 5))
                current_page += 1
                
            except Exception as e:
                print(f"Error fetching page {current_page}: {str(e)}")
                break
        
        return reviews
    
    def _parse_reviews_page(self, soup: BeautifulSoup, product_id: str) -> List[Review]:
        """Parse all reviews from a page"""
        reviews = []
        
        # First, let's print some debug info about the page structure
        print("\nDebug: Analyzing page structure...")
        
        # Find the reviews container
        reviews_section = soup.find('div', class_='reviews-container')
        if not reviews_section:
            print("Debug: No reviews container found")
            print("Available sections:", [div.get('class', []) for div in soup.find_all('div')])
            return reviews
        
        # Find all individual review elements
        review_elements = reviews_section.find_all('div', class_='review')
        print(f"Debug: Found {len(review_elements)} review elements")
        
        for review_elem in review_elements:
            try:
                review = self._parse_review(review_elem, product_id)
                if review:
                    reviews.append(review)
            except Exception as e:
                print(f"Error parsing review: {str(e)}")
                continue
        
        return reviews
    
    def _parse_review(self, review_elem: BeautifulSoup, product_id: str) -> Optional[Review]:
        """Parse a single review element"""
        try:
            # Print the review element structure for debugging
            print("\nDebug: Review element structure:")
            print(review_elem.prettify())
            
            # Generate a unique review ID from available data
            review_id = review_elem.get('id', '')
            if not review_id:
                timestamp = int(time.time() * 1000)
                review_id = f"argos_{product_id}_{timestamp}"
            
            # Find rating (usually in data-rating attribute or stars images)
            rating_elem = review_elem.find('span', {'class': 'rating'})
            rating = 0
            if rating_elem:
                # Count filled star elements or get from attribute
                stars = rating_elem.find_all('img', {'alt': 'Filled star'})
                rating = len(stars) if stars else int(rating_elem.get('data-rating', 0))
            
            # Find review title
            title_elem = review_elem.find('h3', {'class': ['review-title', 'review-heading']})
            title = title_elem.text.strip() if title_elem else ''
            
            # Find review content
            content_elem = review_elem.find('div', {'class': ['review-text', 'review-content']})
            content = content_elem.text.strip() if content_elem else ''
            
            # Find author
            author_elem = review_elem.find('span', {'class': ['author', 'reviewer-name']})
            author = author_elem.text.strip() if author_elem else 'Anonymous'
            
            # Find date
            date_elem = review_elem.find('time', {'class': 'review-date'}) or \
                       review_elem.find('span', {'class': 'review-date'})
            date_str = date_elem.get('datetime') if date_elem else None
            if date_str:
                try:
                    date = datetime.fromisoformat(date_str.replace('Z', '+00:00'))
                except ValueError:
                    # Fallback for different date format
                    date = datetime.strptime(date_str, '%d %B %Y')
            else:
                date = datetime.utcnow()
            
            # Find verified purchase status
            verified_elem = review_elem.find('span', {'class': 'verified-purchase'})
            verified_purchase = bool(verified_elem)
            
            # Find helpful votes
            helpful_elem = review_elem.find('span', {'class': 'helpful-votes'})
            helpful_votes = 0
            if helpful_elem:
                votes_text = helpful_elem.text.strip()
                try:
                    helpful_votes = int(''.join(filter(str.isdigit, votes_text)))
                except ValueError:
                    helpful_votes = 0
            
            return Review(
                product_id=product_id,
                review_id=review_id,
                rating=rating,
                title=title,
                content=content,
                author=author,
                date=date,
                verified_purchase=verified_purchase,
                helpful_votes=helpful_votes
            )
            
        except Exception as e:
            print(f"Error parsing review details: {str(e)}")
            return None

def main():
    # Initialize scraper
    scraper = ArgosScraper()
    
    # Test with Garmin bike computer
    product_id = "9453421"
    
    print(f"\nFetching reviews for Argos product ID: {product_id}")
    reviews = scraper.get_product_reviews(product_id, pages=1)  # Start with 1 page for testing
    
    print(f"\nFound {len(reviews)} reviews")
    
    # Print the first few reviews to verify structure
    for i, review in enumerate(reviews[:2], 1):
        print(f"\nReview #{i}")
        print(f"Title: {review.title}")
        print(f"Rating: {'⭐' * review.rating}")
        print(f"Author: {review.author}")
        print(f"Date: {review.date.strftime('%d %B %Y')}")
        print(f"Verified Purchase: {review.verified_purchase}")
        print(f"Content: {review.content[:200]}...")

if __name__ == "__main__":
    main()