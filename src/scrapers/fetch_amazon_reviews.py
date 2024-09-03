import requests
from bs4 import BeautifulSoup
import schedule
import time
import re

# Set to keep track of processed review IDs
processed_review_ids = set()

def extract_product_id(url):
    pattern = r'/dp/([A-Z0-9]+)'
    match = re.search(pattern, url)
    if match:
        return match.group(1)
    else:
        print("Could not extract product ID from the URL")
        return None

def fetch_amazon_reviews(product_url, review_type='most_recent'):
    product_id = extract_product_id(product_url)
    if not product_id:
        return
    
    if review_type == 'most_recent':
        reviews_url = f'https://www.amazon.co.uk/product-reviews/{product_id}/ref=cm_cr_arp_d_viewopt_rvwer?sortBy=recent'
    elif review_type == 'top_reviews':
        reviews_url = f'https://www.amazon.co.uk/product-reviews/{product_id}/ref=cm_cr_arp_d_viewopt_rvwer?sortBy=helpful'
    else:
        print("Invalid review type specified. Use 'most_recent' or 'top_reviews'.")
        return

    headers = {'User-Agent': 'Mozilla/5.0'}
    response = requests.get(reviews_url, headers=headers)
    
    if response.status_code == 200:
        print(f"Successfully fetched the {review_type.replace('_', ' ')} page")
        soup = BeautifulSoup(response.text, 'html.parser')
        reviews = extract_reviews(soup)
        print(f"Extracted {len(reviews)} reviews")
        for review in reviews:
            print(review)
    else:
        print(f"Failed to fetch the page. Status code: {response.status_code}")

def extract_reviews(soup):
    reviews = []
    for review in soup.find_all('div', {'data-hook': 'review'}):
        review_id = review.get('id')
        if review_id and review_id not in processed_review_ids:
            review_text = review.find('span', {'data-hook': 'review-body'}).text.strip()
            star_rating = review.find('i', {'data-hook': 'review-star-rating'}).text.split(' ')[0].strip()
            reviews.append({'id': review_id, 'text': review_text, 'rating': star_rating})
            processed_review_ids.add(review_id)
    return reviews

# Schedule the scraper to run once a day
schedule.every().day.at("02:00").do(fetch_amazon_reviews, 
                                    'https://www.amazon.co.uk/HP-Black-Original-Cartridge-N9K06AE/dp/B01EA0EG3W?pd_rd_w=2Tbp1&content-id=amzn1.sym.701f4cdb-5c61-4b0f-855d-750559e8d2d8&pf_rd_p=701f4cdb-5c61-4b0f-855d-750559e8d2d8&pf_rd_r=C1RMBDA0DZASF3JMB4FE&pd_rd_wg=irSM6&pd_rd_r=84051662-3b77-4b63-8b92-8bea4b657eac&pd_rd_i=B01EA0EG3W&psc=1&ref_=pd_bap_d_grid_rp_0_2_scp_i', 
                                    'most_recent')

# Test immediately by calling the function directly with a valid product URL and review type
print("Testing the fetch_amazon_reviews function immediately...")
fetch_amazon_reviews('https://www.amazon.co.uk/HP-Black-Original-Cartridge-N9K06AE/dp/B01EA0EG3W?pd_rd_w=2Tbp1&content-id=amzn1.sym.701f4cdb-5c61-4b0f-855d-750559e8d2d8&pf_rd_p=701f4cdb-5c61-4b0f-855d-750559e8d2d8&pf_rd_r=C1RMBDA0DZASF3JMB4FE&pd_rd_wg=irSM6&pd_rd_r=84051662-3b77-4b63-8b92-8bea4b657eac&pd_rd_i=B01EA0EG3W&psc=1&ref_=pd_bap_d_grid_rp_0_2_scp_i', 
                    'most_recent')

while True:
    schedule.run_pending()
    time.sleep(1)
