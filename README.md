[![](https://img.shields.io/nuget/v/Soenneker.Utils.Reflection.svg?style=for-the-badge)](https://www.nuget.org/packages/Soenneker.Utils.Reflection/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.utils.reflection/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.utils.reflection/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/Soenneker.Utils.Reflection.svg?style=for-the-badge)](https://www.nuget.org/packages/Soenneker.Utils.Reflection/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.utils.reflection/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.utils.reflection/actions/workflows/codeql.yml)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Utils.Reflection
A utility library for various Reflection based operations.

## Installation

```bash
dotnet add package Soenneker.Utils.Reflection
```

## Quick start

```csharp
using Soenneker.Utils.Reflection;
```

Call the static `ReflectionUtil` methods directly; no dependency-injection registration is required.

## Usage

```csharp
public static class Headers
{
    public const string RequestId = "X-Request-Id";
    public const string CorrelationId = "X-Correlation-Id";

    public const int MaximumLength = 128; // ignored: not a string
    public static readonly string RuntimeValue = "ignored"; // ignored: not const
}

Dictionary<string, string> headers = ReflectionUtil.GetConstantsFromType<Headers>();

// The runtime-Type overload is equivalent:
Dictionary<string, string> sameHeaders = ReflectionUtil.GetConstantsFromType(typeof(Headers));
```

The result contains public static literal fields whose value is a non-null string. Non-public
fields, `static readonly` fields, non-string constants, and null string constants are ignored.
Public inherited constants returned by `Type.GetFields(BindingFlags.Public | BindingFlags.Static)`
are included as well.

Metadata is reflected once per `Type` and cached for the process lifetime. Every call materializes
a new case-sensitive `Dictionary<string, string>`, so adding or removing entries from the returned
dictionary does not affect later callers. The cache has no eviction; avoid feeding an unbounded
stream of dynamically generated types.

Field order is not part of the contract. If a type hierarchy exposes multiple included fields with
the same name, dictionary materialization throws because names are used as unique keys.
