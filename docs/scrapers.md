# Scraper Stuff

## You might need pip

```command
curl https://bootstrap.pypa.io/get-pip.py -o get-pip.py
python get-pip.py
```

## Running locally

Creating a python environment. You only need to do this when creating a new scraper. Infact potentially we can reuse the same `requirements.txt` as they will likely have all the same requirements.

```command
python -m venv venv
```

Activate the python environment on windows

```command
.\venv\Scripts\Activate
```

Install the dependencies

> Note: requirements file is shared between scrapers

```command
pip install -r ../requirements.txt
```

Run scraper

```command
python yahoo_finance_scraper.py
```
