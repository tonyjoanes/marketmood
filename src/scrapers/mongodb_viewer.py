from pymongo import MongoClient
from datetime import datetime
from pprint import pprint

class MongoViewer:
    def __init__(self, connection_string: str = "mongodb://localhost:27017"):
        self.client = MongoClient(connection_string)
        self.db = self.client.marketmood
        self.reviews = self.db.reviews
        self.product_metadata = self.db.product_metadata

    def view_all_reviews(self, limit: int = 10):
        """View all reviews with optional limit"""
        print(f"\nShowing first {limit} reviews:")
        print("-" * 80)
        
        for review in self.reviews.find().limit(limit):
            print(f"\nReview ID: {review.get('review_id')}")
            print(f"Product ID: {review.get('product_id')}")
            print(f"Rating: {'⭐' * review.get('rating', 0)}")
            print(f"Title: {review.get('title')}")
            print(f"Author: {review.get('author')}")
            print(f"Date: {review.get('date')}")
            print(f"Content: {review.get('content'):100)}..." if len(review.get('content', '')) > 100 else review.get('content'))
            print("-" * 80)

    def view_review_count(self):
        """Show total number of reviews"""
        count = self.reviews.count_documents({})
        print(f"\nTotal reviews in database: {count}")

    def view_products_with_reviews(self):
        """Show unique products and their review counts"""
        pipeline = [
            {"$group": {
                "_id": "$product_id",
                "count": {"$sum": 1},
                "avg_rating": {"$avg": "$rating"}
            }},
            {"$sort": {"count": -1}}
        ]
        
        print("\nProducts with reviews:")
        print("-" * 80)
        for product in self.reviews.aggregate(pipeline):
            print(f"Product ID: {product['_id']}")
            print(f"Number of reviews: {product['count']}")
            print(f"Average rating: {product['avg_rating']:.1f}")
            print("-" * 80)

    def search_reviews(self, query: dict):
        """Search reviews with a specific query"""
        print(f"\nSearching reviews with query: {query}")
        print("-" * 80)
        
        for review in self.reviews.find(query).limit(5):
            print(f"\nReview ID: {review.get('review_id')}")
            print(f"Product ID: {review.get('product_id')}")
            print(f"Rating: {'⭐' * review.get('rating', 0)}")
            print(f"Title: {review.get('title')}")
            print("-" * 80)

def main():
    viewer = MongoViewer()
    
    while True:
        print("\nMongoDB Viewer Menu:")
        print("1. View recent reviews")
        print("2. Show review count")
        print("3. Show products with review counts")
        print("4. Search reviews")
        print("5. Exit")
        
        choice = input("\nEnter your choice (1-5): ")
        
        if choice == "1":
            limit = int(input("How many reviews to show? "))
            viewer.view_all_reviews(limit)
        
        elif choice == "2":
            viewer.view_review_count()
        
        elif choice == "3":
            viewer.view_products_with_reviews()
        
        elif choice == "4":
            print("\nSearch options:")
            print("1. Search by product ID")
            print("2. Search by rating")
            print("3. Search by date range")
            
            search_choice = input("Enter search option (1-3): ")
            
            if search_choice == "1":
                product_id = input("Enter product ID: ")
                viewer.search_reviews({"product_id": product_id})
            
            elif search_choice == "2":
                rating = int(input("Enter rating (1-5): "))
                viewer.search_reviews({"rating": rating})
            
            elif search_choice == "3":
                start_date = input("Enter start date (YYYY-MM-DD): ")
                end_date = input("Enter end date (YYYY-MM-DD): ")
                start = datetime.strptime(start_date, "%Y-%m-%d")
                end = datetime.strptime(end_date, "%Y-%m-%d")
                viewer.search_reviews({"date": {"$gte": start, "$lte": end}})
        
        elif choice == "5":
            print("Goodbye!")
            break
        
        else:
            print("Invalid choice, please try again")

if __name__ == "__main__":
    main()