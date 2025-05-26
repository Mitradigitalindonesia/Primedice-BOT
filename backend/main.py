from fastapi import FastAPI, Request
from fastapi.middleware.cors import CORSMiddleware
from pydantic import BaseModel
from core_bot.primedice_api import place_bet

app = FastAPI()

# Boleh diakses dari semua origin (bisa ubah ke domain tertentu nanti)
app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"], 
    allow_methods=["*"],
    allow_headers=["*"],
)

# Format data request dari frontend
class BetRequest(BaseModel):
    api_key: str
    amount: float
    chance: float
    high: bool

@app.post("/api/bet")
def place_bet_route(data: BetRequest):
    result = place_bet(
        api_key=data.api_key,
        amount=data.amount,
        chance=data.chance,
        high=data.high
    )
    return result
