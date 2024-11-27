import requests
import time
import logging
from datetime import datetime
from textblob import TextBlob
import spacy
from typing import Dict, List
import os

# Configure logging
logging.basicConfig(
    level=logging.INFO,
    format='%(asctime)s - %(name)s - %(levelname)s - %(message)s'
)
logger = logging.getLogger(__name__)

class ReviewProcessor:
    def __init__(self):
        # Get API URL from environment variable, default to localhost
        self.api_base_url = os.getenv('API_BASE_URL', 'http://localhost:5000')
        self.headers = {'Content-Type': 'application/json'}
        
        # Load spaCy model
        logger.info("Loading spaCy model...")
        self.nlp = spacy.load('en_core_web_sm')
        logger.info("spaCy model loaded")

    def fetch_unprocessed_reviews(self) -> List[Dict]:
        """Fetch reviews that need processing"""
        try:
            response = requests.get(
                f"{self.api_base_url}/api/reviews/unprocessed",
                headers=self.headers
            )
            response.raise_for_status()
            reviews = response.json()
            logger.info(f"Fetched {len(reviews)} unprocessed reviews")
            return reviews
        except Exception as e:
            logger.error(f"Error fetching unprocessed reviews: {str(e)}")
            return []

    def analyze_review(self, review_text: str) -> Dict:
        """Perform NLP analysis on the review text"""
        try:
            # Basic sentiment analysis with TextBlob
            blob = TextBlob(review_text)
            sentiment_score = blob.sentiment.polarity

            # Process with spaCy
            doc = self.nlp(review_text)

            # Extract themes and their sentiments
            themes = self.analyze_themes(doc)

            # Extract key phrases (noun chunks)
            key_phrases = [chunk.text for chunk in doc.noun_chunks 
                         if len(chunk.text.split()) >= 2]

            return {
                "sentiment_score": sentiment_score,
                "themes": themes,
                "key_phrases": key_phrases
            }
        except Exception as e:
            logger.error(f"Error analyzing review text: {str(e)}")
            raise

    def analyze_themes(self, doc) -> List[Dict]:
        """Extract common themes and their sentiments"""
        # Define common product review themes
        theme_keywords = {
            "Quality": ["quality", "build", "material", "durability", "construction"],
            "Price": ["price", "cost", "value", "worth", "expensive", "cheap"],
            "Performance": ["performance", "speed", "efficient", "effective", "works"],
            "Design": ["design", "look", "style", "aesthetic", "appearance"],
            "Usability": ["easy", "simple", "convenient", "practical", "user-friendly"]
        }

        themes = []
        text = doc.text.lower()

        for theme, keywords in theme_keywords.items():
            # Check if any theme keywords are present
            theme_mentions = [word for word in keywords if word in text]
            if theme_mentions:
                # Get sentences containing theme keywords
                supporting_phrases = []
                sentiment_sum = 0
                count = 0
                
                for sent in doc.sents:
                    sent_text = sent.text.lower()
                    if any(keyword in sent_text for keyword in keywords):
                        supporting_phrases.append(sent.text)
                        sent_sentiment = TextBlob(sent.text).sentiment.polarity
                        sentiment_sum += sent_sentiment
                        count += 1

                if count > 0:
                    themes.append({
                        "theme": theme,
                        "count": count,
                        "sentiment_score": sentiment_sum / count,
                        "supporting_phrases": supporting_phrases
                    })

        return themes

    def update_review_analysis(self, review_id: str, analysis: Dict) -> bool:
        """Send analyzed data back to the API"""
        try:
            response = requests.put(
                f"{self.api_base_url}/api/reviews/{review_id}/analysis",
                headers=self.headers,
                json=analysis
            )
            response.raise_for_status()
            logger.info(f"Successfully updated analysis for review {review_id}")
            return True
        except Exception as e:
            logger.error(f"Error updating review {review_id}: {str(e)}")
            return False

    def process_reviews(self):
        """Main processing loop"""
        while True:
            try:
                # Fetch unprocessed reviews
                reviews = self.fetch_unprocessed_reviews()
                
                if not reviews:
                    logger.info("No unprocessed reviews found. Waiting...")
                    time.sleep(60)  # Wait a minute before next check
                    continue

                # Process each review
                for review in reviews:
                    try:
                        logger.info(f"Processing review {review['id']}")
                        
                        # Perform analysis
                        analysis_result = self.analyze_review(review['rawText'])
                        
                        # Update review with analysis results
                        if self.update_review_analysis(review['id'], analysis_result):
                            logger.info(f"Successfully processed review {review['id']}")
                        else:
                            logger.error(f"Failed to update review {review['id']}")
                    
                    except Exception as e:
                        logger.error(f"Error processing review {review['id']}: {str(e)}")
                        continue

            except Exception as e:
                logger.error(f"Error in processing loop: {str(e)}")
                time.sleep(60)  # Wait before retrying

if __name__ == "__main__":
    processor = ReviewProcessor()
    logger.info("Starting review processor")
    processor.process_reviews()