import requests

BASE_URL = "https://api.primedice.com/api"

def place_bet(api_key: str, amount: float, chance: float, high: bool) -> dict:
    """Melakukan taruhan di Primedice API."""
    payload = {
        "amount": amount,
        "chance": chance,
        "target": "high" if high else "low"
    }

    headers = {
        "x-api-key": api_key,
        "Content-Type": "application/json"
    }

    try:
        response = requests.post(f"{BASE_URL}/bet", json=payload, headers=headers)
        response.raise_for_status()
        return response.json()
    except requests.exceptions.RequestException as e:
        return {"error": str(e)}
