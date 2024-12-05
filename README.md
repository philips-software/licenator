# Licenator

⚠️ This project is archived

Automatically gathers all the license URLs for all your NuGet packages.

## Usage

### Build the tool

```
build.bat
```

### Run the tool
```
Licenator <Root Directory> <Output filename>
```

#### Options 

Usage: `Licenator "<Root Directory>" "<Output filename>"`

```
Options:
    -o : Omits the "Used In" information from the output.
    -i : Specify a list of packages to be ignored.
Usage: 
    Licenator "<Root Directory>" "<Output filename>" -o -i "Package.To.Ignore.One" "Package.To.Ignore.Two"
```

## Maintainers

- Ben Bierens
- Gertjan Maas

## Issues

[Issues](https://github.com/philips-software/licenator/issues)

## License

License is MIT. See [LICENSE file](LICENSE.md)
