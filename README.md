# Manco Language
<table>
  <tr>
    <td><img src="https://github.com/RodrigoPAml/MancoLanguage/assets/41243039/d73ac874-72e8-4f94-8ff1-1bc97daca54f" alt="Manco language logo" style="width: 100px; height: auto; margin-right: 20px;"></td>
    <td><h1 style="margin: 0; line-height: 1;">The Manco programming language</h1></td>
  </tr>
</table

Manco is a programming language i created from scratch for fun, designed to compile to both MIPS-based assembly or transpiles to C++ code. 

No external tools like *Yacc* or *Bison* were used in its development. It's implemented in C# with a Windows Forms GUI for user interaction.

## Preview

On the left is the code editor, and on the right are the generated assembly and the corresponding tokens of the code.

The compiler output and program output are in bottom.

Into the top, a select exists with the options of a MIPS Compiler or C++ transpiler,

![image](https://github.com/user-attachments/assets/37979c1a-5506-44f6-86c6-e3641d3b12c5)

An error in the editor and tokens involved on the left

![image](https://github.com/RodrigoPAml/MancoLanguage/assets/41243039/f0078c33-f456-4fd9-b01e-6cb63d11eb05)

### A nice GIF

![ezgif-7-945f170853](https://github.com/RodrigoPAml/MancoLanguage/assets/41243039/4e5b9709-5f1a-496b-9da9-5f148204bc6d)

# Running the project 

If you are going to build and run the project don't forget to do **git submodule init** and **git submodule update** before that and run the project **GUI** as the initial project.

The files examples are on the folder **Examples**

Or you can download tha **available release** that contains the **GUI** and some examples ready for use.

The code editor is a little buggier because of Windows Form limitations on syntax highlighting, so you can disable it if you want, it will still show errors on the text.

**Note**: To use the g++ compiler when using the transpiler mode you need to install g++. Use https://www.msys2.org/ to easy setup on windows.

## Features

- Primitive types like integer, decimal, bool and string
- Complex expression compilation like (10+20*2) > 200 and (true or false) and so on
- Array of primitives types with index access
- Functions that can receive primitives as value or by reference
- Strings and list are always passed by reference
- While loop, if, elif and else
- Functions do not return values, instead use reference values for return
- The language only uses the stack, no heap is used, which means no dynamic allocation, and fixed array sizes
- The language syntax is based on lua with some mix of c++ and imagination
- The language can be compile to a set of MIPS based assembly and will be executed with the assembly simulator embedded
- Or, be transpiled to C++ and executed with g++ (need to be installed to work)
  
### Example 1

```lua
-- A sum program, that passes a number with reference for the returned value

function sum(integer a, integer b, integer& ret)
    ret = a + b
end

function main()
    integer ret = 0
    sum(1, 3, ret)     
    print("The sum of 1 + 3 is ")
    print(ret)
end
```

Output
```
The sum of 1 + 3 is 4
Program exited 0
```

### Example 2

```lua
-- Program that prints pair numbers up to N

function pairs(integer n)
    integer idx = 2
    while idx < n
        if idx % 2 == 0
            print(idx)
            print(" ")
        end
        idx = idx + 1
    end
end

function main()
    pairs(150)     
end
```

Output
```
2 4 6 8 10 12 14 16 18 20 22 24 26 28 30 32 34 36 38 40 42 44 46 48 50 52 54 56 58 60 62 64 66 68 70 72 74 76 78 80 82 84 86 88 90 92 94 96 98 100 102 104 106 108 110 112 114 116 118 120 122 124 126 128 130 132 134 136 138 140 142 144 146 148 
Program exited 0
```
## Running the Manco API

To use the API provider to compile/transpile the code, use the Manco Project and take a look at the main, there is two examples of how to use it.

~~With the compiled code you can use the GUI of the [AssemblerSimulator](https://github.com/RodrigoPAml/AssemblerSimulator) project to run the code.~~

When compiling to MIPS based architecture, the project already include the assembler simulator to run and will have an ouput.

If you are transpiling to C++ make sure you have g++ installed and on the PATH and it will automatically compile the code and execute it, also showing the output.

### Example 1 - Executing with MIPS

![image](https://github.com/user-attachments/assets/57fda26b-f2ee-4fb9-a9e3-b199b74fa5e8)

Output:
```assembly
Código compilado:

j main

-- Instrução para função main with id 1
main:

-- Instrução print ( 10 * 2 )
lir t0 10
lir t1 2
mul t0 t0 t1
sw t0 0 sp
addi sp sp 4
jal #print_int

-- Fim scopo da função main
addi sp sp -4
j end

#print_int:
lw t0 -4 sp
lir v0 1
move a0 t0
syscall
jr ra
end:

Saída:

20
```

### Example 2 - Executing with g++

With g++ is way more faster since is native (runs directly on cpu)

![image](https://github.com/user-attachments/assets/2220259f-0185-4759-b294-00da2d51b4d0)

Output:
```c++
Código transpilado em C++:

// Transpiled code by manco language to C++
#include <iostream>
#include <chrono>

int main()
{
    auto start = std::chrono::high_resolution_clock::now();
    std::cout << 10 * 2;
    if (1 + 1 == 2)
    {
        std::cout << "\n1 + 1 is 2";
    }
    auto end = std::chrono::high_resolution_clock::now();
    std::chrono::duration<double, std::milli> duration = end - start;
    std::cout << std::endl << "Execution time: " << duration.count() << " milliseconds" << std::endl;
}
Invocando g++

Invocando programa compilado:

20
1 + 1 is 2
Execution time: 0.02 milliseconds
```

## Compiled or Interpreted?

  * If the language is compiled into a set of assembly based on the mips architecture, which does not run on current computers, it will run on my [AssemblerSimulator](https://github.com/RodrigoPAml/AssemblerSimulator) (used as a submodule) that
  i have developed. I choose this type of assembly because already learn it from university. So while the language is compiled into assembly, the assembly is getting interpreted, which is slow.

  * If you are using the transpiled C++ mode with the g++ compiler it will be really fast since is running directly on CPU.

## Phases implemented

### 1. Lexical Analysis

- **Description:** This phase analyzes the source code's stream of characters and converts it into meaningful tokens.
- **Purpose:** To break down the source code into individual tokens such as keywords, identifiers, operators, etc.
- **Implementation Details:** A Lexical parser use a regex to identify the tokens.

### 2. Syntax Analysis (Parsing)

- **Description:** This phase parses the sequence of tokens generated by the lexical analyzer according to the language's grammar rules. This phase do not check for meaning of the code, only for syntax. For example, it will not check if a variable exists, only if the rules to an assign is valid. Ex: TYPE IDENTIFIER EQUALS (NUMBER|FLOAT|BOOL|STRING).
- **Purpose:** To ensure that the source code follows the correct syntactic structure specified by the language grammar.
- **Implementation Details:** Tokens are parsed, and each line is validated according to a tree-like structure (like a finite state machine), also a stack is also used to control the open and close of scopes

### 3. Semantic Analysis

- **Description:** This phase examines the syntactic structure of the program to enforce the language's semantics, including type checking and scope analysis. For example in the syntax phase comparing a integer to a bool is a valid syntax, but in this phase is not, because it doesn't  make sense in the language's semantics.
- **Purpose:** To ensure that the program adheres to the language's semantic rules beyond its syntax.
- **Implementation Details:** Tokens are parsed, and each line is validated according to a tree-like structure (FSM), also a stack is also used to control the scopes. But in this phase it validate if the operations make sense. For example: if a variable exists, and if the assign values match the expected type. 

### 4. Assembly generation (When mips compilation selected)

- **Description:** This phase transforms the validated source code into an assembly code in MIPS based architecture
- **Purpose:** To provide code that runs direcly into the CPU
- **Implementation Details:** The compiler also parse each line in a tree-like structure, storing information on where each operation will be on the stack, while also inserting the assembly code and incrementing the stack usage. It read the tokens and identify the operation, the variables that are in the operations, in which scope/function/if we are doing it, and then write the code for that operation. Also when you finish a scope it do the proper cleaning of the stack, and much more.

### 4.1 Transpilation (When C++ transpilation selected)

- **Description:** This phase transforms the validated source code into C++
- **Purpose:** To also provide code that runs direcly into the CPU when the C++ is compiled
- **Implementation Details:** The transpiler converts the Manco code into C++ code using the tokens, tree and a stack structure for scope control.
  
### 5. Optimization

- **Description:** No optimization implemented, the assembly generated is very big and inefficient in MIPS mode. When using g++ compiler it uses the O2 flag for optimization.
- **Purpose:** To produce optimized code that executes faster or consumes fewer resources.

### 6. Execution MIPS

- **Description:** The execution of the assembler is done by a [AssemblerSimulator](https://github.com/RodrigoPAml/AssemblerSimulator) which is a public repository of mine, this is used as a submodule of this project.
- **Purpose:** To produce the final output that can be executed by the target platform.

### 6.1. Execution g++

- **Description:** The g++ compiler is invoked by the program to compile and execute the transpiled code. For this to work install g++ from [here](https://www.msys2.org/).
- **Purpose:** To also produce the final output that can be executed by the target platform.

## Examples
Some examples available in the folder to open and run.
  
### Hello world
![image](https://github.com/RodrigoPAml/MancoLanguage/assets/41243039/7f8463c7-59a2-464a-84a0-9546daf771ee)

### Prime numbers with MIPS

![image](https://github.com/user-attachments/assets/a0d8a7cc-adc5-4cda-a8db-c5cb5b006040)

### Prime number with g++

![image](https://github.com/user-attachments/assets/5b8bb147-fa15-46c8-8934-89d9c1d255c4)

### Bubble Sort with MIPS

![image](https://github.com/user-attachments/assets/5780f593-5286-4b81-a987-547fe4dd42dd)

### Bubble Sort with g++

![image](https://github.com/user-attachments/assets/97c9d08a-c1b9-42c5-84b8-a2adbd1c4b4f)

### Draw a tringle
![image](https://github.com/RodrigoPAml/MancoLanguage/assets/41243039/dfed84e9-3503-4708-9886-6bb4fb3cc74b)

