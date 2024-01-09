# DFAutomaton

DFAutomaton is a library for building Deterministic Finite Automata.

## Quick overview

This library helps to build automata in a simple and flexible way. It contains the following features:

- Defining fully generic graph of automaton states with convenient language
- Building an automaton that will be run over a sequence of transitions transforming a start value into an accepted final state value
- Defining transitions between states as either a constant value change or as a result of reduction function invocation
- Defining static transitions with fixed next state or dynamic ones having next state calculated by a more sophisticated form of reduction function