# CoreCLRCustomJIT

> WIP

This project hook JIT compiler at runtime and replace it, so you could hook compiling method and modify its compiled result - machine code.

## Example

```csharp
void Main() {
    var jit = new JIT();
    jit.HookMethod(this.GetType().GetMethod(nameof(One)), code => {
        // Makes return data of method `One` 1 to 2
        Marshal.WriteByte(code, 37, 2);
    });

    if (jit.Hook()) {
        Console.WriteLine(One()); // 2
    }
}

int One() {
    return 1;
}
```

## References

[xoofx's awesome post](https://xoofx.com/blog/2018/04/12/writing-managed-jit-in-csharp-with-coreclr/)
