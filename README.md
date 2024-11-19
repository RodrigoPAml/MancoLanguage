# Manco Language
<table>
  <tr>
    <td><img src="https://github.com/RodrigoPAml/MancoLanguage/assets/41243039/d73ac874-72e8-4f94-8ff1-1bc97daca54f" alt="Manco language logo" style="width: 100px; height: auto; margin-right: 20px;"></td>
    <td><h1 style="margin: 0; line-height: 1;">The Manco programming language</h1></td>
  </tr>
</table

Manco is my own compiled programming language made from scratch for fun that compiles to a MIPS based assembly architecture. 

No external tools were used like yacc, bison, etc.

Implemented in C#, with a GUI to interact in Windows Forms.

## Preview
In the left there is the code, and in the right the assembly generated and also tokens of the code. 

The compiler output and program output are in bottom.

![image](https://github.com/RodrigoPAml/MancoLanguage/assets/41243039/eacbdfed-38b3-4920-b7e9-2422cd8837ea)

Error in the editor and tokens on the left

![image](https://github.com/RodrigoPAml/MancoLanguage/assets/41243039/f0078c33-f456-4fd9-b01e-6cb63d11eb05)

### A nice GIF

![ezgif-7-945f170853](https://github.com/RodrigoPAml/MancoLanguage/assets/41243039/4e5b9709-5f1a-496b-9da9-5f148204bc6d)

# Running the project 

If you are going to build and run the project dont forget to do **git submodule init** and **git submodule update** before that and run the project **GUI** as the initial project.

The files examples are on the **Manco/Files**

Or you can run the executable in the Release folder that contains an Examples Folder to run.

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
  
### Program 1

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

### Program 2

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
## Running the API

To use the API provider to compile code, use the Manco Project and take a look at the main, there is an example of how to use the compiler to generate assembly. With the compiled code you case use the AssemblerEmulator project to run the code, or just use the GUI to have fun.

![image](https://github.com/user-attachments/assets/b7125036-4fa6-4d5f-a443-110118e2c3fc)

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

## Compiled or Interpreted?

  The language is compiled into a set of assembly based on the mips architecture, which does not run on current computers, but runs on my Assembler Simulator (used as a submodule) that
  i have developed. I choose this type of assembly because already learn it from university. So while the language is compiled into assembly, the assembly is getting interpreted, which is slow, it also waits the GUI to render after a syscall for print, which makes it worse, but can be disabled.

## Compilation Phases

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

### 4. Assembly generation (Compilation final phase)

- **Description:** This phase transforms the validated source code into an assembly code in MIPS based architecture
- **Purpose:** To provide code that runs direcly into the CPU
- **Implementation Details:** The compiler also parse each line in a tree-like structure, storing information on where each operation will be on the stack, while also inserting the assembly code and incrementing the stack usage. It read the tokens and identify the operation, the variables that are in the operations, in which scope/function/if we are doing it, and then write the code for that operation. Also when you finish a scope it do the proper cleaning of the stack, and much more.

### 5. Optimization

- **Description:** No optimization implemented, the assembly generated is very big and inefficient.
- **Purpose:** To produce optimized code that executes faster or consumes fewer resources.

### 6. Execution

- **Description:** The execution of the assembler is done by a Assembler Simulator which is a public repository of mine, this is used as a submodule of this project.
- **Purpose:** To produce the final output that can be executed by the target platform.

## Examples
Some examples available in the folder to open and run.
  
### Hello world
![image](https://github.com/RodrigoPAml/MancoLanguage/assets/41243039/7f8463c7-59a2-464a-84a0-9546daf771ee)

### Prime numbers
![image](https://github.com/RodrigoPAml/MancoLanguage/assets/41243039/d79d81b1-83d7-484f-ac27-9400a0b16fc2)

### Bubble Sort
![image](https://github.com/RodrigoPAml/MancoLanguage/assets/41243039/6c42934c-73da-4be2-829d-68faa7a0c286)

### Draw a tringle
![image](https://github.com/RodrigoPAml/MancoLanguage/assets/41243039/dfed84e9-3503-4708-9886-6bb4fb3cc74b)

