# AI Helpers

## Links
[Link to ChatGPT Helper](https://chatgpt.com/share/9415f729-cc9e-445c-98d0-4e3bb2cee7f9)

## Prompts

---

**Prompt:**

Hi ChatGPT, I'm working on a project to collect, analyze, and serve customer feedback data from Amazon. Here's a summary of what we've discussed so far:

1. **High-Level Architecture**:
   - **Data Collection Service**: Scrape customer feedback data from Amazon and store it in MongoDB.
   - **Data Analysis Service**: Analyze the feedback data for sentiment and key metrics, then store the results in MongoDB.
   - **API Service**: Create a .NET Core API to serve the analyzed feedback data.
   - **Front-End Application**: Develop a front-end to visualize the feedback data and insights using React.

2. **Data Collection**:
   - Python script to fetch and extract reviews from Amazon.
   - Store raw review data in MongoDB.

3. **Data Analysis**:
   - Python script to perform sentiment analysis on the reviews using TextBlob.
   - Store the analyzed data in MongoDB.

4. **API Service**:
   - .NET Core API to expose endpoints for accessing the analyzed data.

5. **Front-End Application**:
   - React-based dashboard to visualize sentiment trends, feature highlights, and provide search/filter capabilities.

6. **Docker Setup**:
   - Dockerfiles for the scraper, analyzer, and API.
   - Docker Compose configuration to manage services.

Can you help me continue from here and provide detailed steps or code snippets for implementing these components?

---
