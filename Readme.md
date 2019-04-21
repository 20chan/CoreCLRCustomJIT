# CoreCLRCustomJIT

> WIP

This project hook JIT compiler at runtime and replace it, so you could hook compiling method and modify its compiled result - machine code.

## Example

```csharp
void Main() {
    var jit = new JIT();
    jit.HookMethod(this.GetType().GetMethod(nameof(One)), code => {
        Marshal.WriteByte(code, 37, 2);
    });

    if (jit.Hook()) {
        Console.WriteLine(One());
    }
}

int One() {
    return 1;
}
```

## References

[xoofx's awesome post](https://xoofx.com/blog/2018/04/12/writing-managed-jit-in-csharp-with-coreclr/)
