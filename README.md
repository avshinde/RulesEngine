# RulesEngine

A flexible and lightweight rules engine built in C#. This project is designed to allow users to define, manage, and evaluate business rules dynamically without hard-coding logic. It is ideal for scenarios where rule logic changes frequently or needs to be managed externally.

## Features

- **Dynamic Rule Definition**: Create and update rules without modifying source code.
- **Rule Evaluation**: Evaluate rules on provided data objects at runtime.
- **Extensible**: Easily add new operators, conditions, and actions.
- **Separation of Concerns**: Clean architecture separating rule definitions from business logic.
- **Logging and Error Handling**: Built-in support for diagnostics and error reporting.

## Getting Started

### Prerequisites

- [.NET 6.0 SDK or later](https://dotnet.microsoft.com/download)
- IDE such as [Visual Studio](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)

### Installation

Clone the repository:

```bash
git clone https://github.com/avshinde/RulesEngine.git
cd RulesEngine
```

Restore dependencies:

```bash
dotnet restore
```

Build the project:

```bash
dotnet build
```

### Usage

1. Define your rules in a JSON or XML format (or as C# objects).
2. Load rules using the engine's API.
3. Pass data objects to the engine for rule evaluation.
4. Handle the results in your application logic.

#### Example

```csharp
var engine = new RulesEngine();
engine.LoadRules("rules.json"); // Load rules from a file

var input = new MyDataObject { /* ... */ };
var result = engine.Evaluate("RuleSetName", input);

if (result.IsSuccess)
{
    // Handle rule match
}
else
{
    // Handle rule failure
}
```

## Project Structure

- `src/` - Main engine codebase
- `tests/` - Unit and integration tests
- `examples/` - Example rules and sample usage

## Contributing

Contributions are welcome! Please fork the repository and submit pull requests. For major changes, open an issue first to discuss your ideas.

1. Fork the repo
2. Create your feature branch (`git checkout -b feature/fooBar`)
3. Commit your changes (`git commit -am 'Add some fooBar'`)
4. Push to the branch (`git push origin feature/fooBar`)
5. Create a new Pull Request

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

## Contact

Maintainer: [avshinde](https://github.com/avshinde)

---
