import pymongo
import pandas as pd
import numpy as np
import nltk
import spacy
from sklearn.feature_extraction.text import TfidfVectorizer

# Download NLTK data files
nltk.download('punkt')
nltk.download('stopwords')
nltk.download('wordnet')

from nltk.tokenize import word_tokenize
from nltk.corpus import stopwords
from nltk.stem import WordNetLemmatizer

# MongoDB setup
client = pymongo.MongoClient("mongodb://localhost:27017/")
db = client["financial_data"]
raw_collection = db["raw_news"]
processed_collection = db["processed_news"]

# Initialize NLP tools
lemmatizer = WordNetLemmatizer()
stop_words = set(stopwords.words('english'))
nlp = spacy.load('en_core_web_sm')

def preprocess_text(text):
    # Tokenization
    tokens = word_tokenize(text)
    # Lowercasing
    tokens = [token.lower() for token in tokens]
    # Removing stopwords and punctuation
    tokens = [token for token in tokens if token.isalpha() and token not in stop_words]
    # Lemmatization
    tokens = [lemmatizer.lemmatize(token) for token in tokens]
    return ' '.join(tokens)

def preprocess_articles():
    articles = raw_collection.find()
    processed_articles = []
    
    for article in articles:
        processed_article = {
            "title": preprocess_text(article["title"]),
            "summary": preprocess_text(article["summary"]),
            "date": article["date"],
            "link": article["link"]
        }
        processed_articles.append(processed_article)
    
    if processed_articles:
        processed_collection.insert_many(processed_articles)

if __name__ == "__main__":
    preprocess_articles()
