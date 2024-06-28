import requests
from bs4 import BeautifulSoup
import pymongo

# MongoDB setup
client = pymongo.MongoClient("mongodb://localhost:27017/")  # Use localhost if running locally
db = client["financial_data"]
collection = db["raw_news"]

# Function to scrape Yahoo Finance
def scrape_yahoo_finance():
    url = "https://finance.yahoo.com/news/"
    response = requests.get(url)
    soup = BeautifulSoup(response.content, 'html.parser')
    articles = []

    for item in soup.find_all('div', class_='Ov(h) Pend(44px) Pstart(25px)'):
        title = item.find('h3').text if item.find('h3') else 'No title'
        summary = item.find('p').text if item.find('p') else 'No summary'
        date = item.find('time')['datetime'] if item.find('time') else 'No date'
        link = item.find('a')['href'] if item.find('a') else 'No link'
        full_link = "https://finance.yahoo.com" + link

        articles.append({
            'title': title,
            'summary': summary,
            'date': date,
            'link': full_link
        })

    # Debugging statement to check the articles list
    print(f"Scraped {len(articles)} articles")

    if articles:  # Check if the articles list is not empty
        collection.insert_many(articles)
    else:
        print("No articles found. Nothing was inserted into the database.")

# Run the scraper
if __name__ == "__main__":
    scrape_yahoo_finance()
