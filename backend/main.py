from fastapi import FastAPI, HTTPException
from fastapi.middleware.cors import CORSMiddleware
from pydantic import BaseModel
import requests

from core_bot.primedice_api import place_bet

app = FastAPI()

# Boleh diakses dari semua origin (frontend)
app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],  # Nanti bisa diganti ke domain tertentu
    allow_methods=["*"],
    allow_headers=["*"],
)

# Format data dari frontend untuk bet
class BetRequest(BaseModel):
    api_key: str
    amount: float
    chance: float
    high: bool

# Format data dari frontend untuk cek saldo
class BalanceRequest(BaseModel):
    api_key: str

# Endpoint untuk PLACE BET
@app.post("/api/bet")
def place_bet_route(data: BetRequest):
    try:
        result = place_bet(
            api_key=data.api_key,
            amount=data.amount,
            chance=data.chance,
            high=data.high
        )
        return result
    except Exception as e:
        raise HTTPException(status_code=500, detail=str(e))

# Endpoint untuk GET BALANCE
@app.post("/api/balance")
def get_balance(data: BalanceRequest):
    headers = {
        "x-api-key": data.api_key,
        "Content-Type": "application/json"
    }

    try:
        response = requests.get("https://api.primedice.com/api/account/balance", headers=headers)
        response.raise_for_status()
        balance_data = response.json()
        return {
            "balance": balance_data.get("balance", "N/A"),
            "currency": balance_data.get("currency", "BTC")
        }
    except requests.HTTPError as e:
        raise HTTPException(status_code=response.status_code, detail="Primedice API error: " + str(e))
    except Exception as e:
        raise HTTPException(status_code=500, detail="Server error: " + str(e))
