# Manco language 

Manco is my own compiled programming language maded from scratch for fun. 

No external tools was used like yacc, bison, etc.

Implemented in C#, with a GUI to interact in Windows Forms.

## Preview
In the left there is the code, and in the right the assembly generated. The compiler output and program output are in bottom.

![image](https://github.com/RodrigoPAml/MancoLanguage/assets/41243039/ac3ac22d-f5cb-4ad9-9aed-845d563f25a4)

Error in the editor

![image](https://github.com/RodrigoPAml/MancoLanguage/assets/41243039/fcdcb4ac-7b42-4335-b7f1-0df998aef81f)

## Features
- Primitive types like integer, decimal, bool and string
- Array of primitives types with index access
- Functions that can recieve primitives as value or by reference
- Strings and list are always passed by reference
- While loop, if, elif and else
- Functions do not return values, instead use reference values for return
- The language only uses the stack, no heap is used, wich means no dynamic allocation, and fixed array sizes
  
## Compiled or Interpreted?

  The language is compiled into a set of assembly based on the mips architecture, with do not runs on current computers, but runs on my Assembler Simulator (used as a submodule) that
  i have developed. I choose this type of assembly because already learn it from university. So while the language compiles to assembly, the assembly is getting interpreted, wich is slow, and also it kinda waits the GUI to render after a syscall, wich make worse, but can be disabled.

## Compilation Phases

### 1. Lexical Analysis

- **Description:** This phase analyzes the source code's stream of characters and converts it into meaningful tokens.
- **Purpose:** To break down the source code into individual tokens such as keywords, identifiers, operators, etc.
- **Implementation Details:** A Lexical parser use a regex to identify the tokens.

### 2. Syntax Analysis (Parsing)

- **Description:** This phase parses the sequence of tokens generated by the lexical analyzer according to the language's grammar rules. This phase do not check for meaning of the code, only for syntax. For example, it will not check if a variable exists, only if the rules to an assign is valid.
- **Purpose:** To ensure that the source code follows the correct syntactic structure specified by the language grammar.
- **Implementation Details:** Tokens are parsed, and each line is validated acording to a tree-like structure (like a finite state machine), also a stack is also used to control the open and close of scopes

### 3. Semantic Analysis

- **Description:** This phase examines the syntactic structure of the program to enforce the language's semantics, including type checking and scope analysis. For example in the syntax phase comparing a integer to a bool is valid, but in this phase is not.
- **Purpose:** To ensure that the program adheres to the language's semantic rules beyond its syntax.
- **Implementation Details:** Tokens are parsed, and each line is validated acording to a tree-like structure, also a stack is also used to control the open and close of scopes. But in this phase if validate if the operations make sense. Like if a variable exists, and if the assign values match the expected type.

### 4. Assembly generation (Compilation final phase)

- **Description:** This phase transforms the validated source code into an assembly code in MIPS based architecture
- **Purpose:** To provide code that runs direcly into the CPU
- **Implementation Details:** The compiler also parse each line in a tree-like structure, storing information on where each operation will be on the stack, while also inserting the code, its hard to explain, i guess.

### 5. Optimization

- **Description:** No optimization implemented, the assembly generated is very big and inefficient, but it kinda works.
- **Purpose:** To produce optimized code that executes faster or consumes fewer resources.

### 6. Execution

- **Description:** The execution of the assembler is done by a Assembler Simulator with is a public repository of mine, it is used as a submodule of this project.
- **Purpose:** To produce the final output that can be executed by the target platform.

## Examples
Some examples available in the folder to open and run.
  
### Hello world
![image](https://github.com/RodrigoPAml/MancoLanguage/assets/41243039/c2c9fe85-883c-447d-9053-05573505cc40)

### Prime numbers
![image](https://github.com/RodrigoPAml/MancoLanguage/assets/41243039/d79d81b1-83d7-484f-ac27-9400a0b16fc2)

### Bubble Sort
![image](https://github.com/RodrigoPAml/MancoLanguage/assets/41243039/6c42934c-73da-4be2-829d-68faa7a0c286)


