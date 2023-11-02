# DFAutomaton

DFAutomaton is a library for building Deterministic Finite Automatons.

## _Quick overview_

The library helps to build automatons in both simple and flexible way. It has the following features:

- Provides convenient domain specific language to define fully generic states graphs
- Allows defining transitions between connected states in either a constant state change or an application of a state reduction function
- Gives possibility to define static transitions having next state predefined or dynamic transitions having next state calculated by a more sophisticated form of reducers
- Builds automaton that can be run over a sequence of transition values and transforming a start state value into a final accepted state value