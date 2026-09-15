# UnionDemo 

A quick and simple demo showcasing how the union feature works in C# 15 

# How it Works

LookupUsers() in ConsoleInterface.cs declares a UserResult which is a Union which could return a type in a selection of types... However we do not know which type it will be. 

DisplayResult() also in ConsoleInterface.cs handles the different types the union can contain. 

The UserResult definition can be changed to take different types. 

# What is this and how do I play with it?

Simply put, a union can be one of multiple types that are contained in its definition.

Depending on what type the union contains, it is handled in DisplayResult() in different ways.

The GetOrAddUser() function in UserService.cs returns a union, with the type determined by the value it returns.

You can edit this and try handling different types for yourself! 

Simply change the UserResult's types in UserResult.cs and handle them in DisplayResult(). You can also edit GetOrAddUser() to return a value that works for your new types!

# Why should I care?

Unions let a function clearly define a limited set of types it can return. Therefore, possible outcomes are explicit and the compiler can enforce that they are handled.

Instead of returning an object and having to determine its type at runtime, you have one result with a known set of possible types that can be handled directly.
