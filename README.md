# Primedice.NET
Wraps the functionality of the [Primedice][] API in C#.
The project is written in .NET 4.5 as portable class library.

For versioning, [Semantic Versioning 2.0.0][]'s conventions are used.

[Primedice]: https://primedice.com
[Semantic Versioning 2.0.0]: http://semver.org/spec/v2.0.0.html

## Getting started
### Registering a new user
``` csharp
using KriPod.Primedice;

var username = "<username>";

// Initialize a new unauthorized instance of PrimediceClient
using (var client = new PrimediceClient()) {
    // Create a new user
    var user = client.Users.Create(username);

    // The following is the most important property of the newly-created user
    // Store the value of AuthToken in order to access the created account later
    var authToken = user.AuthToken;
}
```

### Making a bet with an existing user
``` csharp
using KriPod.Primedice;

// NOTE: All the monetary amounts are in satoshi by default
var authToken =  "<authToken>";
var amount = 1; // 0.00000001 BTC
var condition = BetCondition.LowerThan;
var target = 49.5;

// Initialize a new authorized instance of PrimediceClient
using (var client = new PrimediceClient(authToken)) {
    // Place a new bet with the parameters specified above
    client.Bets.Create(amount, condition, target);
}
```
