# README #

### Company Xyz42 test task

(from https://docs.google.com/document/d/1w6jajyG97iGG34YKQdWjfeXjFbjVMoRa-sqymJWM3pE)

Create an application for transforming equation into canonical form. An equation can be of any order. It may contain any amount of variables and brackets.

The equation will be given in the following form:

    P1 + P2 + ... = ... + PN

where `P1..PN` - summands, which look like: 

    ax^k

where

- `a` - floating point value;
- `k` - integer value;
- `x` - variable (each summand can have many variables).
 
For example:

  1. `"x^2 + 3.5xy + y = y^2 - xy + y"` should be transformed into: `"x^2 - y^2 + 4.5xy = 0"`
  2. `"x = 1"` => `"x - 1 = 0"`
  3. `"x - (y^2 - x) = 0"` => `"2x - y^2 = 0"`
  4. `"x - (0 - (0 - x)) = 0"` => `"0 = 0"`
  etc.

It should be a C# console application with support of two modes of operation: “file” and “interactive”.

- In interactive mode application prompts user to enter equation and displays result on enter.
  This is repeated until user presses Ctrl+C.

- In the file mode application processes a file specified as parameter and writes the output into separate file with “.out” extension.
  Input file contains lines with equations, each equation on separate line. 


### Solution

It will be great if you add unit tests.

Source code must include Visual Studio solution file with necessary projects and configuration.
Do not include built binaries of your code into package.
