# Primedice.NET [![AppVeyor CI](https://img.shields.io/appveyor/ci/kripod/primedice-net/master.svg)](https://ci.appveyor.com/project/kripod/primedice-net) [![NuGet](https://img.shields.io/nuget/v/Primedice.NET.svg)](https://www.nuget.org/packages/Primedice.NET)
Wraps the functionality of the [Primedice][] API in C#.
The project is written in .NET 4.5 as a portable class library.

For versioning, [Semantic Versioning 2.0.0][]'s conventions are used.

[Primedice]: https://primedice.com
[Semantic Versioning 2.0.0]: http://semver.org/spec/v2.0.0.html

## Getting started

The following examples assume that the project has been referenced with the 
using directive: `using KriPod.Primedice;`

Please be advised that all the monetary amounts are in satoshi by default
(1 satoshi = 0.00000001 bitcoin).

### Registering a new user
``` csharp
var username = "<username>";

// Initialize a new unauthorized instance of PrimediceClient
var client = new PrimediceClient();

// Create a new user
var user = await client.Users.Create(username);

// The following is the most important property of the newly-created user
// Store the value of AuthToken in order to access the created account later
var authToken = user.AuthToken;
```

### Making a bet with an existing user
``` csharp
var serverSeed = "<serverSeed>";
var authToken =  "<authToken>";
var amount = 1; // 0.00000001 BTC
var condition = BetCondition.LowerThan;
var target = 49.5F;

// Initialize a new authorized instance of PrimediceClient
// (Bet simulation is available by using an unauthorized instance)
var client = new PrimediceClient(authToken);

// Place a new bet with the parameters specified above
var bet = await client.Bets.Create(amount, condition, target);

// Verify the fairness of the bet whether it was lost
if (!bet.IsWon && !bet.Verify(serverSeed)) {
    // The roll was not calculated fairly. (This should never happen.)
}
```

### Withdrawing and depositing funds
``` csharp
var authToken =  "<authToken>";
var withdrawalAddress = "<bitcoinAddress>";
var amount = 100000; // 0.001 BTC

// Initialize a new authorized instance of PrimediceClient
var client = new PrimediceClient(authToken);

// Withdraw some amount of money to the withdrawal address specified above
var withdrawal = await client.Wallet.Withdraw(amount, withdrawalAddress);

// Query the deposit address of self
var depositAddress = await client.Wallet.GetDepositAddress();
```
