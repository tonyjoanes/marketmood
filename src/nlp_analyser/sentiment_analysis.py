import pymongo
import pandas as pd
import numpy as np
import nltk
import spacy
from textblob import TextBlob

# Download NLTK data files
nltk.download('punkt')
nltk.download('stopwords')
nltk.download('wordnet')

# Initialize SpaCy
nlp = spacy.load('en_core_web_sm')

# MongoDB setup
client = pymongo.MongoClient("mongodb://localhost:27017/")
db = client["financial_data"]
processed_collection = db["processed_news"]
analyzed_collection = db["analyzed_news"]

def analyze_sentiment(text):
    blob = TextBlob(text)
    return blob.sentiment.polarity, blob.sentiment.subjectivity

def analyze_entities(text):
    doc = nlp(text)
    entities = [(entity.text, entity.label_) for entity in doc.ents]
    return entities

def analyze_articles():
    articles = processed_collection.find()
    analyzed_articles = []
    
    for article in articles:
        sentiment_polarity, sentiment_subjectivity = analyze_sentiment(article["summary"])
        entities = analyze_entities(article["summary"])
        
        analyzed_article = {
            "title": article["title"],
            "summary": article["summary"],
            "date": article["date"],
            "link": article["link"],
            "sentiment_polarity": sentiment_polarity,
            "sentiment_subjectivity": sentiment_subjectivity,
            "entities": entities
        }
        analyzed_articles.append(analyzed_article)
    
    if analyzed_articles:
        analyzed_collection.insert_many(analyzed_articles)

if __name__ == "__main__":
    analyze_articles()
